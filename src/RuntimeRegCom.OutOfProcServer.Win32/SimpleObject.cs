using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NRegFreeCom;
using RegFreeCom.Implementations;
using RegFreeCom.Interfaces;

namespace RuntimeRegCom.OutOfProcServer.Win32
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComSourceInterfaces(typeof(ISimpleObjectEvents))]
    [Guid(SimpleObjectId.ClassId)]
    [ComDefaultInterface(typeof(ISimpleObject))]
    [ComVisible(true)]
    public class SimpleObject : ReferenceCountedObject, ISimpleObject
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            Regasm.RegisterLocalServer(t);

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            Regasm.UnregisterLocalServer(t);
        }

        private float fField = 0;


        public float FloatProperty
        {
            get
            {
                new RunningObjectTable().PrintRot();
                return this.fField;

            }
            set
            {
                bool cancel = false;
                // Raise the event FloatPropertyChanging
                if (null != FloatPropertyChanging)
                    FloatPropertyChanging(value, ref cancel);
                if (Callbacks != null)
                {
                    Callbacks.FloatPropertyChanging(value, ref cancel);
                }
                if (!cancel)
                    this.fField = value;
            }
        }

        public string Info
        {
            get
            {
                return
                    string.Format("Type:{0}, ProcessName:{1}, ProcessId:{2},ThreadId:{3}",
                                  this.GetType().FullName, Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void RaisePassStruct()
        {
            var val = new MyCoolStuct { _Val = 123, _Val2 = "312" };
            if (Callbacks != null)
            {
                Callbacks.PassStuct(val);
            }
            if (PassStuct != null)
            {
                PassStuct(val);
            }

        }

        public ISimpleObjectEvents Callbacks { get; set; }

        private IMyCoolClass _obj;
        public void RaisePassClass()
        {
            _obj = new MyCoolClass();
            if (Callbacks != null)
            {
                Callbacks.PassClass(_obj);
            }
            if (PassClass != null)
            {
                PassClass(_obj);
            }
        }

        public void RaisePassString()
        {
            if (Callbacks != null)
            {
                Callbacks.PassString("Hello from managed");
            }
            if (PassString != null)
            {
                PassString("Hello from managed");
            }
        }

        public void RaiseEnsureGCIsNotObstacle()
        {
            if (Callbacks != null)
            {
                Callbacks.EnsureGCIsNotObstacle();
            }
            if (EnsureGCIsNotObstacle != null)
            {
                EnsureGCIsNotObstacle();
            }
        }

        public void RaiseEmptyEvent()
        {
            if (Callbacks != null)
            {
                Callbacks.SimpleEmptyEvent();
            }
            if (SimpleEmptyEvent != null)
            {
                SimpleEmptyEvent();
            }
        }

        public string HelloWorld()
        {
            return "HelloWorld" + fField;
        }

        public void GetProcessThreadID(out uint processId, out uint threadId)
        {
            processId = NativeMethods.GetCurrentProcessId();
            threadId = NativeMethods.GetCurrentThreadId();
        }




        [ComVisible(false)]
        public delegate void PassStuctEventHandler(MyCoolStuct val);

        public event PassStuctEventHandler PassStuct;

        [ComVisible(false)]
        public delegate void PassClassEventHandler(IMyCoolClass obj);
        public event PassClassEventHandler PassClass;

        [ComVisible(false)]
        public delegate void PassStringEventHandler(string str);
        public event PassStringEventHandler PassString;

        [ComVisible(false)]
        public delegate void FloatPropertyChangingEventHandler(float NewValue, ref bool Cancel);
        public event FloatPropertyChangingEventHandler FloatPropertyChanging;

        [ComVisible(false)]
        public delegate void EnsureGCIsNotObstacleEventHandler();
        public event EnsureGCIsNotObstacleEventHandler EnsureGCIsNotObstacle;
        [ComVisible(false)]
        public delegate void SimpleEmptyEventEventHandler();
        public event SimpleEmptyEventEventHandler SimpleEmptyEvent;
    }
}