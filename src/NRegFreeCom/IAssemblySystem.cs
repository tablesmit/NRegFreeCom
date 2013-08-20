namespace NRegFreeCom
{
    /// <summary>
    /// Makes working with native dlls as with .NET ones.
    /// </summary>
    public interface IAssemblySystem
    {
        /// <summary>
        /// Gets native libraries subdirectory of suitable processor architectue and bitness  for managed process (which can be Any Cpu).
        /// Can be imploed in XCOPY deployment were managed code depends upon native libraries.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        string GetAnyCpuPath(string directoryPath);

        /// <summary>
        /// Loads native dll into process.
        /// </summary>
        /// <param name="directoryPath">Full path to directory where dll located.</param>
        /// <param name="name">Name with extension of dll to load.</param>
        /// <returns></returns>
        IAssembly LoadFrom(string directoryPath, string name);

        /// <summary>
        /// Loads dll into process.You cannot execute code from an assembly that has been loaded such way.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/system.reflection.assembly.reflectiononlyloadfrom.aspx"/>
        IAssembly ReflectionOnlyLoadFrom(string path);

        /// <summary>
        /// Loads native dll into process.
        /// </summary>
        /// <param name="path">Full path to dll file.</param>
        /// <returns></returns>
        IAssembly LoadFrom(string path);

        /// <summary>
        /// Adds directoy to search pathes. 
        /// When this <see cref="AssemblySystem"/> loads new dll then dependecies of it are looked in added paths.
        /// This is  unsafe hack for XP (but works). Safe on Vista/Win7 with patch applied, it here on Win8.
        /// </summary>
        /// <param name="directory">Full path to directoy</param>
        /// <see href="http://support.microsoft.com/kb/2533623"/>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/ff919712.aspx"/>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms682586.aspx"/>
        void AddSearchPath(string directory);
    }
}