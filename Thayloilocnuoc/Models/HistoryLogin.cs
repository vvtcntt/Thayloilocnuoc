using System;
using System.Collections.Generic;

namespace Thayloilocnuoc.Models
{
    public partial class HistoryLogin
    {
        public int id { get; set; }
        public string Name { get; set; }
        public Nullable<int> idUser { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
