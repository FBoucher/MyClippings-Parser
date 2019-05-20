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
        private DefaultHttpContext _httpContext;

        public MyClippingsControllerShould()
        {
            _httpContext = new DefaultHttpContext();
        }

        [Fact]
        public void ReturnEmptyBodyForEmptyPost()
        {
            // Arrange
            var controllerContext = new ControllerContext()
            {
                HttpContext = _httpContext,
            };

            MyClippingsController sut = 
                new MyClippingsController { ControllerContext = controllerContext };

            // Act
            var response = sut.Post();

            // Assert
            Assert.Empty(response.Value);
        }

        [Fact]
        public void ReturnSixClippings()
        {
            using (FileStream fs = File.OpenRead(@"DataFiles/CompleteClippings.txt"))
            {
                _httpContext.Request.Body = fs;
                var controllerContext = new ControllerContext()
                {
                    HttpContext = _httpContext,
                };

                MyClippingsController sut = new MyClippingsController { ControllerContext = controllerContext };

                // Act
                var response = sut.Post();

                // Assert
                Assert.Equal(6, response.Value.Count());
            }           
        }

        [Fact]
        public void ReturnCorrectAuthors()
        {
            using (FileStream fs = File.OpenRead(@"DataFiles/CompleteClippings.txt"))
            {
                _httpContext.Request.Body = fs;
                var controllerContext = new ControllerContext()
                {
                    HttpContext = _httpContext,
                };

                MyClippingsController sut = new MyClippingsController { ControllerContext = controllerContext };

                // Act
                var response = sut.Post();
                var authorNames = 
                    new List<string> { "null", "null", "null", "null", "Chris Noring", "Chris Noring" };

                // Assert
                Assert.Equal(authorNames, response.Value.Select(c => c.Author).ToList());
            }            
        }
    }
}
