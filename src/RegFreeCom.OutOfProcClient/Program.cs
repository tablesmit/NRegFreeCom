using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using NRegFreeCom;
using RegFreeCom;
using RegFreeCom.Interfaces;
using ActivationContext = NRegFreeCom.ActivationContext;
using NativeMethods = NRegFreeCom.NativeMethods;

namespace CSExeCOMClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================CreateOutOfProcServerByManifst====================");
            CreateOutOfProcServerByManifst();
            Console.WriteLine("==============================CreateInProceServerByManifest====================");
            CreateInProceServerByManifest();
            Console.WriteLine("==============================CreateInProceServerByManifest2====================");
            CreateInProceServerByManifest2();
            Console.WriteLine("==============================GetOutOfProcFromRot====================");
            GetOutOfProcFromRot();
            Console.WriteLine("==============================GetOutOfProcFromRot2====================");
            GetOutOfProcFromRot2();
            Console.ReadKey();
            return;

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

        private static void GetOutOfProcFromRot()
        {
            try
            {
                object obj = null;

                //ActivationContext.UsingManifestDo("RegFreeCom.dll.manifest", () =>
                //{
                obj = Microsoft.VisualBasic.Interaction.GetObject(typeof(IRegFreeComRotClass).FullName);
                //});
       
                var i = (IUnknown)obj;
                var g = new Guid(Rotguid.IID);
                var face = obj as IRegFreeComRotClass;// QueryInterfaces
                if (face == null)
                {
                    //Stack overflow
                    //var ptr = i.QueryInterface(g);
                   // var ooo = Marshal.GetObjectForIUnknown(ptr);
                    //face = ooo as IRegFreeComRotClass;
                }
                NRegFreeCom.ActivationContext.UsingManifestDo("RegFreeCom.dll.manifest", () =>
                    {
                        if (face == null)
                        {
                            face = obj as IRegFreeComRotClass;
                        }
       
                    });
              
                if (face != null)
                {
                    Console.WriteLine(typeof(IRegFreeComRotClass));
                    Console.WriteLine(face.ProcName);
                    try
                    {
                       ISimpleObject sf = face.Create();
                       Console.WriteLine(typeof(ISimpleObject) + " " +sf.ProcName + " " +sf.FloatProperty );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        
                    }
                }
                object face2 = null;
                var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.dll.manifest");
                NRegFreeCom.ActivationContext.UsingManifestDo(path,() =>
                    {
                        face2 = obj as IRegFreeComRotClass;
                        Console.WriteLine("face2 =" + face2);
                    }
                    );
                var prop = obj.GetType()
                            .InvokeMember("ProcName", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null,
                                          obj, null);
            
                Console.WriteLine(string.Format("{0}", prop));

                NRegFreeCom.ActivationContext.UsingManifestDo(path, () =>
                    {
                        object create = obj.GetType()
                                           .InvokeMember("Create",
                                                         BindingFlags.InvokeMethod | BindingFlags.Public |
                                                         BindingFlags.Instance, null,
                                                         obj, null);

                        var si2 = create as ISimpleObject;
                        Console.WriteLine("si2 " + si2  ?? si2.ProcName);
                    }
                    );


            }
            catch (Exception ex)
            {
                Console.Write("Failed to get out of proc from ROT");
                Console.WriteLine(ex);
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
                Guid clsid= new Guid(SimpleObjectId.ClassId);
                // var type = System.Type.GetTypeFromProgID("RegFreeCom.RegFreeLocalServer");
                //var type = System.Type.GetTypeFromCLSID(clsid);
                object obj = null;
                // obj = RegFreeCom.ActivationContext.CreateInstanceWithManifest(clsid, path);
                // obj = System.Activator.CreateInstance(type);
                NRegFreeCom.ActivationContext.UsingManifestDo(path,() =>
                    {
                         obj = NRegFreeCom.NativeMethods.CoGetClassObject(clsid, CLSCTX.LOCAL_SERVER, IntPtr.Zero, new Guid(NativeMethods.IID_IUnknown));
                    }
                    );

                
                var face = obj as ISimpleObject;
                if (face != null)
                {
                    uint pid, tid;

                    Console.WriteLine(face.ProcName);
                }
                    
            }
            catch (Exception ex)
            {
                Console.Write("Failed to create by manifest");
                Console.WriteLine(ex);
            }

        }
        private static void CreateInProceServerByManifest2()
        {
            try
            {
                //var actCtxType = System.Type.GetTypeFromProgID("Microsoft.Windows.ActCtx");
                //dynamic actCtx = System.Activator.CreateInstance(actCtxType);
                var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.dll.manifest");
                //actCtx.Manifest = path;

                // var type = System.Type.GetTypeFromProgID("RegFreeCom.RegFreeLocalServer");
                //var type = System.Type.GetTypeFromCLSID(new Guid("9C21B7EB-7E27-4405-BCE0-62B338DF83BB"));
                var obj = NRegFreeCom.ActivationContext.CreateInstanceWithManifest(new Guid(SimpleObjectId.ClassId), path);
                //object obj = System.Activator.CreateInstance(type);
                var face = (ISimpleObject)obj;
                Console.WriteLine(face.ProcName);
            }
            catch (Exception ex)
            {
                Console.Write("Failed to create by manifest");
                Console.WriteLine(ex);
            }

        }

        private static void CreateInProceServerByManifest()
        {
            try
            {
                //var actCtxType = System.Type.GetTypeFromProgID("Microsoft.Windows.ActCtx");
                //dynamic actCtx = System.Activator.CreateInstance(actCtxType);
                var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.dll.manifest");
                //actCtx.Manifest = path;

                // var type = System.Type.GetTypeFromProgID("RegFreeCom.RegFreeLocalServer");
                //var type = System.Type.GetTypeFromCLSID(new Guid("9C21B7EB-7E27-4405-BCE0-62B338DF83BB"));
                var obj = NRegFreeCom.ActivationContext.CreateInstanceWithManifest(new Guid("9C21B7EB-7E27-4405-BCE0-62B338DF83BB"), path);
                //object obj = System.Activator.CreateInstance(type);
                var face = (IRegFreeCom)obj;
                Console.WriteLine(face.GetString(42));
            }
            catch (Exception ex)
            {
                Console.Write("Failed to create by manifest");
                Console.WriteLine(ex);
            }

        }


    }
}
