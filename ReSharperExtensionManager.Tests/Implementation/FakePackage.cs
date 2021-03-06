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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakePackage : IPackage
    {
        public FakePackage(string id, string version, params string[] files)
        {
            Id = id;
            Version = Version.Parse(version);
            Authors = new List<string>();
            Owners = new List<string>();
            FrameworkAssemblies = new List<FrameworkAssemblyReference>();
            Dependencies = new List<PackageDependency>();
            AssemblyReferences = new List<IPackageAssemblyReference>();
            Files = (from file in files
                     select new FakePackageFile(file)).ToList();
        }

        public IEnumerable<IPackageFile> Files { get; private set; }

        public string Id { get; private set; }
        public Version Version { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public Uri IconUrl { get; set; }
        public Uri LicenseUrl { get; set; }
        public Uri ProjectUrl { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ReleaseNotes { get; set; }
        public string Language { get; set; }
        public string Tags { get; set; }
        public string Copyright { get; set; }
        public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies { get; set; }
        public IEnumerable<PackageDependency> Dependencies { get; set; }
        public Uri ReportAbuseUrl { get; set; }
        public int DownloadCount { get; set; }
        public int RatingsCount { get; set; }
        public double Rating { get; set; }

        public IEnumerable<IPackageFile> GetFiles()
        {
            return Files;
        }

        public Stream GetStream()
        {
            throw new NotSupportedException();
        }

        public bool IsLatestVersion { get; set; }
        public DateTimeOffset? Published { get; set; }
        public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; set; }
    }
}