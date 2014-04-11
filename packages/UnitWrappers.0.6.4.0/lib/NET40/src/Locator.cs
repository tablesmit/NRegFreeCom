using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitWrappers.System.Diagnostics;

namespace UnitWrappers
{
    public class UnitWrappersLocator
    {
  
        public T GetInstance<T>() where T:class 
        {
            if (typeof(T) == typeof (IProcessSystem))
            {
                return new ProcessSystem() as T;
            }
            return null;
        }
    }
}
