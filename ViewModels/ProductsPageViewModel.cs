
using System.Collections.ObjectModel;
using PrazoCerto.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using System.Linq;
using System;
using System.Collections.Generic;
using PrazoCerto.Views;

namespace PrazoCerto.ViewModels;

public partial class ProductsPageViewModel : ViewModelBase
{
    public ProductsPageViewModel()
    {
        if (Products != null )ProductsList = new ObservableCollection<Product>(Products);
    }

    [ObservableProperty] private Product? _dataGridSelectedProduct;

    [ObservableProperty] private bool _isEditPopupOpen;

    [ObservableProperty] private ObservableCollection<Product>? _productsList;

    private ComboBoxItem? _comboBoxSelectedItem;
    public ComboBoxItem? ComboBoxSelectedItem
    {
        get => _comboBoxSelectedItem;
        set
        {
            if (value != null) SetProperty(ref _comboBoxSelectedItem, value);
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

    //botão de pesquisa
    [RelayCommand]
    private void SearchButton()
    {
        if (ComboBoxSelectedItem == null ||
            string.IsNullOrEmpty(SearchTextBox)) return;
        
        
        if (ComboBoxSelectedItem.Tag == null) return;
        if (Products == null) return;
        
        List<Product> tempList;
        switch (ComboBoxSelectedItem.Tag)
        {
            case "Name":
                tempList = Products.Where(x => x.Name.Contains(SearchTextBox, StringComparison.CurrentCultureIgnoreCase)).ToList();
                ProductsList = new ObservableCollection<Product>(tempList);
                break;
            case "CodeBar":
                    tempList = Products.Where(x => x.CodeBar.ToString() == SearchTextBox).ToList();
                ProductsList = new ObservableCollection<Product>(tempList);
                break;
        }
    }

    // Botão de limpar seleção
    [RelayCommand]
    private void ClearSelection() //botão de limpeza da pesquisa
    {
        SearchTextBox = "";
        if (Products != null) ProductsList = new ObservableCollection<Product>(Products);
    }

    // Delete button
    [RelayCommand]
    private void DeleteButton()
    {
        if (DataGridSelectedProduct == null || ProductsList == null) return;
        DeleteNotificationOpacity = 1;
        DeleteNotificationPopup = true;
    }

    // Confirm Delete
    [RelayCommand]
    private void ConfirmDelete()
    {
        if (DataGridSelectedProduct == null || ProductsList == null) return;
        
        RemoveItem(DataGridSelectedProduct);
        ProductsList.Remove(DataGridSelectedProduct);
        DeleteNotificationOpacity = 0;
        DeleteNotificationPopup = false;
        UpdateProducts();
    }
    
    // Add Product
    [RelayCommand]
    private void AddButton()
    {
        DataGridSelectedProduct = null;
        IsEditPopupOpen = !IsEditPopupOpen;
    }
    // Deny delete
    [RelayCommand]
    private void DenyDelete()
    {
        DeleteNotificationOpacity = 0;
        DeleteNotificationPopup = false;
    }
    
    // Close PopUp
    [RelayCommand]
    private void ClosePopUp()
    {
        IsEditPopupOpen = !IsEditPopupOpen;
        ClearFields();
    }
    
    // Clear fields
    private void ClearFields()
    {
        NewProductName =
            NewProductCodeBar =
                NewProductDay =
                    NewProductMonth =
                        NewProductYear =
                            NewProductAmount = string.Empty;
    }
    
    // Edit product
    [RelayCommand]
    private void EditProduct()
    {
        if (DataGridSelectedProduct == null) return;
        
            var itemSelected = DataGridSelectedProduct;
        
            IsEditPopupOpen = !IsEditPopupOpen;
        
            // get all information
            NewProductName = itemSelected.Name;
            NewProductCodeBar = itemSelected.CodeBar.ToString();
            NewProductDay = itemSelected.ExpirationDate.Day.ToString();
            NewProductMonth = itemSelected.ExpirationDate.Month.ToString();
            NewProductYear = itemSelected.ExpirationDate.Year.ToString();
            NewProductAmount = itemSelected.Amount.ToString();
            
    }

    // Save product
    [RelayCommand]
    private void SaveProduct()
    {
        if (!ValidateForm()) return;
        
        if (DataGridSelectedProduct != null)
        {
            ConfirmDelete();
        }
        
        ProductFormField.SaveProduct(NewProductName,
            NewProductCodeBar,
            NewProductDay,
            NewProductMonth,
            NewProductYear,
            NewProductAmount,
            ProductsFilePath);
        
        UpdateProducts();
        
        if (Products != null) ProductsList = new ObservableCollection<Product>(Products);
        
        _ = SaveNotification();
        IsEditPopupOpen = !IsEditPopupOpen;
    }
}

