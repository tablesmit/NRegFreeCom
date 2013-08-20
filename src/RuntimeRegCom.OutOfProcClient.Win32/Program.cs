using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using NRegFreeCom;
using NRegFreeCom.Interop;
using RegFreeCom.Interfaces;

namespace RuntimeRegCom.OutOfProcClient.Win32
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Thread {0}, Process:{1}, ProcesName:{2}", Thread.CurrentThread.ManagedThreadId, Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("==============================GetRegistrationServices====================");
            GetRegistrationServices();
            Console.WriteLine("==============================CreateOutOfProcServerByManifst====================");
            //CreateOutOfProcServerByManifst();
            Console.ReadKey();

        }

        private static void GetRegistrationServices()
        {
            try
            {
                NativeMethods.CoInitializeEx(IntPtr.Zero, CoInit.MultiThreaded);
                var clsid = new Guid(SimpleObjectId.ClassId);
                var iid = new Guid(SimpleObjectId.InterfaceId);
                object obj;
                //hangs if Enterprise Services registration
                //  NativeMethods.CoCreateInstance(clsid,null,CLSCTX.LOCAL_SERVER,iid,out obj);
                var type = Type.GetTypeFromCLSID(clsid);
                //hangs if Enterprise Services registration
                obj = System.Activator.CreateInstance(type);

                Console.WriteLine(type.GUID);
                Console.WriteLine(obj);
                var inf = (ISimpleObject)obj;

                Console.WriteLine(inf.Info);
                Marshal.ReleaseComObject(obj);
                Marshal.ReleaseComObject(inf);
                Marshal.FinalReleaseComObject(obj);
                Marshal.FinalReleaseComObject(inf);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }


        private static void CreateOutOfProcServerByManifst()
        {
            try
            {
                //var actCtxType = System.Type.GetTypeFromProgID("Microsoft.Windows.ActCtx");
                //dynamic actCtx = System.Activator.CreateInstance(actCtxType);
                var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.dll.manifest");
                //actCtx.Manifest = path;
                var clsid = new Guid(SimpleObjectId.ClassId);
                // var type = System.Type.GetTypeFromProgID("RegFreeCom.RegFreeLocalServer");
                //var type = System.Type.GetTypeFromCLSID(clsid);
                object obj = null;
                // obj = RegFreeCom.ActivationContext.CreateInstanceWithManifest(clsid, path);
                // obj = System.Activator.CreateInstance(type);
                NRegFreeCom.ActivationContext.UsingManifestDo(path, () =>
                {
                    obj = NativeMethods.CoGetClassObject(clsid, CLSCTX.LOCAL_SERVER, IntPtr.Zero, new Guid(WELL_KNOWN_IIDS.IID_IUnknown));
                }
                    );


                var face = obj as ISimpleObject;
                if (face != null)
                {
                    uint pid, tid;

                    Console.WriteLine(face.Info);
                }

            }
            catch (Exception ex)
            {
                Console.Write("Failed to create by manifest");
                Console.WriteLine(ex);
            }

        }
    }
}
