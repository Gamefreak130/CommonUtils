## Gamefreak130.CommonUtils
The aim of this project is to eventually consolidate all shared code and libraries across my Sims 3 mods into one project, with new and existing mods linking to individual modules from this source as needed.

More specifically, this project contains:

* Common procedures for mod injection, such as interaction injection and NRaas-style menus. Some UI and reflection code has been adapted from Battery's [C# Script Utility](https://modthesims.info/d/615096/c-script-utility.html) for my own purposes.

* Support for LINQ to Objects and extension methods when compiling against .NET Framework 2.0, adapted from [LINQBridge](https://github.com/atifaziz/LINQBridge).

* Modified versions of the core script assemblies which expose all classes and their members. The game's MONO intepreter has no concept of protection levels, so mods compiled against these so-called "unprotected" assemblies will still run without issue.
