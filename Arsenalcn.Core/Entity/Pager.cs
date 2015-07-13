using System;

namespace Arsenalcn.Core
{
    public class Pager : IPager
    {
        public short PagingSize { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public int TotalCount { get; set; }

        public Pager()
        {
            PagingSize = 10;
            CurrentPage = 0;
        }

        public Pager(int index)
        {
            PagingSize = 10;
            CurrentPage = index;
        }
    }

    public interface IPager
    {
        short PagingSize { get; set; }
        int CurrentPage { get; set; }
        int MaxPage { get; set; }
        int TotalCount { get; set; }
    }

}
