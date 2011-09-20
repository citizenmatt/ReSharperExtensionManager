using System.ComponentModel;
using System.Windows;
using Microsoft.VisualStudio.ExtensionsExplorer;
using Microsoft.VisualStudio.ExtensionsExplorer.UI;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class InstalledProvider : VsExtensionsProvider, IPackageItemCommandHandler, ISelectedTreeNodeProvider
    {
        private readonly IPackageManager packageManager;
        private readonly ResourceDictionary resources;
        private IVsExtensionsTreeNode extensionsTree;
        private object mediumIconDataTemplate;
        private object detailViewDataTemplate;

        public InstalledProvider(IPackageManager packageManager, ResourceDictionary resources)
        {
            this.packageManager = packageManager;
            this.resources = resources;
        }

        public override string Name
        {
            get { return "Installed Extensions"; }
        }

        public override float SortOrder
        {
            get { return 100; }
        }

        public override IVsExtensionsTreeNode ExtensionsTree
        {
            get
            {
                if (extensionsTree == null)
                {
                    extensionsTree = new RootTreeNode();
                    extensionsTree.Nodes.Add(new SimpleTreeNode("All", extensionsTree, packageManager.LocalRepository, this)
                                                 {IsSelected = true, IsExpanded = true});
                }
                return extensionsTree;
            }
        }

        public override object MediumIconDataTemplate
        {
            get { return mediumIconDataTemplate ?? (mediumIconDataTemplate = resources["PackageItemTemplate"]); }
        }

        public override object DetailViewDataTemplate
        {
            get { return detailViewDataTemplate ?? (detailViewDataTemplate = resources["PackageDetailTemplate"]); }
        }

        public string Label
        {
            get { return Resources.Resources.Dialog_UninstallButton; }
        }

        public bool CanExecute(PackageItem item)
        {
            return true;
        }

        public void Execute(PackageItem item)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += OnRunWorkerDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync(item);
        }

        private void OnRunWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var item = (PackageItem)e.Argument;
            var succeeded = ExecuteCore(item);
            e.Cancel = !succeeded;
            e.Result = item;
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // TODO: Hide progress window
                // TODO: Allow operations again
                if (SelectedNode != null)
                    SelectedNode.Extensions.Remove((PackageItem) e.Result);
                MessageBox.Show("Extension uninstalled");
            }
            else
                MessageBox.Show(string.Format("Error uninstalling extension: {0}", e.Error));
        }

        private bool ExecuteCore(PackageItem item)
        {
            // TODO: Show progress window
            // TODO: We're running async, stop other operations from happening
            packageManager.UninstallPackage(item.PackageIdentity, true, false);
            return true;
        }

        public SimpleTreeNode SelectedNode { get; set; }
    }

    public interface ISelectedTreeNodeProvider
    {
        SimpleTreeNode SelectedNode { get; set; }
    }
}