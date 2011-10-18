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
using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakePackageRepository : IPackageRepository
    {
        private readonly List<IPackage> packages = new List<IPackage>();

        public FakePackageRepository(string source)
        {
            Source = source;
        }

        public IQueryable<IPackage> GetPackages()
        {
            return packages.AsQueryable();
        }

        public void AddPackage(IPackage package)
        {
            packages.Add(package);
        }

        public void RemovePackage(IPackage package)
        {
            packages.Remove(package);
        }

        public string Source { get; private set; }
    }
}