using System.Collections.Generic;

namespace Arsenalcn.Core
{
    // Abstract Factory
    public interface IViewerFactory<T> where T : class, IViewer, new()
    {
        T Single(Criteria criteria);

        List<T> All();

        List<T> All(IPager pager, string orderBy = null);

        List<T> Query(Criteria criteria);
    }
}
