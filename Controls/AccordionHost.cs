using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Controls
{
    public class AccordionHost : ItemsControl
    {
        public AccordionHost()
        {
            Loaded += OnControlLoaded;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            FrameworkElementFactory factoryPanel = new FrameworkElementFactory(typeof(StackPanel));
            factoryPanel.SetValue(StackPanel.IsItemsHostProperty, true);

            if (ExpandDirection == ExpandDirection.Up || ExpandDirection == ExpandDirection.Down)
            {
                factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            }
            else
            {
                factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            }

            ItemsPanelTemplate template = new ItemsPanelTemplate();
            template.VisualTree = factoryPanel;
            ItemsPanel = template;            
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
        DependencyProperty.RegisterAttached("ExpandDirection", typeof(ExpandDirection), typeof(AccordionHost), new FrameworkPropertyMetadata(ExpandDirection.Down));

        public bool ShowArrow
        {
            get { return (bool)GetValue(ShowArrowProperty); }
            set 
            { 
                SetValue(ShowArrowProperty, value);                
            }
        }
        public static readonly DependencyProperty ShowArrowProperty =
        DependencyProperty.Register("ShowArrow", typeof(bool), typeof(AccordionHost), new FrameworkPropertyMetadata(true, OnShowArrowPropertyChanged));

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
