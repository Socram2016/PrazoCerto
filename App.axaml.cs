using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
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

public partial class App : Application
{
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

        string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ProductsDatabase.json");
        if (File.Exists(configFilePath))
        {

            string jsonProducts = File.ReadAllText(configFilePath);
            var products = JsonConvert.DeserializeObject<List<Product>>(jsonProducts);

            // atualiza a data de validade
            foreach (var product in products!)
            {
                if (product.TimeRemaining != "Vencido")
                {
                    var daysToExpiration = (product.ExpirationDate - DateTime.Now).Days;

                    if (daysToExpiration <= 0)
                    {
                        product.TimeRemaining = "Vencido";
                    }
                    else
                    {
                        product.TimeRemaining = $"{(product.ExpirationDate - DateTime.Now).Days} dias";
                    }
                    
                }
            }
            
            
            string productsToJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(configFilePath, productsToJson);

        }


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