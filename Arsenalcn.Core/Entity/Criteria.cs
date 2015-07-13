using System;

namespace Arsenalcn.Core
{
    public class Criteria : IPager
    {
        public Criteria() { PagingSize = GetPageSize(); }

        public string SearchKeyword { get; set; }
        public string SortByField { get; set; }
        public short PagingSize { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public int TotalCount { get; set; }

        public short GetPageSize()
        {
            return this.PagingSize > 0 ? this.PagingSize : (short)10;
        }
    }
}
