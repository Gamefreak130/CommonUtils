## Gamefreak130.CommonUtils
The aim of this project is to eventually consolidate all shared code and libraries across my Sims 3 mods into one project, with new and existing mods linking to individual modules from this source as needed.

More specifically, this project contains:

* Common procedures for mod injection, such as interaction injection and NRaas-style menus. Some UI and reflection code has been adapted from Battery's [C# Script Utility](https://modthesims.info/d/615096/c-script-utility.html) for my own purposes.

* Syntactic sugar for the Task Asynchronous Pattern using the Simulator, including returning objects and awaiting their results, adapted from the [.NET Core source](https://github.com/dotnet/runtime). Note that the async/await keywords themselves are not supported.

* Support for LINQ to Objects and extension methods when compiling against .NET Framework 2.0, adapted from [LINQBridge](https://github.com/atifaziz/LINQBridge).

* Quick and dirty HashSet<T> implementation for .NET Framework 2.0 from [Chris Doggett](https://stackoverflow.com/questions/687034/using-hashset-in-c-sharp-2-0-compatible-with-3-5/711335#711335)

* Support for C# 7 value tuples when compiling against .NET Framework 2.0, adapted from [igor-tkachev](https://github.com/igor-tkachev/Portable.System.ValueTuple)

* Support for C# 8 indices and ranges when compiling against .NET Framework 2.0, adapted from [Stuart Lang](https://github.com/slang25/csharp-ranges-compat/)

* Modified versions of the core script assemblies which expose all classes and their members. The game's MONO intepreter has no concept of protection levels, so mods compiled against these so-called "unprotected" assemblies will still run without issue.
