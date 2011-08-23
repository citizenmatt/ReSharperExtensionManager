using System;
using System.Collections.Generic;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests
{
    public class FakeReSharperApi : IReSharperApi
    {
        public FakeReSharperApi(Version version)
        {
            Version = version;
        }

        public Version Version { get; private set; }

        public void Initialise(Action continuation)
        {
            Initialised = true;
            continuation();
        }

        public void AddPlugin(string id, IEnumerable<string> assemblyFiles, bool enabled)
        {
        }

        public bool Initialised { get; private set; }
    }
}