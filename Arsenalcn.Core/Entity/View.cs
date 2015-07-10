using System;
using System.Collections.Generic;
using System.Linq;

namespace Arsenalcn.Core
{
    public abstract class View<T> : IPager
    {
        public virtual void Query(Criteria criteria, IEnumerable<T> data)
        {
            PagingSize = criteria.GetPageSize();
            CurrentPage = criteria.CurrentPage;
            TotalCount = data.Count();

            if (TotalCount > PagingSize)
            {
                this.MaxPage = TotalCount / PagingSize;

                if (CurrentPage > MaxPage)
                { CurrentPage = MaxPage; }

                this.Data = data.Page(criteria.CurrentPage, PagingSize);
            }
            else
            {
                this.Data = data;
            }
        }

        public IEnumerable<T> Data { get; set; }

        public short PagingSize { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public int TotalCount { get; set; }
    }
}
