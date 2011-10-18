using System;
using System.Collections.Generic;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public interface IReSharperApi
    {
        Version Version { get; }
        void Initialise(Action onInitialised, Action onDispose);
        void AddPlugin(string id, IEnumerable<string> assemblyFiles, bool enabled);
        void AddManagerMenuItem(string label, Action action);
        void RemoveManagerMenuItem();
    }
}