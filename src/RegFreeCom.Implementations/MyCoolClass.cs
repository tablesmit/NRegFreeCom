using System.Runtime.InteropServices;
using NRegFreeCom;
using RegFreeCom.Interfaces;

namespace RegFreeCom.Implementations
{
    [ComVisible(true)]
    [Guid("76042FEC-275F-4152-8661-9BEDFB5CCF65")]
    [ComDefaultInterface(typeof(IMyCoolClass))]
    public class MyCoolClass : IMyCoolClass
    {
        public string MyValue { get { return "The Property"; } }

        public int GetMyValue2(out string vval)
        {
            vval = "The Property";
            return SYSTEM_ERROR_CODES.ERROR_SUCCESS;
        }

        
    }
}