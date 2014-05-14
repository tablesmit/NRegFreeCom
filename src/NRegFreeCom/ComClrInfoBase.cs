namespace NRegFreeCom
{
    public abstract class ComClrInfoBase
    {
	    
        public abstract  System.Runtime.InteropServices.ComTypes.TYPEKIND TypeKind { get; }
        public string Guid { get;  internal set; }
		

    }
}