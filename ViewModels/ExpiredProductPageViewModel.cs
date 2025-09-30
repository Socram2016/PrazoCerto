using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PrazoCerto.Models;

namespace PrazoCerto.ViewModels;

public partial class ExpiredProductPageViewModel : ViewModelBase
{
    public ExpiredProductPageViewModel()
    {
        // Filtra apenas os produtos vencidos
        var tempList = Products.Where(static item => item.TimeRemaining == "Vencido").ToList();
        ExpiredProducts = new ObservableCollection<Product>(tempList);

        NumberOfProducts = $"{ExpiredProducts.Count()} Produtos";
    }
    
    [ObservableProperty]
    private Product? _dataGrid_SelectedProduct;

    [ObservableProperty]
    private string _numberOfProducts;

    private ObservableCollection<Product>? _expiredProducts;
    public ObservableCollection<Product> ExpiredProducts
    {
        get => _expiredProducts!;
        set => SetProperty(ref _expiredProducts, value);
    }

    private ComboBoxItem? _comboBox_SelectedItem; // combobox
    public ComboBoxItem? ComboBox_SelectedItem
    {
        get => _comboBox_SelectedItem;

        set
        {
            if (value != null)
            {
                SetProperty(ref _comboBox_SelectedItem, value);
            }
        }
    }

    private string? _searchTextBox; // barra de pesquisa
    public string? SearchTextBox
    {
        get => _searchTextBox;
        set
        {
            if (value != null)
            {
                SetProperty(ref _searchTextBox, value);
            }
        }
    }

    //botão de pesquisa
    [RelayCommand]
    private void SearchButton() 
    {
        if (ComboBox_SelectedItem != null && !string.IsNullOrEmpty(SearchTextBox))
        {
            var tempList = new List<Product>();
            if (ComboBox_SelectedItem.Tag != null)
            {
                switch (ComboBox_SelectedItem.Tag)
                {
                    case "Name":
                        tempList = Products.Where(x => x.Name.Contains(SearchTextBox.ToUpper())).ToList();
                        ExpiredProducts = new ObservableCollection<Product>(tempList);
                        break;
                    case "CodeBar":
                        tempList = Products.Where(x => x.CodeBar.ToString() == SearchTextBox.ToString()).ToList();
                        ExpiredProducts = new ObservableCollection<Product>(tempList);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Botão de limpeza da pesquisa
    [RelayCommand]
    private void ClearSelection() 
    {
        SearchTextBox = "";
        ExpiredProducts = new ObservableCollection<Product>(Products);
    }
    
    // Botão de remover produto
    [RelayCommand]
    private void RemoveProduct()
    {
        if (DataGrid_SelectedProduct != null)
        {
            Products.Remove(DataGrid_SelectedProduct);
            ExpiredProducts.Remove(DataGrid_SelectedProduct);

            string strToJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
            File.WriteAllText(configFilePath, strToJson);
        }
    }
}
