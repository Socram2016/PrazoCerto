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
using Avalonia.Media;
using ReactiveUI;

namespace PrazoCerto.ViewModels;

public partial class ProductsPageViewModel : ViewModelBase
{
    public ProductsPageViewModel()
    {
        ProductsList = new ObservableCollection<Product>(Products);
    }

    // Armazena as caracteristicas do "Name" do popup
    [ObservableProperty]
    private IBrush _textBoxNameBrush = Brushes.DarkGray;
    [ObservableProperty]
    private string? _textBoxNameText;

    // Armazena as caracteristicas do "CodeBar" do popup
    [ObservableProperty]
    private IBrush _textBoxCodeBarBrush = Brushes.DarkGray;
    [ObservableProperty]
    private string? _textBoxCodeBarText;

    // Armazena as caracteristicas do "Day" do popup
    [ObservableProperty]
    private IBrush _textBoxDayBrush = Brushes.DarkGray;
    [ObservableProperty]
    private string? _textBoxDayText;

    // Armazena as caracteristicas do "Month" do popup
    [ObservableProperty]
    private IBrush _textBoxMonthBrush = Brushes.DarkGray;
    [ObservableProperty]
    private string? _textBoxMonthText;

    // Armazena as caracteristicas do "Year" do popup
    [ObservableProperty]
    private IBrush _textBoxYearBrush = Brushes.DarkGray;
    [ObservableProperty]
    private string? _textBoxYearText;

    // Armazena as caracteristicas do "Quantity" do popup
    [ObservableProperty]
    private IBrush _TextBoxAmountBrush = Brushes.DarkGray;
    [ObservableProperty]
    private string? _TextBoxAmountText;

    // Armazena qual produto está selecionado
    [ObservableProperty]
    private Product? _dataGrid_SelectedProduct;

    [ObservableProperty]
    private bool _isPopupOpen = false;

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

    // Botão de limpar seleção
    [RelayCommand]
    private void ClearSelection() //botão de limpeza da pesquisa
    {
        SearchTextBox = "";
        ProductsList = new ObservableCollection<Product>(Products);
    }

    // Botão de remover
    [RelayCommand]
    private void RemoveProduct()
    {
        if (DataGrid_SelectedProduct != null)
        {
            Products.Remove(DataGrid_SelectedProduct);
            ProductsList.Remove(DataGrid_SelectedProduct);

            string strToJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
            File.WriteAllText(configFilePath, strToJson);

        }
    }

    // Botão de Editar
    [RelayCommand]
    private void EditProduct()
    {
        if (DataGrid_SelectedProduct == null) return;

        // Pega os dados do produto e armazena
        string productName = DataGrid_SelectedProduct.Name;
        string productCodeBar = DataGrid_SelectedProduct.CodeBar.ToString();
        string productDay = DataGrid_SelectedProduct.ExpirationDate.Day.ToString();
        string productMonth = DataGrid_SelectedProduct.ExpirationDate.Month.ToString();
        string productYear = DataGrid_SelectedProduct.ExpirationDate.Year.ToString();
        string quantity = DataGrid_SelectedProduct.Amount.ToString();

        // Define o as informações do produto selecionado no popup
        TextBoxNameText = productName;
        TextBoxCodeBarText = productCodeBar;
        TextBoxDayText = productDay;
        TextBoxMonthText = productMonth;
        TextBoxYearText = productYear;
        TextBoxAmountText = quantity;

        IsPopupOpen = true;
    }

    // Botão de fechar
    [RelayCommand]
    private void ClosePopup()
    {
        IsPopupOpen = false;
    }

    [RelayCommand]
    private void Save()
    {
        // Check if all is filled
        bool isAllFilled = ToCheck.IsFilled(TextBoxNameText!,
                                            TextBoxCodeBarText!,
                                            TextBoxDayText!,
                                            TextBoxMonthText!,
                                            TextBoxYearText!,
                                            TextBoxAmountText!);

        // Check if all is valid
        bool isAllValid = ToCheck.IsValidCodeBar(TextBoxCodeBarText!) &&
                          ToCheck.IsValidDate(TextBoxDayText!,
                                              TextBoxMonthText!,
                                              TextBoxYearText!) &&
                          ToCheck.IsInt(TextBoxAmountText!);

        bool isAllOk = isAllFilled && isAllValid;

        if (isAllOk)
        {
            // Remove current product
            if (DataGrid_SelectedProduct == null) return;
            Product? productToRemove = Products.FirstOrDefault(x => x.Name == DataGrid_SelectedProduct.Name);
            if (productToRemove != null)
            {
                Products.Remove(productToRemove);
                ProductsList.Remove(productToRemove);
            }

            // Store the current product to add
            Product currentProduct = new Product
            (
                name: TextBoxNameText!.ToUpper(),
                codeBar: long.Parse(TextBoxCodeBarText!),
                expirationDate: new DateTime(int.Parse(TextBoxYearText!),
                                             int.Parse(TextBoxMonthText!),
                                             int.Parse(TextBoxDayText!)),
                amount: int.Parse(TextBoxAmountText!)
            );

            // Add Product
            Products.Add(currentProduct);
            ProductsList.Add(currentProduct);

            // Update Json
            List<Product> tempProductlist = new List<Product>(Products);
            string stringToJson = JsonConvert.SerializeObject(tempProductlist, Formatting.Indented);
            File.WriteAllText(configFilePath, stringToJson);

            // Close popup
            IsPopupOpen = false;            
        }
        else
        {
            // Set the correct one in red
            if (string.IsNullOrEmpty(TextBoxNameText)) TextBoxNameBrush = Brushes.Red;
            if (string.IsNullOrEmpty(TextBoxCodeBarText)) TextBoxCodeBarBrush = Brushes.Red;
            if (string.IsNullOrEmpty(TextBoxDayText)) TextBoxDayBrush = Brushes.Red;
            if (string.IsNullOrEmpty(TextBoxMonthText)) TextBoxMonthBrush = Brushes.Red;
            if (string.IsNullOrEmpty(TextBoxYearText)) TextBoxYearBrush = Brushes.Red;
            if (string.IsNullOrEmpty(TextBoxAmountText)) TextBoxAmountBrush = Brushes.Red;
        }

    }
}

