using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CitizenMatt.ReSharper.ExtensionManager
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.VsPackageString)]
    public sealed class VsPackage : Package
    {
        public VsPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }

        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();

            // The guid is from typeof(ReSharperPkg).Guid
            var resharperPackageGuid = new Guid("0C6E6407-13FC-4878-869A-C8B4016C57FE");
            var version = GetReSharperVersion(resharperPackageGuid);
            if (version == null)
                return;

            Bootstrapper.Initialise(version);
        }

        private Version GetReSharperVersion(Guid resharperPackageGuid)
        {
            var package = GetReSharperVsPackage(resharperPackageGuid);
            return package != null ? package.GetType().Assembly.GetName().Version : null;
        }

        private IVsPackage GetReSharperVsPackage(Guid resharperPackageGuid)
        {
            IVsPackage package;
            var vsShell = (IVsShell)GetService(typeof(IVsShell));
            vsShell.IsPackageLoaded(resharperPackageGuid, out package);
            return package;
        }
    }
}
