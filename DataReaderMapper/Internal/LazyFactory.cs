using System;
using System.Threading;

namespace DataReaderMapper.Internal
{
    public static class LazyFactory
    {
        public static ILazy<T> Create<T>(Func<T> valueFactory) where T : class
        {
            return new LazyImpl<T>(valueFactory);
        }

        private sealed class LazyImpl<T> : ILazy<T>
            where T : class
        {
            private readonly object _lockObj = new object();
            private readonly Func<T> _valueFactory;
            private bool _isDelegateInvoked;

            private T m_value;

            public LazyImpl(Func<T> valueFactory)
            {
                _valueFactory = valueFactory;
            }

            public T Value
            {
                get
                {
                    if (!_isDelegateInvoked)
                    {
                        var temp = _valueFactory();
                        Interlocked.CompareExchange(ref m_value, temp, null);

                        var lockTaken = false;

                        try
                        {
                            Monitor.Enter(_lockObj);
                            lockTaken = true;

                            _isDelegateInvoked = true;
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                Monitor.Exit(_lockObj);
                            }
                        }
                    }

                    return m_value;
                }
            }
        }
    }
}