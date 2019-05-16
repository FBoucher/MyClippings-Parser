using System;

namespace MyClipping_Parser.Tests
{
    public class SharedState
    {
        public Guid OurGuid {get; private set;}

        public SharedState()
        {
            OurGuid = Guid.NewGuid();
        }
    }
}
