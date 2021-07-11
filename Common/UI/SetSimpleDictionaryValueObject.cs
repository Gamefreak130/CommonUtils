namespace Gamefreak130.Common.UI
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="SetSimpleValueObject{T}"/> that sets the value of a given <typeparamref name="TKey"/> in a given <see cref="IDictionary{TKey, TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">The type of the given dictionary's keys</typeparam>
    /// <typeparam name="TValue">The type of the given dictionary's values</typeparam>
    public sealed class SetSimpleDictionaryValueObject<TKey, TValue> : SetSimpleValueObject<TValue> where TValue : IConvertible
    {
        public SetSimpleDictionaryValueObject(string menuTitle, IDictionary<TKey, TValue> dict, TKey key, Func<bool> test) : this(menuTitle, "", dict, key, test)
        {
        }

        public SetSimpleDictionaryValueObject(string menuTitle, string dialogPrompt, IDictionary<TKey, TValue> dict, TKey key, Func<bool> test) : base(menuTitle, dialogPrompt, test)
        {
            if (!dict.ContainsKey(key))
            {
                throw new ArgumentException("Key not in dictionary");
            }
            mGetValue = () => dict[key];
            mSetValue = (val) => dict[key] = val;
            ConstructDefaultColumnInfo();
        }

        public SetSimpleDictionaryValueObject(string menuTitle, IDictionary<TKey, TValue> dict, TKey key, List<ColumnDelegateStruct> columns, Func<bool> test) : this(menuTitle, "", dict, key, columns, test)
        {
        }

        public SetSimpleDictionaryValueObject(string menuTitle, string dialogPrompt, IDictionary<TKey, TValue> dict, TKey key, List<ColumnDelegateStruct> columns, Func<bool> test) : base(menuTitle, dialogPrompt, columns, test)
        {
            if (!dict.ContainsKey(key))
            {
                throw new ArgumentException("Key not in dictionary");
            }
            mGetValue = () => dict[key];
            mSetValue = (val) => dict[key] = val;
        }
    }
}
