namespace Gamefreak130.Common.Helpers
{
    using System;
    using System.Reflection;

    public static partial class ReflectionEx
    {
        /// <summary>
        /// Given a method name and containing type name, find and invoke the associated parameterless public static method, discarding any return values
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        public static void StaticInvoke(string assemblyQualifiedTypeName, string methodName)
            => StaticInvoke<object>(Type.GetType(assemblyQualifiedTypeName), methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a method name and containing type, find and invoke the associated parameterless public static method, discarding any return values
        /// </summary>
        /// <param name="type">The type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        public static void StaticInvoke(Type type, string methodName)
            => StaticInvoke<object>(type, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a method name, arguments, and containing type name, find and invoke the associated public static method, discarding any return values
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="args">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="argTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        public static void StaticInvoke(string assemblyQualifiedTypeName, string methodName, object[] args, Type[] argTypes)
            => StaticInvoke<object>(Type.GetType(assemblyQualifiedTypeName), methodName, args, argTypes);

        /// <summary>
        /// Given a method name, arguments, and containing type, find and invoke the associated public static method, discarding any return values
        /// </summary>
        /// <param name="type">The type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="args">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="argTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        public static void StaticInvoke(Type type, string methodName, object[] args, Type[] argTypes)
            => StaticInvoke<object>(type, methodName, args, argTypes);

        /// <summary>
        /// Given a method name, containing type name, and return type, find and invoke the associated parameterless public static method and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T StaticInvoke<T>(string assemblyQualifiedTypeName, string methodName)
            => StaticInvoke<T>(Type.GetType(assemblyQualifiedTypeName), methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a method name, containing type, and return type, find and invoke the associated parameterless public static method and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="type">The type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T StaticInvoke<T>(Type type, string methodName)
            => StaticInvoke<T>(type, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a method name, arguments, containing type name, and return type, find and invoke the associated public static method and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="args">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="argTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T StaticInvoke<T>(string assemblyQualifiedTypeName, string methodName, object[] args, Type[] argTypes)
            => StaticInvoke<T>(Type.GetType(assemblyQualifiedTypeName), methodName, args, argTypes);

        /// <summary>
        /// Given a method name, arguments, containing type, and return type, find and invoke the associated public static method and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="args">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="argTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T StaticInvoke<T>(Type type, string methodName, object[] args, Type[] argTypes)
            => type is null
            ? throw new ArgumentNullException("type")
            : type.GetMethod(methodName, argTypes) is not MethodInfo method
            ? throw new MissingMethodException("No public method found in type with specified name and args")
            : (T)method.Invoke(null, args);
    }
}
