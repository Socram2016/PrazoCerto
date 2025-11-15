using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using PrazoCerto.ViewModels;
using PrazoCerto.Views;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using PrazoCerto.Models;
using System;

namespace PrazoCerto;

public class App : Application
{
    // Directories
    //================================================
    private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string DataDirectory = BaseDirectory+Path.DirectorySeparatorChar+"dados";
    protected readonly string ProductsFilePath = Path.Combine(DataDirectory, "ProductsDatabase.json");
    //================================================
        
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }
        
        // Update Time Remaining of the json
        //================================================
        string stringFromFile = File.ReadAllText(ProductsFilePath);
        List<Product>? products = JsonConvert.DeserializeObject<List<Product>>(stringFromFile);
        if (products == null) return;
            
        foreach (var t in products)
        { 
            t.TimeRemaining = t.UpdateTimeRemaining(t.ExpirationDate);
        } 
        string stringToJson = JsonConvert.SerializeObject(products, Formatting.Indented);
        File.WriteAllText(ProductsFilePath, stringToJson);
        //================================================

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}