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
        // Filter only expired products
        UpdateExpiredProducts();
        NumberOfProducts = $"{ExpiredProducts.Count()} Produtos";
    }

    private void UpdateExpiredProducts()
    {
        if (Products != null)
        {
            var tempList = Products.Where(static item => item.TimeRemaining == "Vencido").ToList();
            ExpiredProducts = new ObservableCollection<Product>(tempList);
        }
    }
    
    [ObservableProperty]
    private Product? _dataGridSelectedProduct;

    [ObservableProperty]
    private string _numberOfProducts;

    // Expired products
    //================================================
    private ObservableCollection<Product>? _expiredProducts;
    public ObservableCollection<Product> ExpiredProducts
    {
        get => _expiredProducts!;
        set => SetProperty(ref _expiredProducts, value);
    }
    //================================================
    
    // Combobox
    //================================================
    private ComboBoxItem? _comboBoxSelectedItem;
    public ComboBoxItem? ComboBoxSelectedItem
    {
        get => _comboBoxSelectedItem;

        set
        {
            if (value != null)
            {
                SetProperty(ref _comboBoxSelectedItem, value);
            }
        }
    }
    //================================================
    
    // Search Bar
    //================================================
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
    //================================================

    // Search button
    //================================================
    [RelayCommand]
    private void SearchButton()
    {
        if (ComboBoxSelectedItem == null || string.IsNullOrEmpty(SearchTextBox)) return;
        if (ComboBoxSelectedItem.Tag == null || Products == null) return;
        
        List<Product> tempList;
        
        switch (ComboBoxSelectedItem.Tag)
        {
            case "Name":
                tempList = Products.Where(x => x.Name.Contains(SearchTextBox.ToUpper())).ToList();
                ExpiredProducts = new ObservableCollection<Product>(tempList);
                break;
            case "CodeBar":
                tempList = Products.Where(x => x.CodeBar.ToString() == SearchTextBox.ToString()).ToList();
                ExpiredProducts = new ObservableCollection<Product>(tempList);
                break;
        }
    }
    //================================================

    // Search clear button
    //================================================
    [RelayCommand]
    private void ClearSelection() 
    {
        SearchTextBox = "";
        if (Products == null) return;
        ExpiredProducts = new ObservableCollection<Product>(Products);
    }
    //================================================
    
    // Remove button
    //================================================
    [RelayCommand]
    private void RemoveProduct()
    {
        if (DataGridSelectedProduct == null) return;
        DeleteNotificationOpacity = 1;
        DeleteNotificationPopup = true;
    }
    //================================================
    
    // Confirm Delete
    [RelayCommand]
    private void ConfirmDelete()
    {
        
        ConfirmRemove(DataGridSelectedProduct);
        UpdateExpiredProducts();
    }
    
    [RelayCommand]
    private void DenyDelete()
    {
        DeleteNotificationOpacity = 0;
        DeleteNotificationPopup = false;
    }
}
