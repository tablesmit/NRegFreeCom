using System;
using System.Runtime.InteropServices;

namespace NRegFreeCom
{
    public static class COMExceptionChecker
    {
        public const int RPC_E_CALL_REJECTED = unchecked((int)0x80010001);
        public const int RPC_E_SERVERCALL_RETRYLATER = unchecked((int)0x8001010A);

        public static bool IsCOMThreadingException(Exception ex)
        {
            COMException comException = ex as COMException;
            if (comException != null
                && (comException.ErrorCode == RPC_E_CALL_REJECTED || comException.ErrorCode == RPC_E_SERVERCALL_RETRYLATER))
            {
                return true;
            }
            return false;
        }
    }
}
