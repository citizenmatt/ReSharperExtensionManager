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