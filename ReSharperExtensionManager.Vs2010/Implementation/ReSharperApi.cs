using System;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public class ReSharperApi : IReSharperApi
    {
        public ReSharperApi(Version version)
        {
            Version = version;
        }

        public Version Version { get; private set; }

        public void Initialise(Action continuation)
        {
            throw new NotImplementedException();
        }
    }
}