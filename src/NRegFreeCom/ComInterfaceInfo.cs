namespace NRegFreeCom
{
    public sealed class ComInterfaceInfo:ComClrInfoBase{
	   
        public override   System.Runtime.InteropServices.ComTypes.TYPEKIND TypeKind { 
            get { return System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_INTERFACE;}
        }
		
        public ITypeLibAttributes TypeLib {get;internal set;}

    }
}