using System.ComponentModel;

namespace DataReaderMapper.Internal
{
    public abstract class ProxyBase
    {
        protected void NotifyPropertyChanged(PropertyChangedEventHandler handler, string method)
        {
            handler?.Invoke(this, new PropertyChangedEventArgs(method));
        }
    }
}