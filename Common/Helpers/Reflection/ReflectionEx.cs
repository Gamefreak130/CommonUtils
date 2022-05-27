namespace Gamefreak130.Common.Helpers
{
    using System;
    using System.Linq;

    public static partial class ReflectionEx
    {
        /// <summary>
        /// Given all or part of an assembly name, check if the assembly is loaded
        /// </summary>
        /// <param name="str">The assembly name or assembly name substring to match on</param>
        /// <param name="matchExact"><see cref="true"/> if <paramref name="str"/> must be an entire assembly name; <see cref="false"/> if it can be a substring of an assembly name</param>
        /// <returns><see langword="true"/> if an assembly matching the search criteria is currently loaded; <see langword="false"/> otherwise</returns>
        public static bool IsAssemblyLoaded(string str, bool matchExact = true)
            => AppDomain.CurrentDomain.GetAssemblies()
                                      .Any(assembly => matchExact
                                                    ? assembly.GetName().Name == str
                                                    : assembly.GetName().Name.Contains(str));
    }
}
