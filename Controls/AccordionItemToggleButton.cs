using System.Windows;
using System.Windows.Controls.Primitives;

namespace Controls
{
    public class AccordionItemToggleButton : ToggleButton
    {
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.RegisterAttached("IsExpanded", typeof(bool), typeof(AccordionItemToggleButton), new FrameworkPropertyMetadata(false));

        public Visibility ArrowVisibility
        {
            get { return (Visibility)GetValue(ArrowVisibilityProperty); }
            set { SetValue(ArrowVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ArrowVisibilityProperty =
        DependencyProperty.RegisterAttached("ArrowVisibility", typeof(Visibility), typeof(AccordionItemToggleButton), new FrameworkPropertyMetadata(Visibility.Visible));

        protected override void OnToggle()
        {
            if (!IsExpanded)
            {
                base.OnToggle();
            }
        }
    }
}
