namespace DataReaderMapper.Internal
{
    public interface ILazy<T>
    {
        T Value { get; }
    }
}