using System;
using Avalonia.Controls;

namespace PrazoCerto.Views
{
    public partial class ProductsPageView : UserControl
    {
        public ProductsPageView()
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
}

