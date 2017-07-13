using System;
using System.Collections.Generic;

namespace Thayloilocnuoc.Models
{
    public partial class tblGroupProduct
    {
        public int Id { get; set; }
        public Nullable<int> idManu { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Keyword { get; set; }
        public Nullable<int> Ord { get; set; }
        public string Tag { get; set; }
        public string Level { get; set; }
        public Nullable<bool> Index { get; set; }
        public Nullable<bool> Priority { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<bool> Baogia { get; set; }
        public string Images { get; set; }
        public string Favicon { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<int> idUser { get; set; }
    }
}
