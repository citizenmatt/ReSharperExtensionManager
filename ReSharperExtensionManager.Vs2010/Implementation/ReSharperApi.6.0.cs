using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.PluginSupport;
using JetBrains.DataFlow;
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

        // ReSharper 6.0 uses the PluginsDirectory.Plugins observable collection to keep
        // track of plugins. The PluginLoader class sets up listeners on this collection
        // to handle loading the plugins into the product (I'm not sure who loads the
        // assemblies). The docs for PluginsDirectory state: "Plugins to be loaded are
        // published here, by standard collectors like CollectPluginsInProductFolders
        // and CollectPluginsOnCommandLine, or by you".
        //
        // I don't know where PluginManager lives in all of this. It looks like it still
        // gets created and works as it used to, but only used by Internal stuff (there's
        // an internal PluginDownloader namespace!) - the plugins page in the options
        // uses the new PluginsDirectory class

        public void AddPlugin(string id, IEnumerable<string> assemblyFiles, bool enabled)
        {
            // TODO: There should probably be some kind of exception handling here
            var assemblies = (from path in assemblyFiles
                              select new FileSystemPath(path)).ToList();

            var records = new List<PluginsDirectory.Record>();
            var pluginPresentation = PluginPresentation.ReadFromAssemblies(assemblies, null, records);

            var plugin = new Plugin(EternalLifetime.Instance, assemblies, null, pluginPresentation, null);
            plugin.IsEnabled.Value = enabled;
            plugin.RuntimeInfoRecords.AddRange(records);
            PluginsDirectory.Plugins.Add(plugin);
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
