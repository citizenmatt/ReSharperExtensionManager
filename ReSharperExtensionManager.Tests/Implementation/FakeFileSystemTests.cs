using System.Linq;
using Xunit;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakeFileSystemTests
    {
        [Fact]
        public void ShouldDeleteOnlyFilesInDirectory()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            fileSystem.DeleteDirectory("Matt", true);

            Assert.Equal(1, fileSystem.Files.Count);
            Assert.Equal("Matt2\\file3.txt", fileSystem.Files.First());
        }

        [Fact]
        public void ShouldGetDirectoriesAtRootLevel()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            var directories = fileSystem.GetDirectories(string.Empty).ToList();

            Assert.Equal(2, directories.Count);
            Assert.Equal(new[] { "Matt", "Matt2" }, directories.ToArray());
        }

        [Fact]
        public void ShouldDeleteFile()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            fileSystem.DeleteFile("Matt\\file2.txt");

            Assert.Equal(3, fileSystem.Files.Count);
            Assert.Contains("Matt\\file1.txt", fileSystem.Files);
            Assert.Contains("Matt\\file3.txt", fileSystem.Files);
            Assert.Contains("Matt2\\file3.txt", fileSystem.Files);
            Assert.DoesNotContain("Matt\\file2.txt", fileSystem.Files);
        }

        [Fact]
        public void ShouldDeleteFileCaseInsensitive()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            fileSystem.DeleteFile("MATT\\FILE2.txt");

            Assert.Equal(3, fileSystem.Files.Count);
            Assert.Contains("Matt\\file1.txt", fileSystem.Files);
            Assert.Contains("Matt\\file3.txt", fileSystem.Files);
            Assert.Contains("Matt2\\file3.txt", fileSystem.Files);
            Assert.DoesNotContain("Matt\\file2.txt", fileSystem.Files);
        }

        [Fact]
        public void FileExistsShouldReturnTrueIfFileExistsCaseInsensitive()
        {
            var fileSystem = new FakeFileSystem(@"C:\\temp\\repo\");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            Assert.True(fileSystem.FileExists("MATT\\FILE1.txt"));
        }

        [Fact]
        public void FileExistsShouldReturnFalseIfFileDoesNotExist()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            Assert.False(fileSystem.FileExists("Matt2\\file1.txt"));
        }

        [Fact]
        public void DirectoryExistsShouldReturnTrueIfExistsCaseInsensitive()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            Assert.True(fileSystem.DirectoryExists("MATT"));
        }

        [Fact]
        public void DirectoryExistsShouldReturnFalseIfDoesNotExist()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add("Matt\\file1.txt");
            fileSystem.Files.Add("Matt\\file2.txt");
            fileSystem.Files.Add("Matt\\file3.txt");
            fileSystem.Files.Add("Matt2\\file3.txt");

            Assert.False(fileSystem.DirectoryExists("Blah"));
        }

        [Fact]
        public void DirectoryExistsShouldReturnTrueWithDeepPath()
        {
            var fileSystem = new FakeFileSystem(@"C:\temp\repo");
            fileSystem.Files.Add(@"dir1\dir2\dir3\dir4\file1.txt");

            Assert.True(fileSystem.DirectoryExists(@"dir1\dir2\dir3"));
        }
    }
}