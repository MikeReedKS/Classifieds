using System;
using System.IO;

namespace Classifieds.Helpers
{
    /// <summary>
    /// Routines to simplify access to storage, typically the disk.
    /// </summary>
    public static class StorageHelper
    {
        /// <summary>
        /// Gets the location on disk to store the data file(s).
        /// </summary>
        /// <returns>Folder Path where data files can be found.</returns>
        public static string GetDataFolder()
        {
            var location = AppDomain.CurrentDomain.BaseDirectory;
            var directoryInfo = new FileInfo(location).Directory;
            return directoryInfo != null ? directoryInfo.FullName : "";
        }
    }
}