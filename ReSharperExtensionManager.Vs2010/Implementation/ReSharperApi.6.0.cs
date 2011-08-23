using System;
using JetBrains.Application;
using JetBrains.DataFlow;
using JetBrains.Threading;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public class ReSharperApi60 : IReSharperApi
    {
        public ReSharperApi60(Version version)
        {
            Version = version;
        }

        public Version Version { get; private set; }

        public void Initialise(Action onInitialised)
        {
            var initialisationTimer = new ReentrancyGuardTimer("CitizenMatt.ReSharper.ExtensionManager.Initialisation");
            initialisationTimer.Interval.Value = TimeSpan.FromMilliseconds(100);
            initialisationTimer.Tick.Advise(EternalLifetime.Instance, () =>
                                                                          {
                                                                              if (Shell.HasInstance)
                                                                              {
                                                                                  initialisationTimer.Dispose();
                                                                                  onInitialised();
                                                                              }
                                                                          });
            initialisationTimer.IsEnabled.Value = true;
        }
    }
}