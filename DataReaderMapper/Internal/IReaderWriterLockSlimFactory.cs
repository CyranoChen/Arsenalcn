namespace DataReaderMapper.Internal
{
    public interface IReaderWriterLockSlimFactory
    {
        IReaderWriterLockSlim Create();
    }
}