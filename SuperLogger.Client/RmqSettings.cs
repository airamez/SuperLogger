﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLogger.Client
{
    public class RmqSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
    }
}
