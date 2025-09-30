
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

        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            // Reset borders color
            ProductNameTextBox.BorderBrush =
            CodeBarTextBox.BorderBrush =
            DayTextBox.BorderBrush =
            MouthTextBox.BorderBrush =
            YearTextBox.BorderBrush =
            AmountTextBox.BorderBrush = Avalonia.Media.Brushes.DarkGray;

            // Get values from text boxes
            string? productName = ProductNameTextBox.Text;
            string? codeBar = CodeBarTextBox.Text;
            string? day = DayTextBox.Text;
            string? mouth = MouthTextBox.Text;
            string? year = YearTextBox.Text;
            string? amount = AmountTextBox.Text;

            // Use ToCheck methods to validate inputs is filled
            bool isAllFilled = ToCheck.IsFilled(
                productName ?? string.Empty,
                codeBar ?? string.Empty,
                day ?? string.Empty,
                mouth ?? string.Empty,
                year ?? string.Empty,
                amount ?? string.Empty
            );

            // Use ToCheck methods to validate inputs is valid
            bool isAllValid = ToCheck.IsValidCodeBar(codeBar ?? string.Empty) &&
                              ToCheck.IsValidDate(day ?? string.Empty,
                                                  mouth ?? string.Empty,
                                                  year ?? string.Empty) &&
                              ToCheck.IsInt(amount ?? string.Empty);

            // Is all good
            bool isAllGood = isAllFilled && isAllValid;

            if (isAllGood)
            {
                
            }
            else
            {
                bool isNameFilled = ToCheck.IsFilled(productName!);
                bool isCodeBarFilled = ToCheck.IsFilled(codeBar!);
                bool isCodeBarValid = ToCheck.IsValidCodeBar(codeBar!);
                bool isDateFilled = ToCheck.IsFilled(day!, mouth!, year!);
                bool isDayFilled = ToCheck.IsFilled(day!);
                bool isDayValid = ToCheck.IsValidDate(day!, "1", "2000");
                bool isMouthFilled = ToCheck.IsFilled(mouth!);
                bool isMouthValid = ToCheck.IsValidDate("1", mouth!, "2000");
                bool isYearFilled = ToCheck.IsFilled(year!);
                bool isYearValid = ToCheck.IsValidDate("1", "1", year!);
                bool isAmountFilled = ToCheck.IsFilled(amount!);
                bool isAmountValid = ToCheck.IsInt(amount!);


                bool isDateValid = ToCheck.IsValidDate(day!, mouth!, year!);

                var redColor = Avalonia.Media.Brushes.Red;
                var grayColor = Avalonia.Media.Brushes.Gray;

                // Field validation
                ProductNameTextBox.BorderBrush = isNameFilled ? grayColor : redColor;
                CodeBarTextBox.BorderBrush = (isCodeBarFilled && isCodeBarValid) ? grayColor : redColor;
                AmountTextBox.BorderBrush = (isAmountFilled && isAmountValid) ? grayColor : redColor;

                // Date validation
                if (!isDateFilled || !isDateValid)
                {
                    DayTextBox.BorderBrush = (isDayFilled && isDayValid) ? grayColor : redColor;
                    MouthTextBox.BorderBrush = (isMouthFilled & isMouthValid) ? grayColor : redColor;
                    YearTextBox.BorderBrush = (isYearFilled & isYearValid) ? grayColor : redColor;
                }
            }
        }
    }
}

