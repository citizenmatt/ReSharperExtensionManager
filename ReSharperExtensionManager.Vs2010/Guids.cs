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

namespace CitizenMatt.ReSharper.ExtensionManager
{
    static class GuidList
    {
        public const string VsPackageString = "a32d3c3a-2b0e-4c64-bf33-4bb842e0f8f8";
        public const string VsCommandSetString = "0236d38d-aba3-4c60-8e26-ce5040728439";

        public static readonly Guid VsCommandSet = new Guid(VsCommandSetString);
    };
}