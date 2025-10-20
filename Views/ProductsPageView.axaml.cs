using System;
using Avalonia;
using Avalonia.Controls;

namespace PrazoCerto.Views;

    public partial class ProductsPageView : UserControl
    {
        private IDisposable? _resizeSubscription;
        public ProductsPageView()
        {
            InitializeComponent();
            // update
            AttachedToVisualTree += OnAttachedToVisualTree;
        }

        private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (TopLevel.GetTopLevel(this) is Window parentWindow)
            {
                _resizeSubscription = parentWindow.GetObservable(TopLevel.ClientSizeProperty).
                    Subscribe(newSize =>
                    {
                        double newHeight = newSize.Height;
                        MyBorder.Height = newHeight - 100;
                    });
            }
        }
        
    }


