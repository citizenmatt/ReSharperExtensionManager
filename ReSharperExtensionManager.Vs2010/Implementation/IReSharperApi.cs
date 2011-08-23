using System;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public interface IReSharperApi
    {
        Version Version { get; }
        void Initialise(Action onInitialised);
    }
}