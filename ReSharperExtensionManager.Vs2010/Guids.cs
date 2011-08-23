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