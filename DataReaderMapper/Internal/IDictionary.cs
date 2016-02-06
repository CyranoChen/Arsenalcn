using System;
using System.Collections.Generic;

namespace DataReaderMapper.Internal
{
    public interface IDictionary<TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
        ICollection<TValue> Values { get; }
        ICollection<TKey> Keys { get; }

        TValue AddOrUpdate(
            TKey key,
            TValue addValue,
            Func<TKey, TValue, TValue> updateValueFactory
            );

        bool TryGetValue(
            TKey key,
            out TValue value
            );

        TValue GetOrAdd(
            TKey key,
            Func<TKey, TValue> valueFactory
            );

        bool TryRemove(TKey key, out TValue value);
        void Clear();
        bool ContainsKey(TKey key);
    }


    public static class FeatureDetector
    {
        public static Func<Type, bool> IsIDataRecordType = t => false;
        private static bool? _isEnumGetNamesSupported;


        public static bool IsEnumGetNamesSupported
        {
            get
            {
                if (_isEnumGetNamesSupported == null)
                    _isEnumGetNamesSupported = ResolveIsEnumGetNamesSupported();

                return _isEnumGetNamesSupported.Value;
            }
        }

        private static bool ResolveIsEnumGetNamesSupported()
        {
            return typeof (Enum).GetMethod("GetNames") != null;
        }
    }
}