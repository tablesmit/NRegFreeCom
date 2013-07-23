using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using NRegFreeCom;
using RegFreeCom.Interfaces;

namespace RegFreeCom.OutOfProcClient
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Calls Reg Free Out Of Proc object in other running process");
            Console.WriteLine("==============================GetActiveObject====================");
            GetActiveObject();
            //Console.WriteLine("==============================GetOutOfProcFromRot2====================");
            //GetOutOfProcFromRot2();
            Console.ReadKey();
        }

        private static void GetActiveObject()
        {
            try
            {
                object obj = null;

                //ActivationContext.UsingManifestDo("RegFreeCom.dll.manifest", () =>
                //{
                obj = Microsoft.VisualBasic.Interaction.GetObject(typeof(IRegFreeComRotClass).FullName);
                //obj = Marshal.GetActiveObject(typeof (IRegFreeComRotClass).FullName);
                //});

                UsingUsafePointers(obj);
                QueryInterfacesUsingManifest(obj);
                UsingReflection(obj);
            }
            catch (Exception ex)
            {
                Console.Write("Failed to get out of proc from ROT");
                Console.WriteLine(ex);
            }
        }

        private static void UsingUsafePointers(object obj)
        {
            var i = (IUnknown)obj;
            var g = new Guid(RotIds.IID);
            var face = obj as IRegFreeComRotClass; // QueryInterfaces
            if (face == null)
            {
                //Stack overflow
                //var ptr = i.QueryInterface(g);
                // var ooo = Marshal.GetObjectForIUnknown(ptr);
                //face = ooo as IRegFreeComRotClass;
            }
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

        private static void UsingReflection(object obj)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("---------------------------");
            var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.Interfaces.dll.manifest");

            var regFreeInvoker = RotRegFreeComInvoker.ProxyInterface<IRegFreeComRotClass>(obj);
            Console.WriteLine(regFreeInvoker.Answer());

            var prop = regFreeInvoker.Info;

            Console.WriteLine(string.Format("{0}", prop));

            NRegFreeCom.ActivationContext.UsingManifestDo(path, () =>
                {
                    object create = regFreeInvoker.Create();

                    var si2 = create as ISimpleObject;
                    Console.WriteLine("si2 " + si2 ?? si2.Info);
                }
                );
            Console.WriteLine("---------------------------");
        }



        private static void QueryInterfacesUsingManifest(object obj)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("---------------------------");
            IRegFreeComRotClass face = null;

            face = obj as IRegFreeComRotClass;
            if (face == null)
            {
                NRegFreeCom.ActivationContext.UsingManifestDo("RegFreeCom.Interfaces.dll.manifest", () =>
                {

                    face = obj as IRegFreeComRotClass;

                });

            }
  
            if (face != null)
            {
                Console.WriteLine(typeof(IRegFreeComRotClass));
                Console.WriteLine(face.Info);
                face.Ping();
                face.Request("Hello from " + Process.GetCurrentProcess().ProcessName);
                try
                {
                    ISimpleObject sf = face.Create();
                    Console.WriteLine(typeof(ISimpleObject) + " " + sf.Info + " " + sf.FloatProperty);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            Console.WriteLine("---------------------------");
        }
    }
}
