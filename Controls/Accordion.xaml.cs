using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    /// Interaction logic for Accordion.xaml
    /// </summary>
    public partial class Accordion : ItemsControl
    {        
        public Accordion()
        {
            InitializeComponent();            
        }

        #region event handlers

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {            
            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    accordionItem.ArrowVisibility = ShowArrow ? Visibility.Visible : Visibility.Hidden;
                    accordionItem.Collapse();
                    accordionItem.OnExpandedEvent += OnAccordionItemExpanded;
                    accordionItem.OnCollapsedEvent += OnAccordionItemCollapsed;
                }
            }

            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    accordionItem.Expand();
                    break;
                }
            }
        }

        void OnAccordionItemExpanded(AccordionItem expandedItem)
        {
            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    if (expandedItem != accordionItem)
                    {
                        if (accordionItem.IsExpanded)
                        {
                            accordionItem.Collapse();
                        }
                    }
                }
            } 
        }

        void OnAccordionItemCollapsed(AccordionItem collapsedItem)
        {
            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    if (collapsedItem != accordionItem)
                    {
                        if (accordionItem.IsExpanded)
                        {
                            return;
                        }
                    }
                }
            }

            collapsedItem.Expand();
        }

        #endregion

        #region properties

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }
        public static readonly DependencyProperty ExpandDirectionProperty =
        DependencyProperty.RegisterAttached("ExpandDirection", typeof(ExpandDirection), typeof(Accordion), new FrameworkPropertyMetadata(ExpandDirection.Down));

        public bool ShowArrow
        {
            get { return (bool)GetValue(ShowArrowProperty); }
            set 
            { 
                SetValue(ShowArrowProperty, value);                
            }
        }
        public static readonly DependencyProperty ShowArrowProperty =
        DependencyProperty.Register("ShowArrow", typeof(bool), typeof(Accordion), new FrameworkPropertyMetadata(true, OnShowArrowPropertyChanged));

        private static void OnShowArrowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Accordion;
            foreach (var item in control.Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    accordionItem.ArrowVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        #endregion
    }

}
