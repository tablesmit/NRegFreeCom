using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RegFreeCom.Interfaces
{
    [Guid("821B0F39-C5AA-47E8-B00C-2A96758D3B2D")]
    [ComVisible(true)]
    
    public interface ILoadedByManagedImplementedByNative
    {
        void Set(int data);
        string Get();
    }


}
