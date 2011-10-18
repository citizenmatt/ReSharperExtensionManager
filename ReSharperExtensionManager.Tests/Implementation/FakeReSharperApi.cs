using System;
using System.Collections.Generic;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakeReSharperApi : IReSharperApi
    {
        private Action onDisposeAction;

        public FakeReSharperApi(Version version)
        {
            Version = version;
            Plugins = new List<FakePlugin>();
        }

        public Version Version { get; private set; }

        public void Initialise(Action continuation, Action onDispose)
        {
            onDisposeAction = onDispose;

            Initialised = true;
            continuation();
        }

        public void AddPlugin(string id, IEnumerable<string> assemblyFiles, bool enabled)
        {
            Plugins.Add(new FakePlugin(id, assemblyFiles, enabled));
        }

        public void AddManagerMenuItem(string label, Action action)
        {
            ManagerMenuItemLabel = label;
            ManagerMenuItemAction = action;
        }

        public void RemoveManagerMenuItem()
        {
            ManagerMenuItemLabel = null;
            ManagerMenuItemAction = null;
        }

        public void FakeReSharperTermination()
        {
            if (onDisposeAction != null)
                onDisposeAction();
        }

        public bool Initialised { get; private set; }
        public IList<FakePlugin> Plugins { get; private set; }
        public string ManagerMenuItemLabel { get; private set; }
        public Action ManagerMenuItemAction { get; private set; }
    }
}