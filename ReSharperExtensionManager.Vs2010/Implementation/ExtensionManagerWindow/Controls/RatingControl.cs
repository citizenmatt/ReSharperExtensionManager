using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow.Controls
{
    public class RatingControl : RangeBase
    {
        private StackPanel rootElement;
        private ControlTemplate starTemplate;

        static RatingControl()
        {
            //Set so that individual StarControls don't get focus.
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));

            //Not Focusable by default
            FocusableProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(false));

            MaximumProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
            ValueProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(typeof(RatingControl)));
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateStarList();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            UpdateStarList();
        }

        private void UpdateStarList()
        {
            if (rootElement != null)
            {
                rootElement.Children.Clear();
                for (int i = 0; i < Maximum; ++i)
                {
                    StarControl star = new StarControl();
                    star.Value = Value - i;
                    star.Template = starTemplate;
                    rootElement.Children.Add(star);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.rootElement != null)
            {
                this.rootElement.SizeChanged -= new SizeChangedEventHandler(RootElement_SizeChanged);
            }

            rootElement = (StackPanel)Template.FindName("RootElement", this);
            starTemplate = rootElement.TryFindResource(new ComponentResourceKey(typeof(RatingControl), "StarTemplate")) as ControlTemplate;
            UpdateStarList();

            if (this.rootElement != null)
            {
                this.rootElement.SizeChanged += new SizeChangedEventHandler(RootElement_SizeChanged);
            }

        }

        void RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (StarControl star in rootElement.Children)
            {
                star.UpdateVisuals();
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new RangeBaseAutomationPeer(this);
        }
    }
}