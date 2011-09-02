using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace ShowMenuItem
{
    [ActionHandler("ShowMenuItem.Show")]
    public class ShowMenuItem_ShowActionHandler : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.Show(string.Format("Menu item plugin: {0}", GetType().Assembly.GetName().Version));
        }
    }
}
