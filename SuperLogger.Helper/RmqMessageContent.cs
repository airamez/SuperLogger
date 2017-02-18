using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLogger.Helper
{
    public class RmqMessageContent
    {
        public string LogType { get; set; }
        public string CorrelationID { get; set; }
        public string CreatedOn { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IDictionary<string, string> Data { get; set; }
    }
}
