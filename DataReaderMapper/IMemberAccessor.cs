namespace DataReaderMapper
{
    public interface IMemberAccessor : IMemberGetter
    {
        void SetValue(object destination, object value);
    }
}