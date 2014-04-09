using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NRegFreeCom
{
    public class ClrComRegistryInfo
    {
        public IAssemblyInfo Assembly { get; set; }//TODO: avoid direct dependency on assembly to allow assembly touchless registration
        public string Class { get; set; }
        public string ProgId { get; set; }
        public string ThreadingModel { get; set; }
        public string Guid { get; set; }
        public string NetVersion { get; set; }
        public string NetEntryPoint { get; set; }

        public static ClrComRegistryInfo Create(Type t)
        {
            raiseErrorOnBadType(t);
            var attrs = CustomAttributeData.GetCustomAttributes(t);// GetCustomAttributes() is not usable against reflection only assemblies
             raiseErrorOnBadAttrs(attrs);

            var reg = new ClrComRegistryInfo();

            reg.Assembly = new AssemblyInfo(t.Assembly);
            reg.Class = t.FullName;
            //TODO: optimize string usage for many calls of this
            var progIdMatch = string.Format("[{0}", typeof(ProgIdAttribute).FullName);
            var progIdAttr = attrs.FirstOrDefault(x => x.ToString().StartsWith(progIdMatch));
            reg.ProgId = progIdAttr != null ? progIdAttr.ConstructorArguments.First().Value.ToString() : reg.Class;
            reg.ThreadingModel = "Both";
            reg.Guid = t.GUID.ToString("B").ToUpper();
            reg.NetVersion = "v4.0.30319";
            reg.NetEntryPoint = "mscoree.dll";
            return reg;
        }

        private static void raiseErrorOnBadAttrs(IList<CustomAttributeData> attrs)
        {
            var comVisibleMatch = string.Format("[{0}",typeof(ComVisibleAttribute).FullName);
            var combVisibleAttr = attrs.FirstOrDefault(x => x.ToString().StartsWith(comVisibleMatch));
            if (combVisibleAttr == null)
                throw new ArgumentException("The CLR type must be COM visible.", "t");
            if ((bool)combVisibleAttr.ConstructorArguments.First().Value == false)
                throw new ArgumentException("The CLR type must be COM visible.", "t");
        }

        private static void raiseErrorOnBadType(Type t)
        {
            if (t == null || t.IsAbstract || t.IsCOMObject) //NOTE: may be more checks needed
                throw new ArgumentException("The non abstract CLR type must be specified.", "t");
        }


        public static ClrComRegistryInfo Create(string assemblyLocation, string fullClassName)
        {
            var asm = System.Reflection.Assembly.ReflectionOnlyLoadFrom(assemblyLocation);
            var type = asm.GetType(fullClassName,true);
            return Create(type);
        }

        private class AssemblyInfo : IAssemblyInfo
        {
            private readonly System.Reflection.Assembly _assembly;

            public AssemblyInfo(System.Reflection.Assembly assembly)
            {
                _assembly = assembly;

            }

            public string FullName { get { return _assembly.FullName; } }
            public IAssemblyNameInfo GetName()
            {
                return new AssemblyNameInfo(_assembly.GetName());
            }
        }

        private class AssemblyNameInfo : IAssemblyNameInfo
        {
            private AssemblyName _name;

            public AssemblyNameInfo(AssemblyName name)
            {
                _name = name;
            }

            public Version Version { get { return _name.Version; } }
        }
    }
}