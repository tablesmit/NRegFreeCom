
using System;

namespace UnitWrappers
{

	public static class IEnvironmentEx
	{
	

        /// <summary>
        /// If this is true, then some registry calls are redirected.
        /// </summary>
        /// <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/aa384232.aspx"/>
        public static bool IsWoW64RedirectionOn(this UnitWrappers.System.IEnvironment current)        	
        {
        	return current.Is64BitOperatingSystem && !current.Is64BitProcess;
		}

	}
}
