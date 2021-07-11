namespace Gamefreak130.Common.UI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A <see cref="SetSimpleValueObject{T}"/> that sets the value of a readable and writable property in a given <see cref="object"/>
    /// </summary>
    /// <typeparam name="T">The type of the given property</typeparam>
    public sealed class SetSimplePropertyObject<T> : SetSimpleValueObject<T> where T : IConvertible
    {
        public SetSimplePropertyObject(string menuTitle, string propertyName, Func<bool> test, object obj) : this(menuTitle, "", propertyName, test, obj)
        {
        }

        public SetSimplePropertyObject(string menuTitle, string dialogPrompt, string propertyName, Func<bool> test, object obj) : base(menuTitle, dialogPrompt, test)
        {
            PropertyInfo mProperty = obj.GetType().GetProperty(propertyName, typeof(T));
            if (mProperty is null)
            {
                throw new ArgumentException("Property with given return type not found in object");
            }
            if (!mProperty.CanWrite || !mProperty.CanRead)
            {
                throw new MissingMethodException("Property must have a get and set accessor");
            }
            mGetValue = () => (T)mProperty.GetValue(obj, null);
            mSetValue = (val) => mProperty.SetValue(obj, val, null);
            ConstructDefaultColumnInfo();
        }

        public SetSimplePropertyObject(string menuTitle, string propertyName, Func<bool> test, object obj, List<ColumnDelegateStruct> columns) : this(menuTitle, "", propertyName, test, obj, columns)
        {
        }

        public SetSimplePropertyObject(string menuTitle, string dialogPrompt, string propertyName, Func<bool> test, object obj, List<ColumnDelegateStruct> columns) : base(menuTitle, dialogPrompt, columns, test)
        {
            PropertyInfo mProperty = obj.GetType().GetProperty(propertyName);
            if (mProperty.PropertyType != typeof(T))
            {
                throw new ArgumentException("Type mismatch between property and return value");
            }
            if (!mProperty.CanWrite || !mProperty.CanRead)
            {
                throw new MissingMethodException("Property must have a get and set accessor");
            }
            mGetValue = () => (T)mProperty.GetValue(obj, null);
            mSetValue = (val) => mProperty.SetValue(obj, val, null);
        }
    }
}
