using System;
using Avalonia.Controls;

namespace PrazoCerto.Views;

    public partial class ProductsPageView : UserControl
    {
        public ProductsPageView()
        {
            InitializeComponent();
            // update
            MyBorder.LayoutUpdated += OnMyBorderAttachedToVisualTree;
        }


        private void OnMyBorderAttachedToVisualTree(object? sender, EventArgs e)
        {
            // Set DataGriHeight
            if (!(MyBorder.Bounds.Height > 0)) return;
            
            var myBorderHeight = MyBorder.Bounds.Height;
            MyDataGrid.Height = myBorderHeight - 100;

        }
        
    }


