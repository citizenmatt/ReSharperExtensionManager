using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.PluginSupport;
using JetBrains.DataFlow;
using JetBrains.Metadata.Reader.API;
using JetBrains.Threading;
using JetBrains.Util;

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

        public void AddPlugin(string id, IEnumerable<string> assemblyFiles, bool enabled)
        {
            using (var loader = new MetadataLoader())
            {
                var pluginFiles = from file in assemblyFiles
                                  select new JetTuple<string, FileSystemPath>(file, new FileSystemPath(file));

                var plugins = DiscoverPluginsInDirectory.CreatePluginsFromFileSet(EternalLifetime.Instance, pluginFiles,
                                                                                  GetComponent<IApplicationDescriptor>(),
                                                                                  PluginsDirectory.InfoRecords, loader);

                PluginsDirectory.Plugins.AddRange(plugins);
            }
        }

        private static PluginsDirectory PluginsDirectory
        {
            get { return GetComponent<PluginsDirectory>(); }
        }

        private static T GetComponent<T>()
            where T : class
        {
            return Shell.Instance.GetComponent<T>();
        }
    }
}
