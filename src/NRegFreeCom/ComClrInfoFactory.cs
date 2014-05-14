using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NRegFreeCom
{
    /// <summary>
    /// Creates COM related desciptions out of CLR types.
    /// </summary>
    public static class ComClrInfoFactory
    {

        public static ComClassInfo CreateClass(System.Reflection.Assembly reflectionAssembly, string fullClassName)
        {
            var type = reflectionAssembly.GetType(fullClassName, true);
            return CreateClass(type);
        }

        /// <summary>
        /// Creates COM class desciptions out of ComVisible CLR class type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ComClassInfo CreateClass(Type t)
        {
            raiseErrorOnBadClassType(t);
            var attrs = CustomAttributeData.GetCustomAttributes(t);
            if (!IsComVisible(attrs))
                throw new ArgumentException("The CLR type must be COM visible.", "t");

            var reg = new ComClassInfo();

            reg.Assembly = new AssemblyInfo(t.Assembly);
            reg.Class = t.FullName;
            //TODO: optimize string usage for many calls of this
            CustomAttributeData progIdAttr = getCustomAttribute(attrs, typeof(ProgIdAttribute));
            reg.ProgId = progIdAttr != null ? progIdAttr.ConstructorArguments.First().Value.ToString() : reg.Class;
            reg.ThreadingModel = "Both";//NOTE: looks like this is default for .NET 
            reg.Guid = t.GUID.ToString("B").ToUpper();
            reg.RuntimeVersion = t.Assembly.ImageRuntimeVersion;
            reg.RuntimeEntryPoint = "mscoree.dll";//TODO: change this depending on runtime version
            return reg;
        }

        private static CustomAttributeData getCustomAttribute(IEnumerable<CustomAttributeData> attrs, Type ofAttr)
        {
            var match = string.Format("[{0}", ofAttr.FullName);
            var attr = attrs.FirstOrDefault(x => x.ToString().StartsWith(match));
            return attr;
        }

        public static ITypeLibAttributes CreateTypeLib(System.Reflection.Assembly typeLib)
        {
            return new TypeLib(typeLib);
        }

        public static ComInterfaceInfo CreateInterface(Type t)
        {
            raiseErrorOnBadInterfaceType(t);
            var attrs = CustomAttributeData.GetCustomAttributes(t);// t.GetCustomAttributes() is not usable against reflection only assemblies, so getting data
            if (!IsComVisible(attrs))
                throw new ArgumentException("The CLR type must be COM visible.", "t");


            var reg = new ComInterfaceInfo();
            reg.TypeLib = new TypeLib(t.Assembly);
            reg.Guid = t.GUID.ToString("B").ToUpper();

            return reg;
        }

        private static void raiseErrorOnBadInterfaceType(Type t)
        {
            if (t == null || !t.IsInterface) //NOTE: may be more checks needed
                throw new ArgumentException("The  CLR interface type must be specified.", "t");
        }


        private static void raiseErrorOnBadClassType(Type t)
        {
            if (t == null || t.IsAbstract || t.IsCOMObject) //NOTE: may be more checks needed
                throw new ArgumentException("The non abstract CLR class type must be specified.", "t");
        }


        public static bool IsComVisible(Type t)
        {
            var attrs = CustomAttributeData.GetCustomAttributes(t);
            return IsComVisible(attrs);
        }

        public static bool IsComVisible(IEnumerable<CustomAttributeData> attrs)
        {
            var comVisibleMatch = string.Format("[{0}", typeof(ComVisibleAttribute).FullName);
            var combVisibleAttr = attrs.FirstOrDefault(x => x.ToString().StartsWith(comVisibleMatch));
            bool isComVisible = !(combVisibleAttr == null || (bool)combVisibleAttr.ConstructorArguments.First().Value == false);
            return isComVisible;
        }

        private sealed class TypeLib : ITypeLibAttributes
        {

            public TypeLib(System.Reflection.Assembly asm)
            {

                // Only 2 numbers sypported by COM instead of 4 in CLR
                var ver = asm.GetName().Version;
                Version = new Version(ver.Major, ver.Minor);

                var attrs = CustomAttributeData.GetCustomAttributes(asm);
                var attr = ComClrInfoFactory.getCustomAttribute(attrs, typeof(GuidAttribute));
                if (attr == null)
                    throw new ArgumentException("Assembly must have GuidAttribute defined", "asm");
                var guidValue = attr.ConstructorArguments.First().ToString();//raw, as is directly in code           
                Guid = new Guid(guidValue.Remove(guidValue.Length - 1, 1).Remove(0, 1));
            }

            public Version Version
            {
                get;
                private set;
            }

            public Guid Guid
            {
                get;
                private set;
            }

        }

        private sealed class AssemblyInfo : IAssemblyInfo
        {
            private readonly System.Reflection.Assembly _assembly;

            public AssemblyInfo(System.Reflection.Assembly assembly)
            {
                //TODO: get all values eagerly and release assembly 
                _assembly = assembly;

            }

            public string FullName { get { return _assembly.FullName; } }
            public IAssemblyNameInfo GetName()
            {
                return new AssemblyNameInfo(_assembly.GetName());
            }
        }

        private sealed class AssemblyNameInfo : IAssemblyNameInfo
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