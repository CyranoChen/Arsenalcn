using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Arsenalcn.Core
{
    public abstract class SearchModel<T>
    {
        public virtual void Search(IEnumerable<T> data)
        {
            Contract.Requires(Criteria != null);

            Criteria.GetPageSize();
            Criteria.SetTotalCount(data.Count());

            if (Criteria.TotalCount > Criteria.PagingSize && Criteria.MaxPage >= 0)
            {
                Data = data.Page(Criteria.CurrentPage, Criteria.PagingSize);
            }
            else
            {
                Data = data;
            }
        }

        public Criteria Criteria { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}
