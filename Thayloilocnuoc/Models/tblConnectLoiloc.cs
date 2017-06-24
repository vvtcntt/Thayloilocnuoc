using System;
using System.Collections.Generic;

namespace Thayloilocnuoc.Models
{
    public partial class tblConnectLoiloc
    {
        public int id { get; set; }
        public Nullable<int> idkh { get; set; }
        public Nullable<int> idll { get; set; }
        public string Note { get; set; }
        public string DateTime { get; set; }
    }
}
