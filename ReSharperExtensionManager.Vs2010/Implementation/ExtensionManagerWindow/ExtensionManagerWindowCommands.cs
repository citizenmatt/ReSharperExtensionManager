using System.Windows.Input;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public static class ExtensionManagerWindowCommands
    {
        public readonly static RoutedCommand PackageOperationCommand = new RoutedCommand();

        public readonly static RoutedCommand ShowOptionsPage = new RoutedCommand();
        public readonly static RoutedCommand FocusOnSearchBox = new RoutedCommand();
        public readonly static RoutedCommand OpenExternalLink = new RoutedCommand();
    }
}