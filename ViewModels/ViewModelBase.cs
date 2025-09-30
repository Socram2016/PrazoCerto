using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PrazoCerto.Models;

namespace PrazoCerto.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    public ViewModelBase()
    {
        if (!File.Exists(configFilePath))
            File.WriteAllText(configFilePath, "[]");

        stringFromJson = File.ReadAllText(configFilePath);
        Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(stringFromJson) ?? new ObservableCollection<Product>();
    }
    private ObservableCollection<Product>? _products;

    public ObservableCollection<Product> Products
    {
        get => _products!;
        set => SetProperty(ref _products, value);
    }

    // pega o caminho do arquivo json que armazena os produtos
    public string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductsDatabase.json");
    public string? stringFromJson;
    
}
