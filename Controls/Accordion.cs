using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Controls
{
    public class Accordion : ItemsControl
    {
        public Accordion()
        {
            Loaded += OnControlLoaded;
        }

        #region methods

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var factoryPanel = new FrameworkElementFactory(typeof(StackPanel));
            factoryPanel.SetValue(StackPanel.IsItemsHostProperty, true);

            if (ExpandDirection == ExpandDirection.Up || ExpandDirection == ExpandDirection.Down)
            {
                factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            }
            else
            {
                factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            }

            var template = new ItemsPanelTemplate
            {
                VisualTree = factoryPanel
            };
            ItemsPanel = template;
        }

        #endregion

        #region event handlers

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            var nonAccordionItems = new List<object>();
            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    RegisterAccordionItem(accordionItem);
                }
                else
                {
                    nonAccordionItems.Add(item);
                }
            }

            foreach (var item in nonAccordionItems)
            {
                Items.Remove(item); // remove from items

                // create accordion item for it
                var newAccordionItem = new AccordionItem
                {
                    Content = item,
                    Header = new TextBlock()
                };
                Items.Add(newAccordionItem);
                RegisterAccordionItem(newAccordionItem);
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

        private void RegisterAccordionItem(AccordionItem item)
        {
            item.ArrowVisibility = ShowArrow ? Visibility.Visible : Visibility.Hidden;
            item.Collapse();
            item.OnExpandedEvent += OnAccordionItemExpanded;
            item.OnCollapsedEvent += OnAccordionItemCollapsed;
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
