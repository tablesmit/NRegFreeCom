using System;

namespace NRegFreeCom
{
    internal class ClrComRegistryInfo
    {
        public System.Reflection.Assembly Assembly { get; set; }//TODO: avoid direct dependency on assembly to allow assembly touchless registration
        public string Class { get; set; }
        public string ProgId { get; set; }
        public string ThreadingModel { get; set; }
        public string Guid { get; set; }
        public string NetVersion { get; set; }
        public string NetEntryPoint { get; set; }

        public static ClrComRegistryInfo Create(Type t, string customProgId)
        {
            raiseErrorOnBadType(t);

            var reg = new ClrComRegistryInfo();

            reg.Assembly = t.Assembly;
            reg.Class = t.FullName;
            reg.ProgId = reg.Class;
            if (customProgId != null)
            {
                reg.ProgId = customProgId;
            }
            reg.ThreadingModel = "Both";
            reg.Guid = t.GUID.ToString("B").ToUpper();
            reg.NetVersion = "v4.0.30319";
            reg.NetEntryPoint = "mscoree.dll";
            return reg;
        }

        private static void raiseErrorOnBadType(Type t)
        {
            if (t == null || t.IsAbstract || t.IsCOMObject) //NOTE: may be more checks needed
                throw new ArgumentException("The non abstract CLR type must be specified.", "t");
        }

   
    }
}