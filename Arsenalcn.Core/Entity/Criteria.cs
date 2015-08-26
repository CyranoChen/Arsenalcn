namespace Arsenalcn.Core
{
    public class Criteria : IPager
    {
        public Criteria() { GetPageSize(); }

        public short PagingSize { get; set; }
        public int CurrentPage { get; set; }

        public int MaxPage { get; private set; }
        public int TotalCount { get; private set; }

        public string SearchKeyword { get; set; }
        public string SortByField { get; set; }

        public void GetPageSize()
        {
            if (PagingSize <= 0) { PagingSize = 10; }
        }

        public void SetTotalCount(int value)
        {
            TotalCount = value;

            GetPageSize();

            MaxPage = TotalCount / PagingSize;

            if (CurrentPage > MaxPage)
            { CurrentPage = MaxPage; }
        }

    }
}
