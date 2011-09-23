using System.ComponentModel;
using System.Windows;
using Microsoft.VisualStudio.ExtensionsExplorer;
using Microsoft.VisualStudio.ExtensionsExplorer.UI;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class UpdatesProvider : VsExtensionsProvider, IPackageItemCommandHandler
    {
        private readonly IPackageManager packageManager;
        private readonly ResourceDictionary resources;
        private IVsExtensionsTreeNode extensionsTree;
        private object mediumIconDataTemplate;
        private object detailViewDataTemplate;

        public UpdatesProvider(IPackageManager packageManager, ResourceDictionary resources)
        {
            this.packageManager = packageManager;
            this.resources = resources;
        }

        public override string Name
        {
            get { return "Updates"; }
        }

        public override float SortOrder
        {
            get { return 300; }
        }

        public override IVsExtensionsTreeNode ExtensionsTree
        {
            get
            {
                if (extensionsTree == null)
                {
                    extensionsTree = new RootTreeNode();
                    extensionsTree.Nodes.Add(new UpdatesTreeNode("Available Updates", extensionsTree, packageManager, this) { IsSelected = true, IsExpanded = true });
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
            get { return Resources.Resources.Dialog_UpdateButton; }
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
                MessageBox.Show("Extension updated");
            }
            else
                MessageBox.Show(string.Format("Error updating extension: {0}", e.Error));
        }

        private bool ExecuteCore(PackageItem item)
        {
            // TODO: Show progress window
            // TODO: We're running async, stop other operations from happening
            if (item.RequireLicenseAcceptance)
                MessageBox.Show("Blah blah accept license blah blah");
            packageManager.UpdatePackage(item.PackageIdentity, true);
            return true;
        }
    }
}