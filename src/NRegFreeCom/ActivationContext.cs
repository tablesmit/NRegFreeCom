using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NRegFreeCom
{
    ///<summary>
    /// Used tune activation context stack of thread for intializing and loading SxS components.
    /// </summary>
    ///<seealso cref="Microsoft.Windows.ActCtx"/>
    ///<seealso href="http://www.atalasoft.com/blogs/spikemclarty/february-2012/dynamically-testing-an-activex-control-from-c-and"/>
    public class ActivationContext
    {

        /// <summary>
        /// Create an instance of a COM object given the GUID of its class
        /// and a filepath of a client manifest (AKA an application manifest.)
        /// </summary>
        /// <param name="guid">GUID = CLSID of the COM object, {NNNN...NNN}</param>
        /// <param name="manifest">full path of manifest to activate, should list the
        /// desired COM class as a dependentAssembly.</param>
        /// <returns>An instance of the specified COM class, or null.</returns>
        static public object CreateInstanceWithManifest(Guid guid, string manifest)
        {
            object comob = null;
            ActivationContext.UsingManifestDo(manifest, delegate()
                {
                    // Get the type object associated with the CLSID.
                    Type T = Type.GetTypeFromCLSID(guid);
                    
                    // Create an instance of the type:
                    comob = System.Activator.CreateInstance(T);
                });
            return comob;
        }

        public delegate void doSomething();

        static public void UsingManifestDo(string manifest, doSomething thingToDo)
        {
            ACTCTX context = new ACTCTX();
            context.cbSize = Marshal.SizeOf(typeof(ACTCTX));
            bool wrongContextStructure = (context.cbSize != 0x20 && IntPtr.Size == 4) // ensure stucture is right on 32 bits
                                  || (context.cbSize != 52 && IntPtr.Size == 8); // the same for 64 bits
            if (wrongContextStructure)
            {
                throw new Exception("ACTCTX.cbSize is wrong");
            }
            context.lpSource = manifest;

            IntPtr hActCtx = NativeMethods.CreateActCtx(ref context);
            if (hActCtx == (IntPtr)(-1))
            {
                throw new Win32Exception();
            }
            try // with valid hActCtx
            {
                IntPtr cookie = IntPtr.Zero;
                if (!NativeMethods.ActivateActCtx(hActCtx, out cookie))
                {
                    throw new Win32Exception();
                }
                try // with activated context
                {
                    thingToDo();
                }
                finally
                {
                    NativeMethods.DeactivateActCtx(0, cookie);
                }
            }
            finally
            {
                NativeMethods.ReleaseActCtx(hActCtx);
            }
        }

        public static void UsingAssemblyManifestDo(System.Reflection.Assembly assembly, doSomething action)
        {
            var manifest = assembly.Location + ".manifest";
            UsingManifestDo(manifest, action);
        }
    }
}