using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public class ExtensionLoader : IExtensionLoader
    {
        private readonly IReSharperApi resharperApi;
        private readonly IPackageRepository localPackageRepository;

        public ExtensionLoader(IReSharperApi resharperApi, IPackageRepository localPackageRepository)
        {
            this.resharperApi = resharperApi;
            this.localPackageRepository = localPackageRepository;
        }

        public void LoadPlugins()
        {
            var packages = localPackageRepository.GetPackages();
            foreach (var package in packages)
            {
                var assemblyFiles = (from packageFile in package.GetFiles(PluginFolder)
                                     select packageFile.Path).ToList();
                if (assemblyFiles.Count > 0)
                    resharperApi.AddPlugin(package.Id, assemblyFiles, true);
            }
        }

        private string PluginFolder
        {
            get { return string.Format(@"rs{0}\plugins", resharperApi.Version.ToString(2)); }
        }
    }
}