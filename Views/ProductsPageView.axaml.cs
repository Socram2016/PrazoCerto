using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Newtonsoft.Json;
using PrazoCerto.Models;
using PrazoCerto.ViewModels;

namespace PrazoCerto.Views;

    public partial class ProductsPageView : UserControl
    {
        private readonly DateTime _currenteDateTime = DateTime.Now;
        
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string DataDirectory = BaseDirectory+Path.DirectorySeparatorChar+"dados";
        protected readonly string ProductsFilePath = Path.Combine(DataDirectory, "ProductsDatabase.json");
        private IDisposable? _resizeSubscription;
        
        
        public ProductsPageView()
        {
            InitializeComponent();
            // update
            AttachedToVisualTree += OnAttachedToVisualTree;
            UpdateProductFile();
            
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

        private void UpdateProductFile()
        {
            string stringFromFile = File.ReadAllText(ProductsFilePath);
            var products = JsonConvert.DeserializeObject<List<Product>>(stringFromFile);
            if (products == null) return;
            
            foreach (var t in products)
            {
                t.TimeRemaining = t.UpdateTimeRemaining(t.ExpirationDate);
            }

            string stringToJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(ProductsFilePath, stringToJson);

        }
        
    }


