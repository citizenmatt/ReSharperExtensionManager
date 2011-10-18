using System;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public interface IExtensionManager : IDisposable
    {
        void InitialiseEnvironment();
    }
}