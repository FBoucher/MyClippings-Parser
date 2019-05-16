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
            var sampleText = @"Announcing WSL 2 | Windows Command Line Tools For Developers (null)
- Your Note on Location 68 | Added on Thursday, May 9, 2019 5:36:43 PM

Awesome news, and not only for  the docker part :).[dev.wdl2.wsl.linux.windows]
==========
﻿Announcing WSL 2 | Windows Command Line Tools For Developers (null)
- Your Highlight on Location 68-68 | Added on Thursday, May 9, 2019 5:36:43 PM

valued
==========
﻿Introducing Windows Terminal | Windows Command Line Tools For Developers (null)
- Your Note on Location 85 | Added on Thursday, May 9, 2019 5:38:48 PM

Awesome new terminal with a  kitass look. Even more it's an open source project![dev.terminal.github]
==========
﻿Introducing Windows Terminal | Windows Command Line Tools For Developers (null)
- Your Highlight on Location 85-85 | Added on Thursday, May 9, 2019 5:38:48 PM

Terminal
==========
﻿Improve your Dockerfile, best practices (Chris Noring)
- Your Highlight on Location 120-120 | Added on Thursday, May 9, 2019 8:45:34 PM

There are many more best practices
==========
﻿Improve your Dockerfile, best practices (Chris Noring)
- Your Note on Location 120 | Added on Thursday, May 9, 2019 8:47:25 PM

Nice quick post about some really easy best practices.It's so simple why would  you not  follow them.[dev.docker.bestpractices]
==========";

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
    }
}
