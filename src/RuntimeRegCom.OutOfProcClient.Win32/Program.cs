using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using NRegFreeCom;
using RegFreeCom.Interfaces;

namespace RuntimeRegCom.OutOfProcClient.Win32
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("==============================CreateOutOfProcServerByManifst====================");
            //CreateOutOfProcServerByManifst();
            Console.WriteLine("==============================GetOutOfProcFromRot2====================");
            //GetOutOfProcFromRot2();
        }
        private static void GetOutOfProcFromRot2()
        {
            try
            {
                var id = new Guid(SimpleObjectId.ClassId);
                IMoniker clm;
                NativeMethods.CreateClassMoniker(ref id, out clm);
                object cl;
                IRunningObjectTable rot;
                NativeMethods.GetRunningObjectTable(0, out rot);
                var res = rot.GetObject(clm, out cl);
                var clo = Marshal.GetObjectForIUnknown((IntPtr)cl);
                var client = (ISimpleObject)clo;
                client.FloatProperty = 200;
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
                Guid clsid = new Guid(SimpleObjectId.ClassId);
                // var type = System.Type.GetTypeFromProgID("RegFreeCom.RegFreeLocalServer");
                //var type = System.Type.GetTypeFromCLSID(clsid);
                object obj = null;
                // obj = RegFreeCom.ActivationContext.CreateInstanceWithManifest(clsid, path);
                // obj = System.Activator.CreateInstance(type);
                NRegFreeCom.ActivationContext.UsingManifestDo(path, () =>
                {
                    obj = NRegFreeCom.NativeMethods.CoGetClassObject(clsid, CLSCTX.LOCAL_SERVER, IntPtr.Zero, new Guid(WELL_KNOWN_IIDS.IID_IUnknown));
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
