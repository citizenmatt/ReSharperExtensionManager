#region license
// Copyright 2011 Matt Ellis (@citizenmatt)
//
// This file is part of ReSharper Extension Manager.
//
// ReSharper Extension Manager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// ReSharper Extension Manager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with ReSharper Extension Manager.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CitizenMatt.ReSharper.ExtensionManager
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.VsPackageString)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    public sealed class VsPackage : Package, IVisualStudioApi
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

            Bootstrapper.Initialise(this, version);
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

        public void Restart()
        {
            var service = (IVsShell4)GetService(typeof(IVsShell));
            service.Restart((uint)__VSRESTARTTYPE.RESTART_Normal);
        }
    }
}
