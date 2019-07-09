using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Controls
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is double)
                    result *= (double)values[i];
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }

    public class AccordionItemToggleButton : ToggleButton
    {
        public bool IsExpanded { get; set; }

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

    /// <summary>
    /// Interaction logic for AccordionItem.xaml
    /// </summary>
    public partial class AccordionItem : HeaderedContentControl, INotifyPropertyChanged
    {
        #region overrides

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion overrides

        public delegate void OnExpanded(AccordionItem item);
        public delegate void OnCollapsed(AccordionItem item);
        public event OnExpanded OnExpandedEvent;
        public event OnCollapsed OnCollapsedEvent;

        public AccordionItem()
        {
            InitializeComponent();           
        }

        #region event handlers        

        private void OnAccordionItemControlLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            if (OnExpandedEvent != null)
            {
                OnExpandedEvent(this);
            }

            var item = Template.FindName("ExpanderButton", this) as AccordionItemToggleButton;
            if (item != null)
            {
                item.IsExpanded = true;
            }
        }

        private void OnToggleButtonUnChecked(object sender, RoutedEventArgs e)
        {
            if (OnCollapsedEvent != null)
            {
                OnCollapsedEvent(this);
            }

            var item = Template.FindName("ExpanderButton", this) as AccordionItemToggleButton;
            if (item != null)
            {
                item.IsExpanded = false;
            }
        }

        #endregion

        #region methods        

        internal void Collapse()
        {
            IsExpanded = false;
        }

        internal void Expand()
        {
            IsExpanded = true;
        }

        #endregion

        #region properties

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsItemExpandedProperty); }
            private set { SetValue(IsItemExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsItemExpandedProperty =
        DependencyProperty.RegisterAttached("IsExpanded", typeof(bool), typeof(AccordionItem), new FrameworkPropertyMetadata(false));

        public Visibility ArrowVisibility
        {
            get { return (Visibility)GetValue(ArrowVisibilityProperty); }
            set { SetValue(ArrowVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ArrowVisibilityProperty =
        DependencyProperty.RegisterAttached("ArrowVisibility", typeof(Visibility), typeof(AccordionItem), new FrameworkPropertyMetadata(Visibility.Visible));

        #endregion properties
    }
}