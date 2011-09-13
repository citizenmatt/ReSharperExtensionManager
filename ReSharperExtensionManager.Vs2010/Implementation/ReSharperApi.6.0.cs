using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
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

        // TODO: Don't really like this - it knows too much
        // But how would I abstract out the positioning of the menu in a resharper version agnostic manner?
        public void AddManagerMenuItem(string label, Action action)
        {
            var executableAction = ActionManager.CreateAction("ShowExtensionManager", new ActionPresentation(label));
            executableAction.AddHandler(EternalLifetime.Instance, new SimpleActionHandler(action));

            var group = ActionManager.GetActionGroup("ReSharper");
            var optionsIndex = group.GetActionIndex("ShowOptions");
            group.InsertAction(optionsIndex, executableAction);
        }

        private static PluginsDirectory PluginsDirectory
        {
            get { return GetComponent<PluginsDirectory>(); }
        }

        private static ActionManager ActionManager
        {
            get { return GetComponent<ActionManager>(); }
        }

        private static T GetComponent<T>()
            where T : class
        {
            return Shell.Instance.GetComponent<T>();
        }

        private class SimpleActionHandler : IActionHandler
        {
            private readonly Action action;

            public SimpleActionHandler(Action action)
            {
                this.action = action;
            }

            public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
            {
                return true;
            }

            public void Execute(IDataContext context, DelegateExecute nextExecute)
            {
                action();
            }
        }
    }
}
