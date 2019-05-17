using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyClippings_Parser.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace MyClipping_Parser.Tests
{
    public class MyClippingsControllerShould
    {
        [Fact]
        public void ReturnEmptyBodyForEmptyPost()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            MyClippingsController sut = new MyClippingsController { ControllerContext = controllerContext };

            // Act
            var response = sut.Post();

            // Assert
            Assert.Empty(response.Value);
        }

        [Fact]
        public void ReturnSixClippings()
        {
            // Arrange
            string sampleText = null;

            using (StreamReader sr = new StreamReader(@"DataFiles/CompleteClippings.txt"))
            {
                sampleText = sr.ReadToEnd();
            }

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            MyClippingsController sut = new MyClippingsController { ControllerContext = controllerContext };

            // Act
            var response = sut.Post();

            // Assert
            Assert.Equal(6,response.Value.Count());
        }

        [Fact]
        public void ReturnCorrectAuthors()
        {
            // Arrange
            string sampleText = null;

            using (StreamReader sr = new StreamReader(@"DataFiles/CompleteClippings.txt"))
            {
                sampleText = sr.ReadToEnd();
            }

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            MyClippingsController sut = new MyClippingsController { ControllerContext = controllerContext };

            // Act
            var response = sut.Post();
            var authorNames = new List<string> { "null", "null", "null", "null", "Chris Noring", "Chris Noring" };

            // Assert
            Assert.Equal(authorNames,response.Value.Select(c=>c.Author).ToList());
        }
    }
}
