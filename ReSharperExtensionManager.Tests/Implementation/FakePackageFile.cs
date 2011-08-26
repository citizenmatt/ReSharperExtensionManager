using System;
using System.IO;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakePackageFile : IPackageFile
    {
        public FakePackageFile(string path)
        {
            Path = path;
        }

        public Stream GetStream()
        {
            throw new NotSupportedException();
        }

        public string Path { get; private set; }
    }
}