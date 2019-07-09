using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Controls
{
    #region commands

    public class ToggleCommand : ICommand
    {
        private AccordionItem owner;

        public ToggleCommand(AccordionItem owner)
        {
            this.owner = owner;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
        
        public bool CanExecute(object parameter)
        {
            return !owner.IsExpanded;
        }

        public void Execute(object parameter)
        {
            owner.OnToggle();            
        }
    }

    #endregion

    public class AccordionItem : HeaderedContentControl, INotifyPropertyChanged
    {
        #region overrides

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion overrides

        public delegate void OnExpanded(AccordionItem item);
        public delegate void OnCollapsed(AccordionItem item);
        public event OnExpanded OnExpandedEvent;
        public event OnCollapsed OnCollapsedEvent;

        public AccordionItem()
        {
            ToggleCommand = new ToggleCommand(this);
        }

        #region event handlers

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var resources = new ResourceDictionary
            {
                Source = new Uri("/Controls;component/AccordionItem.xaml", UriKind.RelativeOrAbsolute)
            };
            Resources = resources;
            Template = resources["RevealExpanderTemp"] as ControlTemplate;
        }

        private void OnAccordionItemControlLoaded(object sender, RoutedEventArgs e)
        {
        }       

        #endregion

        #region methods     

        internal void OnToggle()
        {            
            if (IsExpanded)
            {
                OnCollapsedEvent?.Invoke(this);

                Collapse();                
            }
            else
            {
                OnExpandedEvent?.Invoke(this);

                Expand();                
            }
        }

        internal void Collapse()
        {
            IsExpanded = false;

            var item = Template.FindName("ExpanderButton", this) as AccordionItemToggleButton;
            if (item != null)
            {
                item.IsExpanded = false;
            }
        }

        internal void Expand()
        {
            IsExpanded = true;

            var item = Template.FindName("ExpanderButton", this) as AccordionItemToggleButton;
            if (item != null)
            {
                item.IsExpanded = true;                
            }
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

        public ICommand ToggleCommand
        {
            get { return (ICommand)GetValue(ToggleCommandProperty); }
            set { SetValue(ToggleCommandProperty, value); }
        }

        public static readonly DependencyProperty ToggleCommandProperty =
        DependencyProperty.RegisterAttached("ToggleCommand", typeof(ICommand), typeof(AccordionItem), new FrameworkPropertyMetadata(null));

        #endregion properties        
    }
}
