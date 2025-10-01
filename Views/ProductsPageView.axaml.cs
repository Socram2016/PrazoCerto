
using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Newtonsoft.Json;
using PrazoCerto.Models;

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

        // Set para apenas 2 digitos
        private void OnTwoDigitInput(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            if (textBox.Text?.Length >= 2)
            {
                textBox.Text = textBox.Text[..2];
            }
        }

        // Set para apenas 4 digitos
        private void OnThreeDigitInput(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            if (textBox.Text?.Length >= 4)
            {
                textBox.Text = textBox.Text[..4];
            }
        }

        
        
    }
}

