using MyClippings_Parser.Models;
using System;
using Xunit;

namespace MyClipping_Parser.Tests
{
    public class ClippingShould
    {
        [Fact, Trait("Category", "Null Output Tests")]
        public void ReturnNullBeginningForNullInput()
        {
            // Arrange
            var sut = new Clipping();

            // Act
            sut.Page = null;
            var beginning = sut.BeginningPage;

            //Assert
            Assert.Null(beginning);
        }

        [Fact, Trait("Category", "Null Output Tests")]
        public void ReturnNullBeginningForEmptyInput()
        {
            // Arrange
            var sut = new Clipping();

            // Act
            sut.Page = string.Empty;
            var beginning = sut.BeginningPage;

            //Assert
            Assert.Null(beginning);
        }

        [Fact, Trait("Category", "Null Output Tests")]
        public void ReturnNullBeginningForWhiteSpaceInput()
        {
            // Arrange
            var sut = new Clipping();

            // Act
            sut.Page = " ";
            var beginning = sut.BeginningPage;

            //Assert
            Assert.Null(beginning);
        }

        [Theory]
        [InlineData(null,null)]
        [InlineData("",null)]
        [InlineData(" ", null)]
        public void ReturnNull(string input, int? output)
        {
            var sut = new Clipping();

            sut.Page = input;
            var beginning = sut.BeginningPage;

            Assert.Equal(beginning,output);
        }

        [Fact, Trait("Category", "Input/Output Tests")]
        public void ReturnFiveBeginningForFiveInput()
        {
            // Arrange
            var sut = new Clipping();

            // Act
            sut.Page = "5";
            var beginning = sut.BeginningPage;

            //Assert
            Assert.Equal(5,beginning);
        }

        [Fact, Trait("Category", "Input/Output Tests")]
        public void ReturnTwelveBeginningForTwelveToSixteenInput()
        {
            // Arrange
            var sut = new Clipping();

            // Act
            sut.Page = "12-16";
            var beginning = sut.BeginningPage;

            //Assert
            Assert.Equal(12, beginning);
        }

        [Theory]
        [InlineData("5",5)]
        [InlineData("12-16",12)]
        public void ReturnCorrectBeginningForGivenInput(string input,int? output)
        {
            var sut = new Clipping() { Page = input };

            var beginning = sut.BeginningPage;

            Assert.Equal(output, beginning);
        }

        [Fact]
        public void ThrowExceptionForInvalidRange()
        {
            // Arrange
            var sut = new Clipping();

            // Act & Assert
            sut.Page = "1X-1Y";            
            Assert.Throws<ArgumentException>(()=>sut.BeginningPage);
        }
    }
}
