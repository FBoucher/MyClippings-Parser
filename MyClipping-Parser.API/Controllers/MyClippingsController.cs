using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyClippings_Parser.Models;

namespace MyClippings_Parser.Controllers
{
    [Route("/api/[controller]")]
    [Produces("application/json")]
    public class MyClippingsController : Controller
    {
        private const string ClippingSeparator = "==========";
        private const string Line1RegexPattern = @"^(.+?)(?: \(([^)]+?)\))?$";

        [HttpPost]
        public ActionResult<IEnumerable<Clipping>> Post()
        {
            var clippings = new Collection<Clipping>();

            if (Request.Body.CanRead)
            {
                using (var sr = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    int lineNumber = 0;
                    string line = null;
                    int clippingLineNumber = 0;
                    Clipping clipping = new Clipping();

                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            lineNumber++;

                            if (line == ClippingSeparator)
                            {
                                clippings.Add(clipping);
                                clippingLineNumber = 0;
                                clipping = new Clipping();
                            }
                            else
                            {
                                clippingLineNumber++;
                            }

                            switch (clippingLineNumber)
                            {
                                case 1:
                                    ParseLine1(line, clipping);
                                    break;
                                case 2:
                                    ParseLine2(line, clipping);
                                    break;
                                case 4:
                                    ParseLine4(line, clipping);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error encountered parsing line " + lineNumber + ": " + ex.Message, ex);
                    }
                }
            }

            return clippings;
        }

        private static void ParseLine1(string line, Clipping clipping)
        {
            var match = Regex.Match(line, Line1RegexPattern);
            if (match.Success)
            {
                var bookName = match.Groups[1].Value.Trim();
                var author = match.Groups[2].Value.Trim();

                clipping.BookName = bookName;
                clipping.Author = author;
            }
            else
            {
                throw new Exception("Clipping Line 1 did not match regex pattern.");
            }
        }

        private static void ParseLine2(string line, Clipping clipping)
        {
            var split = line.Split(' ');

            switch (split[2])
            {
                case "Highlight":
                    clipping.ClippingType = ClippingType.Highlight;
                    break;
                case "Note":
                    clipping.ClippingType = ClippingType.Note;
                    break;
                case "Bookmark":
                    clipping.ClippingType = ClippingType.Bookmark;
                    break;
            }

            var hasPageNumber = line.Contains(" on Page ");
            var hasLocation = line.Contains(" Location ");

            var dtIdx = 9;
            var locIdx = 5;
            var pageIndex = 5; 

            if (hasPageNumber)
            {
                var pageNumber = split[pageIndex];
                clipping.Page = pageNumber;

                locIdx = 8;

                dtIdx = hasLocation ? 12 : 9;
            }

            if (hasLocation)
            {
                var location = split[locIdx];
                clipping.Location = location;
            }

            var dateAddedString = String.Join(" ", split[dtIdx], split[dtIdx + 1], split[dtIdx + 2], split[dtIdx + 3], split[dtIdx + 4], split[dtIdx + 5]);

            var dateAdded = DateTime.Parse(dateAddedString);

            clipping.DateAdded = dateAdded;
        }

        private static void ParseLine4(string line, Clipping clipping)
        {
            clipping.Text = line.Trim();
        }

    }

}
