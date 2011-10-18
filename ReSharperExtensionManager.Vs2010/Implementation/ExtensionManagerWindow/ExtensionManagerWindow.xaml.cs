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
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.ExtensionsExplorer.UI;
using NuGet;
using MessageBox = JetBrains.Util.MessageBox;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public partial class ExtensionManagerWindow : INotifyPropertyChanged
    {
        private bool restartRequired;

        public ExtensionManagerWindow(IPackageManager packageManager)
        {
            InitializeComponent();

            explorer.Providers.Add(new InstalledProvider(packageManager, Resources));
            explorer.Providers.Add(new GalleryProvider(packageManager, Resources));
            explorer.Providers.Add(new UpdatesProvider(packageManager, Resources));

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

        public bool RestartRequired
        {
            get { return restartRequired; }
            set
            {
                if (value == restartRequired) return;

                restartRequired = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RestartRequired"));
            }
        }

        public bool ShouldRestart { get; private set; }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void OnSetFocusOnSearchBox(object sender, ExecutedRoutedEventArgs e)
        {
            explorer.SetFocusOnSearchBox();
        }

        private void OnRestartVisualStudio(object sender, ExecutedRoutedEventArgs e)
        {
            ShouldRestart = true;
            Close();
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

                // Install, uninstall and update all require a restart
                RestartRequired = true;
            }
            catch (Exception exception)
            {
                MessageBox.ShowError(exception.ToString());
            }
        }

        private void OnCategorySelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var simpleTreeNode = explorer.SelectedExtensionTreeNode as SimpleTreeNode;
            var selectedTreeNodeProvider = explorer.SelectedProvider as ISelectedTreeNodeProvider;
            if (simpleTreeNode != null && selectedTreeNodeProvider != null)
            {
                selectedTreeNodeProvider.SelectedNode = simpleTreeNode;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
