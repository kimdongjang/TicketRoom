using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using TicketRoom.Models.Custom;
using UIKit;

namespace TicketRoom.iOS.Renderer
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            Thread.CurrentThread.Abort();
        }
    }
}