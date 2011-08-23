using System;
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

        public bool Initialised { get; private set; }
    }
}