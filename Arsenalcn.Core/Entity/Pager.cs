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
            Index = 1;
        }

        public Pager(int index)
        {
            Size = 10;
            Index = index;
        }
    }
}
