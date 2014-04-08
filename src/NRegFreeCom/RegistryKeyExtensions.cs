using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace NRegFreeCom
{
    public  static class RegistryKeyExtensions
    {
        /// <summary>
        /// Deletes the specified subkey and any child subkeys recursively, and specifies whether an exception is raised if the subkey is not found.
        /// </summary>
        /// <param name="subkey">The name of the subkey to delete. This string is not case-sensitive.</param><param name="throwOnMissingSubKey">Indicates whether an exception should be raised if the specified subkey cannot be found. If this argument is true and the specified subkey does not exist, an exception is raised. If this argument is false and the specified subkey does not exist, no action is taken.</param><exception cref="T:System.ArgumentException">An attempt was made to delete the root hive of the tree.-or-<paramref name="subkey"/> does not specify a valid registry subkey, and <paramref name="throwOnMissingSubKey"/> is true.</exception><exception cref="T:System.ArgumentNullException"><paramref name="subkey"/> is null.</exception><exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey"/> is closed (closed keys cannot be accessed).</exception><exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception><exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key.</exception>
        public static void DeleteSubKeyTree(this RegistryKey current,string subkey, bool throwOnMissingSubKey)
        {
            try
            {
                current.DeleteSubKeyTree(subkey);
            }
            catch (System.ArgumentException)
            {
                if (throwOnMissingSubKey)
                    throw;
            }   
        }
    }
}
