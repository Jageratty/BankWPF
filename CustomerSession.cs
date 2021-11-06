using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaBank
{
    public class CustomerSession
    {
        public string CustomerID { get; set; }
        public string CustomerPass { get; set; }
        public bool Authenticated { get; set; } = false;
    }
}
