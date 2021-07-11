namespace Gamefreak130.Common.Booters
{
    using Gamefreak130.Common.Loggers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class Booter
    {
        private readonly List<string> mResourceNames;

        public Booter(params string[] resourceNames) => mResourceNames = resourceNames.ToList();

        protected void AddResource(string resourceName) => mResourceNames.Add(resourceName);

        protected string GetResourceAt(int index) => mResourceNames[index];

        protected abstract void LoadData();

        public void Boot()
        {
            try
            {
                LoadData();
            }
            catch (Exception e)
            {
                ExceptionLogger.sInstance.Log(e);
            }
        }

        protected static MethodInfo FindMethod(string methodName, Type defaultType)
        {
            if (methodName.Contains(","))
            {
                string[] array = methodName.Split(new[] { ',' });
                string typeName = array[0].Trim() + "," + array[1].Trim();
                Type type = Type.GetType(typeName, true);
                return type.GetMethod(array[2].Trim(), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }
            return defaultType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}
