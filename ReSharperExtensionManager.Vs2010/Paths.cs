using System;
using System.IO;

namespace CitizenMatt.ReSharper.ExtensionManager
{
    public static class Paths
    {
        public static string LocalRepositoryRoot
        {
            get
            {
                return Path.Combine(ExtensionManagerRoot, "Repository");
            }
        }

        public static string ExtensionManagerRoot
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CitizenMatt.ReSharper.ExtensionManager");
            }
        }
    }
}
