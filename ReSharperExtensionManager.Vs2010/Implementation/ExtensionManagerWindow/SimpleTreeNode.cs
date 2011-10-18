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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class SimpleTreeNode : IVsExtensionsTreeNode
    {
        private readonly IPackageRepository packageManager;
        private readonly IPackageItemCommandHandler commandHandler;
        private IList<IVsExtension> extensions;

        public SimpleTreeNode(string name, IVsExtensionsTreeNode parent, IPackageRepository packageManager, IPackageItemCommandHandler commandHandler)
        {
            Name = name;
            Parent = parent;
            this.packageManager = packageManager;
            this.commandHandler = commandHandler;

            Nodes = new ObservableCollection<IVsExtensionsTreeNode>();
        }

        public string Name { get; private set; }
        public IList<IVsExtensionsTreeNode> Nodes { get; private set; }
        public IVsExtensionsTreeNode Parent { get; private set; }

        public IList<IVsExtension> Extensions
        {
            get
            {
                if (extensions == null)
                {
                    extensions = new ObservableCollection<IVsExtension>();
                    var packages = from package in GetPackages()
                                   select CreatePackageItem(package);
                    extensions.AddRange(packages);
                }
                return extensions;
            }
        }

        protected virtual PackageItem CreatePackageItem(IPackage package)
        {
            return new PackageItem(package, commandHandler, false);
        }

        protected virtual IQueryable<IPackage> GetPackages()
        {
            return packageManager.GetPackages();
        }

        public bool IsSearchResultsNode
        {
            get { return false; }
        }

        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
    }
}