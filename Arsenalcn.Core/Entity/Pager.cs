using System;

namespace Arsenalcn.Core
{
    public class Pager
    {
        public short Size;
        public int Index;

        public Pager()
        {
            Size = 10;
            Index = 0;
        }

        public Pager(int index)
        {
            Size = 10;
            Index = index;
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
