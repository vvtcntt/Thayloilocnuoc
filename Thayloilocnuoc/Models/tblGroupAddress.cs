using System;
using System.Collections.Generic;

namespace Thayloilocnuoc.Models
{
    public partial class tblGroupAddress
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Ord { get; set; }
    }
}
