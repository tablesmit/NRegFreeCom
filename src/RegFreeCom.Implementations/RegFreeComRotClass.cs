using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using RegFreeCom.Interfaces;

namespace RegFreeCom
{
    [ComVisible(true)]
    [Guid("581FBD61-BD82-4E2A-8200-519D90BBB1F7")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComDefaultInterface(typeof(IRegFreeComRotClass))]
    public class RegFreeComRotClass : IRegFreeComRotClass
    {
        private float _floatProperty = 100;
        [ComVisible(false)]
        public Func<byte[], byte[]> OnExecuted;

        //public int TestFunc(ref System.Runtime.InteropServices.ComTypes.DISPPARAMS dispParams, out object varResult)
        public object TestFunc(string a, int i)
        {
            return i + 1;
        }

        public float FloatProperty
        {
            get { return _floatProperty; }
            set { _floatProperty = value; }
        }

        public string ProcName { get { return Process.GetCurrentProcess().ProcessName + " " + Process.GetCurrentProcess().Id + " " + Thread.CurrentThread.ManagedThreadId; } }
        public ISimpleObject Create()
        {
            return new SimpleObject();
        }

        public byte[] Execute(byte[] request)
        {
            Func<byte[], byte[]> onExecuted = OnExecuted;
            return onExecuted(request);
        }
    }
}