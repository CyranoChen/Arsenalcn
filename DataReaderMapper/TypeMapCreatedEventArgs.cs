using System;

namespace DataReaderMapper
{
    public class TypeMapCreatedEventArgs : EventArgs
    {
        public TypeMapCreatedEventArgs(TypeMap typeMap)
        {
            TypeMap = typeMap;
        }

        public TypeMap TypeMap { get; private set; }
    }
}