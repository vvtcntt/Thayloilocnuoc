using System;
using System.Collections.Generic;

namespace Thayloilocnuoc.Models
{
    public partial class tblLoiloc
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Age { get; set; }
        public Nullable<int> Ord { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
