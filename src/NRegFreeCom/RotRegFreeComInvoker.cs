using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace NRegFreeCom
{
    /// <summary>
    /// Allows to invoke Out of Proc COM object if Windows registy does  contains zero info about COM class and interfaces.
    /// Adds some layer of indirection with some overhead comparable with ovehead 
    /// add by routing usual .NET call from C# to usual native COM via interop assembly.
    /// </summary>
    public class RotRegFreeComInvoker : RealProxy
    {

        public static T GetRealObjectByProxy<T>(object instance)
        {
            Guid typeIdd = typeof(T).GUID;
            IntPtr unknownPointer = Marshal.GetIUnknownForObject(instance);
            IntPtr realPointer = IntPtr.Zero;
            int result = Marshal.QueryInterface(unknownPointer, ref typeIdd, out realPointer);
            if (result != SYSTEM_ERROR_CODES.ERROR_SUCCESS)
                throw new Win32Exception(result);
            return (T)Marshal.GetObjectForIUnknown(realPointer);
        }

        public static T ProxyInterface<T>(object com)
        {
            return (T)(new RotRegFreeComInvoker(com, typeof(T)).GetTransparentProxy());
        }

        private readonly object _com;
        private Type _type;

        internal RotRegFreeComInvoker(object com, Type type)
            : base(type)
        {
            _com = com;
            _type = _com.GetType();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var input = (IMethodCallMessage)msg;
            try
            {
    
                if (input.MethodName.StartsWith("get_"))
                {
     
                    var result = _type.InvokeMember(input.MethodName.Remove(0,4),
        BindingFlags.GetProperty, null,
         _com, null);
                    return new ReturnMessage(result, null, 0, input.LogicalCallContext, input);
                }
                if (input.MethodName.StartsWith("set_"))
                {
                    var result = _type.InvokeMember(input.MethodName.Remove(0,4),
       BindingFlags.SetProperty, null,
         _com, input.InArgs);
                    return new ReturnMessage(result, null, 0, input.LogicalCallContext, input);
                }
                else
                {
                    var result = _type.InvokeMember(input.MethodName,
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null,
                             _com, input.InArgs);
                    return new ReturnMessage(result, null, 0, input.LogicalCallContext, input);
                }


            }
            catch (Exception ex)
            {
                return new ReturnMessage(ex, input);
            }

        }
    }
}