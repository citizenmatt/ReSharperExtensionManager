using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class PackageItem : IVsExtension
    {
        private readonly IPackageItemCommandHandler commandHandler;

        public PackageItem(IPackage package, IPackageItemCommandHandler commandHandler, bool isUpdateItem)
        {
            this.commandHandler = commandHandler;
            PackageIdentity = package;
            IsUpdateItem = isUpdateItem;
            CommandName = commandHandler.Label;
        }

        public IPackage PackageIdentity { get; private set; }
        public bool IsUpdateItem { get; set; }
        public string CommandName { get; private set; }

        public string Name
        {
            get { return string.IsNullOrEmpty(PackageIdentity.Title) ? PackageIdentity.Id : PackageIdentity.Title; }
        }

        public string Id
        {
            get { return PackageIdentity.Id; }
        }

        public bool ShouldDisplayReleaseNotes
        {
            get { return IsUpdateItem && !string.IsNullOrEmpty(ReleaseNotes); }
        }

        public string Description
        {
            get { return PackageIdentity.Description; }
        }

        public string ReleaseNotes
        {
            get { return PackageIdentity.ReleaseNotes; }
        }

        public string Version
        {
            get { return PackageIdentity.Version.ToString(); }
        }

        public string Summary
        {
            get { return String.IsNullOrEmpty(PackageIdentity.Summary) ? PackageIdentity.Description : PackageIdentity.Summary; }
        }

        public IEnumerable<string> Authors
        {
            get { return PackageIdentity.Authors; }
        }

        public Uri LicenseUrl
        {
            get { return PackageIdentity.LicenseUrl; }
        }

        public bool RequireLicenseAcceptance
        {
            get { return PackageIdentity.RequireLicenseAcceptance; }
        }

        public IEnumerable<PackageDependency> Dependencies
        {
            get { return PackageIdentity.Dependencies; }
        }

        public float Priority
        {
            get { return 0; }
        }

        public BitmapSource MediumThumbnailImage
        {
            get { return null; }
        }

        public BitmapSource SmallThumbnailImage
        {
            get { return null; }
        }

        public BitmapSource PreviewImage
        {
            get { return null; }
        }

        public bool IsSelected { get; set; }

        public bool IsEnabled
        {
            get { return commandHandler.CanExecute(this); }
        }
    }
}