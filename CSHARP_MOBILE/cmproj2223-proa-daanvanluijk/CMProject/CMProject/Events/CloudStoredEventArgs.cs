using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Events
{
    public class CloudStoredEventArgs : EventArgs
    {
        public string Message { get; set; }

        public CloudStoredEventArgs(string message)
        {
            Message = message;
        }
    }
}
