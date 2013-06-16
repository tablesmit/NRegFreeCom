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
                UsingRealProxy(obj);
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

        private static void UsingReflection(object obj)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("---------------------------");
            var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.Interfaces.dll.manifest");


            var prop = obj.GetType()
                          .InvokeMember("Info", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null,
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
                    Console.WriteLine("si2 " + si2 ?? si2.Info);
                }
                );
            Console.WriteLine("---------------------------");
        }

        private static void UsingRealProxy(object com)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("---------------------------");
            var regFreeInvoker = RotRegFreeComInvoker.ProxyInterface<IRegFreeComRotClass>(com);
            Console.WriteLine(regFreeInvoker.Answer());
            Console.WriteLine("---------------------------");
        }

        private static void QueryInterfacesUsingManifest(object obj)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("---------------------------");
            IRegFreeComRotClass face = null;
            NRegFreeCom.ActivationContext.UsingManifestDo("RegFreeCom.Interfaces.dll.manifest", () =>
                {

                    face = obj as IRegFreeComRotClass;

                });

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
