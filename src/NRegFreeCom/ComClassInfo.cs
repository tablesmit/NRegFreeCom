namespace NRegFreeCom
{
    public class ComClassInfo:ComClrInfoBase
    {
        public IAssemblyInfo Assembly { get; set; }
        public string Class { get;  set; }
        public string ProgId { get; set; }
        public string ThreadingModel { get; set; }
 
        public string RuntimeVersion { get; set; }
        public string RuntimeEntryPoint { get; set; }


        public override   System.Runtime.InteropServices.ComTypes.TYPEKIND TypeKind { 
        	get { return System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_COCLASS;}
        }
    }
}