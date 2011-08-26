using System;
using System.Collections.Generic;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakeReSharperApi : IReSharperApi
    {
        public FakeReSharperApi(Version version)
        {
            Version = version;
            Plugins = new List<FakePlugin>();
        }

        public Version Version { get; private set; }

        public void Initialise(Action continuation)
        {
            Initialised = true;
            continuation();
        }

        public void AddPlugin(string id, IEnumerable<string> assemblyFiles, bool enabled)
        {
            Plugins.Add(new FakePlugin(id, assemblyFiles, enabled));
        }

        public bool Initialised { get; private set; }

        public IList<FakePlugin> Plugins { get; private set; }
    }
}