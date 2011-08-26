using System.Collections.Generic;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests
{
    public class FakePlugin
    {
        public readonly string Id;
        public readonly IEnumerable<string> AssemblyFiles;
        public readonly bool Enabled;

        public FakePlugin(string id, IEnumerable<string> assemblyFiles, bool enabled)
        {
            Id = id;
            AssemblyFiles = assemblyFiles;
            Enabled = enabled;
        }
    }
}