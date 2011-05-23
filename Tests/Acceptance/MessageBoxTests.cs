using System;
using NUnit.Framework;
using VisualStudio.UI;

namespace Tests.Acceptance
{
    [TestFixture]
    public class MessageBoxTests
    {
        [Test]
        public void Display_Message_Box()
        {
            new MessageBox().DisplayMessage("Hello!", "The time is {0}.", DateTime.Now);
        }
    }
}
