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
using CitizenMatt.ReSharper.ExtensionManager.Implementation;

namespace CitizenMatt.ReSharper.ExtensionManager
{
    public static class Bootstrapper
    {
        public static void Initialise(IVisualStudioApi visualStudioApi, Version resharperVersion)
        {
            Initialise(visualStudioApi, resharperVersion, version => new ReSharperApi60(version), ExtensionManagerFactory.Create);
        }

        public static void Initialise(IVisualStudioApi visualStudioApi, Version resharperVersion, Func<Version, IReSharperApi> resharperApiFactory,
            Func<IReSharperApi, IVisualStudioApi, IExtensionManager> extensionManagerFactory)
        {
            IExtensionManager extensionManager = null;

            var resharperApi = resharperApiFactory(resharperVersion);
            resharperApi.Initialise(() =>
                                        {
                                            extensionManager = extensionManagerFactory(resharperApi, visualStudioApi);
                                            extensionManager.InitialiseEnvironment();
                                        },
                                    () =>
                                        {
                                            if (extensionManager != null)
                                                extensionManager.Dispose();
                                        }
                );
        }
    }
}