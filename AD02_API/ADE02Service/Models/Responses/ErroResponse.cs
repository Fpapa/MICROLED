using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADE02Service.Models.Responses
{    
    public class ErroResponse
    {
        public string message { get; set; }
        public string code { get; set; }
        public object field { get; set; }
        public object path { get; set; }
        public string tag { get; set; }
        public DateTime date { get; set; }
        public int status { get; set; }
        public object detail { get; set; }
        public string severity { get; set; }
        public ErroResponseInfo info { get; set; }
    }

    public class ErroResponseInfo
    {
        public string mnemonico { get; set; }
        public string sistema { get; set; }
        public string ambiente { get; set; }
        public string visao { get; set; }
        public object usuario { get; set; }
        public string url { get; set; }
        public object fluxo { get; set; }
    }
}
