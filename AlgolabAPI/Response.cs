using System;
using System.Collections.Generic;
using System.Text;

namespace AlgolabAPI
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Content { get; set; }
    }
}
