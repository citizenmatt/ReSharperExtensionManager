using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.ExtensionsExplorer.UI;
using NuGet;
using MessageBox = JetBrains.Util.MessageBox;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public partial class ExtensionManagerWindow
    {
        private readonly IPackageManager packageManager;

        public ExtensionManagerWindow(IPackageManager packageManager)
        {
            this.packageManager = packageManager;

            InitializeComponent();

            explorer.Providers.Add(new InstalledProvider(packageManager, Resources));
            explorer.Providers.Add(new GalleryProvider(packageManager, Resources));
            explorer.Providers.Add(new UpdatesProvider(packageManager));

            explorer.SelectedProvider = explorer.Providers[0];

            //explorer.BringExtensionIntoView();
            //explorer.CategorySelectionChanged+=
            explorer.IsDetailPaneVisible = true;
            explorer.IsLeftNavigationVisible = true;
            explorer.IsPageNavigatorVisible = true;
            explorer.IsProvidersListVisible = true;
            explorer.IsSearchVisible = true;
            //explorer.NoItemSelectedMessage = "Hello";
            //explorer.NoItemsMessage = "Hi";
            //explorer.PageNavigator
            //explorer.ProviderSelectionChanged
            //explorer.Providers
            //explorer.SelectedExtension
            //explorer.SelectedExtensionTreeNode
            //explorer.SelectedProvider
            //explorer.SelectedSortField
            //explorer.SetFocusOnSearchBox();
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void OnSetFocusOnSearchBox(object sender, ExecutedRoutedEventArgs e)
        {
            explorer.SetFocusOnSearchBox();
        }

        private void CanExecuteCommandOnPackage(object sender, CanExecuteRoutedEventArgs e)
        {
            var control = e.Source as VSExtensionsExplorerCtl;
            if (control == null)
            {
                e.CanExecute = false;
                return;
            }

            var selectedItem = control.SelectedExtension as PackageItem;
            if (selectedItem == null)
            {
                e.CanExecute = false;
                return;
            }

            var provider = control.SelectedProvider as IPackageItemCommandHandler;
            if (provider == null) return;

            try
            {
                e.CanExecute = selectedItem.IsEnabled;
            }
            catch (Exception)
            {
                e.CanExecute = false;
            }
        }

        private void ExecutedPackageCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var control = e.Source as VSExtensionsExplorerCtl;
            if (control == null)
            {
                return;
            }

            var selectedItem = control.SelectedExtension as PackageItem;
            if (selectedItem == null)
            {
                return;
            }

            var provider = control.SelectedProvider as IPackageItemCommandHandler;
            if (provider == null) return;

            try
            {
                provider.Execute(selectedItem);
            }
            catch (Exception exception)
            {
                MessageBox.ShowError(exception.ToString());
            }
        }

        private void OnCategorySelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var simpleTreeNode = explorer.SelectedExtensionTreeNode as SimpleTreeNode;
            if (simpleTreeNode != null)
            {
                var selectedTreeNodeProvider = explorer.SelectedProvider as ISelectedTreeNodeProvider;
                selectedTreeNodeProvider.SelectedNode = simpleTreeNode;
            }
        }
    }
}
