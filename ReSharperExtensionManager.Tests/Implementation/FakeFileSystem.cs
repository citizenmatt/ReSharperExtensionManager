using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;
using Xunit;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakeFileSystem : IFileSystem
    {
        public FakeFileSystem(string root)
        {
            Root = root;
            Files = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        }

        public ISet<string> Files { get; private set; }

        public void DeleteDirectory(string path, bool recursive)
        {
            // Can't be bothered implementing non-recursive delete
            Assert.True(recursive);

            if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                path = path + Path.DirectorySeparatorChar;

            var toRemove = from file in Files
                           where file.StartsWith(path, StringComparison.InvariantCultureIgnoreCase)
                           select file;
            foreach (var file in toRemove.ToList())
                Files.Remove(file);
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            if (path != string.Empty && path[path.Length-1] != Path.DirectorySeparatorChar)
                path = path + Path.DirectorySeparatorChar;

            var rootPaths = from file in Files
                            where file.StartsWith(path, StringComparison.InvariantCultureIgnoreCase)
                            select Path.GetDirectoryName(file.Substring(path.Length));

            return rootPaths.Distinct();
        }

        public void DeleteFile(string path)
        {
            var found = Files.Where(s => s.Equals(path, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (found != null)
                Files.Remove(found);
        }

        public bool FileExists(string path)
        {
            return Files.Any(s => s.Equals(path, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool DirectoryExists(string path)
        {
            return Files.Any(s => s.StartsWith(path, StringComparison.InvariantCultureIgnoreCase));
        }

        public string Root { get; private set; }

#region Not implemented

        public IEnumerable<string> GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFiles(string path, string filter)
        {
            throw new NotImplementedException();
        }

        public string GetFullPath(string path)
        {
            throw new NotImplementedException();
        }

        public void AddFile(string path, Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastModified(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetCreated(string path)
        {
            throw new NotImplementedException();
        }

        public ILogger Logger
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
#endregion
    }
}