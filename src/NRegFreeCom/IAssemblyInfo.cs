namespace NRegFreeCom
{
    public interface IAssemblyInfo
    {
        string FullName { get; }
        IAssemblyNameInfo GetName();
    }
}