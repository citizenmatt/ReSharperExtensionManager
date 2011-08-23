using System;
using System.Collections.Generic;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public interface IReSharperApi
    {
        Version Version { get; }
        void Initialise(Action onInitialised);
        void AddPlugin(string id, IEnumerable<string> assemblyFiles, bool enabled);
    }
}