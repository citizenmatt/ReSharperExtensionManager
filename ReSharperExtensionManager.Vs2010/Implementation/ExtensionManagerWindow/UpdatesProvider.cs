using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class UpdatesProvider : IVsExtensionsProvider
    {
        private readonly IPackageManager extensionManager;

        public UpdatesProvider(IPackageManager extensionManager)
        {
            this.extensionManager = extensionManager;
        }

        public IVsExtensionsTreeNode Search(string searchTerms)
        {
            return null;
        }

        public string Name
        {
            get { return "Updates"; }
        }

        public float SortOrder
        {
            get { return 300; }
        }

        public object SmallIconDataTemplate
        {
            get { return null; }
        }

        public object MediumIconDataTemplate
        {
            get { return null; }
        }

        public object LargeIconDataTemplate
        {
            get { return null; }
        }

        public object DetailViewDataTemplate
        {
            get { return null; }
        }

        public IVsExtensionsTreeNode ExtensionsTree
        {
            get { return null; }
        }
    }
}