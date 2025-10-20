using System;
using Avalonia;
using Avalonia.Controls;

namespace PrazoCerto.Views;

public partial class AddProductPageView : UserControl
{
    private IDisposable? _resizeSubscription;
    public AddProductPageView()
    {
        InitializeComponent();
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window parentWindow)
        {
            _resizeSubscription = parentWindow.GetObservable(TopLevel.ClientSizeProperty).Subscribe(newSize =>
            {
                double newHeiht = newSize.Height;

                MyBorder.Height = newHeiht - 100;
            });
        }
    }
}