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

using System.Linq;
using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class UpdatesTreeNode : SimpleTreeNode
    {
        private readonly IPackageManager packageManager;
        private readonly IPackageItemCommandHandler commandHandler;

        public UpdatesTreeNode(string name, IVsExtensionsTreeNode parent, IPackageManager packageManager, IPackageItemCommandHandler commandHandler)
            : base(name, parent, packageManager.SourceRepository, commandHandler)
        {
            this.packageManager = packageManager;
            this.commandHandler = commandHandler;
        }

        protected override IQueryable<IPackage> GetPackages()
        {
            return packageManager.SourceRepository.GetUpdates(packageManager.LocalRepository.GetPackages()).AsQueryable();
        }

        protected override PackageItem CreatePackageItem(IPackage package)
        {
            return new PackageItem(package, commandHandler, true);
        }
    }
}