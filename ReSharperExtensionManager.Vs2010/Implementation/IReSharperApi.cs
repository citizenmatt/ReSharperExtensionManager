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