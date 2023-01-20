using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Events
{
    public class CloudRetrievedEventArgs : EventArgs
    {
        public string Message { get; set; }

        public CloudRetrievedEventArgs(string message)
        {
            Message = message;
        }
    }
}
