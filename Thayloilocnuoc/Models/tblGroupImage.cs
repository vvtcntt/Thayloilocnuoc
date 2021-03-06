using System;
using System.Collections.Generic;

namespace Thayloilocnuoc.Models
{
    public partial class tblGroupImage
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Tag { get; set; }
        public Nullable<int> Ord { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> idUser { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
    }
}
