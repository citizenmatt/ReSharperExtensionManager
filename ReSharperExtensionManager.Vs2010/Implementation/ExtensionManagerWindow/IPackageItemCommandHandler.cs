namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public interface IPackageItemCommandHandler
    {
        string Label { get; }
        bool CanExecute(PackageItem item);
        void Execute(PackageItem item);
    }
}