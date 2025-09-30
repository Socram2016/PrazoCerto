using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using PrazoCerto.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using System.Linq;
using System;
using Avalonia.Controls.Primitives;
using System.Collections.Generic;

namespace PrazoCerto.ViewModels;

public partial class ProductsPageViewModel : ViewModelBase
{
    public ProductsPageViewModel()
    {
        ProductsList = new ObservableCollection<Product>(Products);
    }

    [ObservableProperty]
    private Product? _dataGrid_SelectedProduct;


    [ObservableProperty]
    private ObservableCollection<Product> _productsList;

    private ComboBoxItem? _comboBox_SelectedItem;
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


    private string? _searchTextBox;
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

    [RelayCommand]
    private void SearchButton() //botão de pesquisa
    {


        if (ComboBox_SelectedItem != null && !string.IsNullOrEmpty(SearchTextBox))
        {
            var tempList = new List<Product>();
            if (ComboBox_SelectedItem.Tag != null)
            {
                switch (ComboBox_SelectedItem.Tag)
                {
                    case "Name":
                        tempList = Products.Where(x => x.Name.Contains(SearchTextBox)).ToList();
                        ProductsList = new ObservableCollection<Product>(tempList);
                        break;
                    case "CodeBar":
                        tempList = Products.Where(x => x.CodeBar.ToString() == SearchTextBox.ToString()).ToList();
                        ProductsList = new ObservableCollection<Product>(tempList);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [RelayCommand]
    private void ClearSelection() //botão de limpeza da pesquisa
    {
        SearchTextBox = "";
        ProductsList = new ObservableCollection<Product>(Products);
    }

    [RelayCommand]
    private void RemoveItem()
    {
        if (DataGrid_SelectedProduct != null)
        {
            Products.Remove(DataGrid_SelectedProduct);
            ProductsList.Remove(DataGrid_SelectedProduct);

            string strToJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
            File.WriteAllText(configFilePath, strToJson);
        }
    }

}

