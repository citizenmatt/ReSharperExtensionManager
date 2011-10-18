#region license
// Copyright 2011 Matt Ellis (@citizenmatt)
//
// This file is part of ReSharper Extension Manager.
//
// ReSharper Extension Manager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// ReSharper Extension Manager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with ReSharper Extension Manager.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Components;
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
        private const string ManagerActionId = "CitizenMatt.ExtensionManger.ShowExtensionManager";

        public ReSharperApi60(Version version)
        {
            Version = version;
        }

        public Version Version { get; private set; }

        public void Initialise(Action onInitialised, Action onDispose)
        {
            var initialisationTimer = new ReentrancyGuardTimer("CitizenMatt.ReSharper.ExtensionManager.Initialisation");
            initialisationTimer.Interval.Value = TimeSpan.FromMilliseconds(100);
            initialisationTimer.Tick.Advise(EternalLifetime.Instance, () =>
                                                                          {
                                                                              if (Shell.HasInstance)
                                                                              {
                                                                                  InitialiseCleanupHook(onDispose);
                                                                                  initialisationTimer.Dispose();
                                                                                  onInitialised();
                                                                              }
                                                                          });
            initialisationTimer.IsEnabled.Value = true;
        }

        private void InitialiseCleanupHook(Action onDispose)
        {
            var components = Shell.Instance.Components.ComponentContainer as ComponentContainer;
            if (components != null)
            {
                components.Inject<CleanupHook>();

                var cleanup = GetComponent<CleanupHook>();
                cleanup.AddCleanupHook(onDispose);
            }
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
            // TODO: How do we remove the menu item when we're no longer installed?
            // Adding like this leaves it in the ReSharper menu. I can remove it by creating a new action
            // with the same ID and adding and removing it
            var executableAction = ActionManager.CreateAction(ManagerActionId, new ActionPresentation(label));
            executableAction.AddHandler(EternalLifetime.Instance, new SimpleActionHandler(action));

            var group = ActionManager.GetActionGroup("ReSharper");
            var optionsIndex = group.GetActionIndex("ShowOptions");
            group.InsertAction(optionsIndex, executableAction);
        }

        public void RemoveManagerMenuItem()
        {
            ActionManager.RemoveAction(ManagerActionId);
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
            var instance = Shell.Instance;
            return instance.GetComponent<T>();
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

        [UsedImplicitly]
        public class CleanupHook
        {
            private readonly Lifetime lifetime;

            public CleanupHook(Lifetime lifetime)
            {
                this.lifetime = lifetime;
            }

            public void AddCleanupHook(Action action)
            {
                lifetime.AddAction(action);
            }
        }
    }
}
