﻿namespace Gamefreak130.Common.Helpers
{
    using System;
    using System.Reflection;

    public static partial class ReflectionEx
    {
        /// <summary>
        /// Given a type name and method name, construct the type using the parameterless constructor, find and invoke the type's associated parameterless public instance method, and discard any return values
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        public static void InstanceInvoke(string assemblyQualifiedTypeName, string methodName)
            => InstanceInvoke<object>(Type.GetType(assemblyQualifiedTypeName), new object[0], new Type[0], methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type and method name, construct the type using the parameterless constructor, find and invoke the type's associated parameterless public instance method, and discard any return values
        /// </summary>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        public static void InstanceInvoke(Type type, string methodName)
            => InstanceInvoke<object>(type, new object[0], new Type[0], methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given an object and method name, find and invoke the associated parameterless public instance method, discarding any return values
        /// </summary>
        /// <param name="obj">The object containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        public static void InstanceInvoke(object obj, string methodName)
            => InstanceInvoke<object>(obj, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type name and method name, construct the type using the given constructor arguments, find and invoke the type's associated parameterless public instance method, and discard any return values
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        public static void InstanceInvoke(string assemblyQualifiedTypeName, object[] ctorArgs, Type[] ctorArgTypes, string methodName)
            => InstanceInvoke<object>(Type.GetType(assemblyQualifiedTypeName), ctorArgs, ctorArgTypes, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type and method name, construct the type using the given constructor arguments, find and invoke the type's associated parameterless public instance method, and discard any return values
        /// </summary>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        public static void InstanceInvoke(Type type, object[] ctorArgs, Type[] ctorArgTypes, string methodName)
            => InstanceInvoke<object>(type, ctorArgs, ctorArgTypes, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type name, method name, and arguments, construct the type using the given constructor arguments, find and invoke the type's associated public instance method, and discard any return values
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        public static void InstanceInvoke(string assemblyQualifiedTypeName, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<object>(Type.GetType(assemblyQualifiedTypeName), new object[0], new Type[0], methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given a type, method name, and arguments, construct the type using the parameterless constructor, find and invoke the type's associated public instance method, and discard any return values
        /// </summary>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        public static void InstanceInvoke(Type type, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<object>(type, new object[0], new Type[0], methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given an object, method name, and arguments, find and invoke the associated public instance method, discarding any return values
        /// </summary>
        /// <param name="obj">The object containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        public static void InstanceInvoke(object obj, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<object>(obj, methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given a type name, method name, and arguments, construct the type using the given constructor arguments, find and invoke the type's associated public instance method, and discard any return values
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        public static void InstanceInvoke(string assemblyQualifiedTypeName, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<object>(Type.GetType(assemblyQualifiedTypeName), ctorArgs, ctorArgTypes, methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given a type, method name, and arguments, construct the type using the given constructor arguments, find and invoke the type's associated public instance method, and discard any return values
        /// </summary>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        public static void InstanceInvoke(Type type, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<object>(type, ctorArgs, ctorArgTypes, methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given a type name and method name, construct the type using the parameterless constructor, find and invoke the type's associated parameterless public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(string assemblyQualifiedTypeName, string methodName)
            => InstanceInvoke<T>(Type.GetType(assemblyQualifiedTypeName), new object[0], new Type[0], methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type and method name, construct the type using the parameterless constructor, find and invoke the type's associated parameterless public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(Type type, string methodName)
            => InstanceInvoke<T>(type, new object[0], new Type[0], methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given an object and method name, find and invoke the associated parameterless public instance method and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="obj">The object containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(object obj, string methodName)
            => InstanceInvoke<T>(obj, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type name and method name, construct the type using the given constructor arguments, find and invoke the type's associated parameterless public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(string assemblyQualifiedTypeName, object[] ctorArgs, Type[] ctorArgTypes, string methodName)
            => InstanceInvoke<T>(Type.GetType(assemblyQualifiedTypeName), ctorArgs, ctorArgTypes, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type and method name, construct the type using the given constructor arguments, find and invoke the type's associated parameterless public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(Type type, object[] ctorArgs, Type[] ctorArgTypes, string methodName)
            => InstanceInvoke<T>(type, ctorArgs, ctorArgTypes, methodName, new object[0], new Type[0]);

        /// <summary>
        /// Given a type name, method name, and arguments, construct the type using the given constructor arguments, find and invoke the type's associated public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(string assemblyQualifiedTypeName, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<T>(Type.GetType(assemblyQualifiedTypeName), new object[0], new Type[0], methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given a type, method name, and arguments, construct the type using the parameterless constructor, find and invoke the type's associated public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(Type type, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<T>(type, new object[0], new Type[0], methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given a type name, method name, and arguments, construct the type using the given constructor arguments, find and invoke the type's associated public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type containing the method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(string assemblyQualifiedTypeName, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => InstanceInvoke<T>(Type.GetType(assemblyQualifiedTypeName), ctorArgs, ctorArgTypes, methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given a type, method name, and arguments, construct the type using the given constructor arguments, find and invoke the type's associated public instance method, and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="type">The type containing the instance method</param>
        /// <param name="ctorArgs">An array of the arguments, in order, to pass to the type constructor; or an empty array if the constructor takes no arguments</param>
        /// <param name="ctorArgTypes">An array of the types of the arguments accepted by the type constructor, in order; or an empty array if the constructor takes no arguments</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(Type type, object[] ctorArgs, Type[] ctorArgTypes, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => type is null
            ? throw new ArgumentNullException("type")
            : type.GetConstructor(ctorArgTypes) is not ConstructorInfo ctor
            ? throw new MissingMethodException(type.FullName, ".ctor()")
            : InstanceInvoke<T>(ctor.Invoke(ctorArgs), methodName, methodArgs, methodArgTypes);

        /// <summary>
        /// Given an object, method name, and arguments, find and invoke the associated public instance method and return the result
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="obj">The object containing the method</param>
        /// <param name="methodName">The name of the method to invoke</param>
        /// <param name="methodArgs">An array of the arguments, in order, to pass to the method; or an empty array if the method takes no arguments</param>
        /// <param name="methodArgTypes">An array of the types of the arguments accepted by the method, in order; or an empty array if the method takes no arguments</param>
        /// <returns>The value returned by the method, or <see langword="null"/> if the method's return type is <see langword="void"/></returns>
        public static T InstanceInvoke<T>(object obj, string methodName, object[] methodArgs, Type[] methodArgTypes)
            => obj is null
            ? throw new ArgumentNullException("Instance object")
            : obj.GetType().GetMethod(methodName, methodArgTypes) is not MethodInfo method
            ? throw new MissingMethodException("No public method found in instance with specified name and args")
            : (T)method.Invoke(obj, methodArgs);
    }
}
