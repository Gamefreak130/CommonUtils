namespace Gamefreak130.Common.Helpers
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionEx
    {
        public static bool IsAssemblyLoaded(string str, bool matchExact = true)
            => AppDomain.CurrentDomain.GetAssemblies()
                                      .Any(assembly => matchExact
                                                    ? assembly.GetName().Name == str
                                                    : assembly.GetName().Name.Contains(str));

        public static void StaticInvoke(string assemblyQualifiedTypeName, string methodName, object[] args, Type[] argTypes) => StaticInvoke(Type.GetType(assemblyQualifiedTypeName), methodName, args, argTypes);

        public static void StaticInvoke(Type type, string methodName, object[] args, Type[] argTypes)
        {
            if (type is null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.GetMethod(methodName, argTypes) is not MethodInfo method)
            {
                throw new MissingMethodException("No method found in type with specified name and args");
            }
            method.Invoke(null, args);
        }

        public static T StaticInvoke<T>(string assemblyQualifiedTypeName, string methodName, object[] args, Type[] argTypes) => StaticInvoke<T>(Type.GetType(assemblyQualifiedTypeName), methodName, args, argTypes);

        public static T StaticInvoke<T>(Type type, string methodName, object[] args, Type[] argTypes)
            => type is null
            ? throw new ArgumentNullException("type")
            : type.GetMethod(methodName, argTypes) is not MethodInfo method
            ? throw new MissingMethodException("No method found in type with specified name and args")
            : (T)method.Invoke(null, args);

        public static void InstanceInvoke(string assemblyQualifiedTypeName, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke(Type.GetType(assemblyQualifiedTypeName), ctorArgs, ctorArgTypes, methodName, methodArgs, methodArgTypes);

        public static void InstanceInvoke(Type type, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
        {
            if (type is null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.GetConstructor(ctorArgTypes) is not ConstructorInfo ctor)
            {
                throw new MissingMethodException(type.FullName, ".ctor()");
            }
            InstanceInvoke(ctor.Invoke(ctorArgs), methodName, methodArgs, methodArgTypes);
        }

        public static void InstanceInvoke(object obj, string methodName, object[] args, Type[] argTypes)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("Instance object");
            }
            if (obj.GetType().GetMethod(methodName, argTypes) is not MethodInfo method)
            {
                throw new MissingMethodException("No method found in instance with specified name and args");
            }
            method.Invoke(obj, args);
        }

        public static T InstanceInvoke<T>(string assemblyQualifiedTypeName, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<T>(Type.GetType(assemblyQualifiedTypeName), ctorArgs, ctorArgTypes, methodName, methodArgs, methodArgTypes);

        public static T InstanceInvoke<T>(Type type, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => type is null
            ? throw new ArgumentNullException("type")
            : type.GetConstructor(ctorArgTypes) is not ConstructorInfo ctor
            ? throw new MissingMethodException(type.FullName, ".ctor()")
            : InstanceInvoke<T>(ctor.Invoke(ctorArgs), methodName, methodArgs, methodArgTypes);

        public static T InstanceInvoke<T>(object obj, string methodName, object[] args, Type[] argTypes)
            => obj is null
            ? throw new ArgumentNullException("Instance object")
            : obj.GetType().GetMethod(methodName, argTypes) is not MethodInfo method
            ? throw new MissingMethodException("No method found in instance with specified name and args")
            : (T)method.Invoke(obj, args);
    }
}
