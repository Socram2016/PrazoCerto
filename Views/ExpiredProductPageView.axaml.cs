using System;
using Avalonia.Controls;
using Avalonia.Layout;

namespace PrazoCerto.Views;

public partial class ExpiredProductPageView : UserControl
{
    public ExpiredProductPageView()
    {
        InitializeComponent();
        MyBorder.LayoutUpdated += OnMyBorderAttachedToVisualTree;
    }

    private void OnMyBorderAttachedToVisualTree(object? sender, EventArgs e)
    {
        if (MyBorder.Bounds.Height > 0)
        {
            double myBoderHeight = MyBorder.Bounds.Height;
            MyDataGrid.Height = myBoderHeight - 100;
        }

    }
}