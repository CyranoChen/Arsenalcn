using System.Reflection;

namespace DataReaderMapper.Internal
{
    public class FieldAccessor : FieldGetter, IMemberAccessor
    {
        private readonly ILazy<LateBoundFieldSet> _lateBoundFieldSet;

        public FieldAccessor(FieldInfo fieldInfo)
            : base(fieldInfo)
        {
            _lateBoundFieldSet = LazyFactory.Create(() => DelegateFactory.CreateSet(fieldInfo));
        }

        public void SetValue(object destination, object value)
        {
            _lateBoundFieldSet.Value(destination, value);
        }
    }
}