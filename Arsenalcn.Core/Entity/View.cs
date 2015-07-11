using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Arsenalcn.Core
{
    public abstract class View<T>
    {
        public virtual void Search(IEnumerable<T> data)
        {
            Contract.Requires(Criteria != null);

            Criteria.PagingSize = Criteria.GetPageSize();
            Criteria.TotalCount = data.Count();

            if (Criteria.TotalCount > Criteria.PagingSize && Criteria.MaxPage >= 0)
            {
                Criteria.MaxPage = Criteria.TotalCount / Criteria.PagingSize;

                if (Criteria.CurrentPage > Criteria.MaxPage)
                { Criteria.CurrentPage = Criteria.MaxPage; }

                this.Data = data.Page(Criteria.CurrentPage, Criteria.PagingSize);
            }
            else
            {
                this.Data = data;
            }
        }

        public IEnumerable<T> Data { get; set; }

        public Criteria Criteria { get; set; }
    }
}
