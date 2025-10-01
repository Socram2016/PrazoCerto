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
    private string _textBoxNameText;

    // Cor do brush do codigo de barras
    [ObservableProperty]
    private IBrush _textBoxCodeBarBrush = Brushes.DarkGray;

    // Cor do brush do dia
    [ObservableProperty]
    private IBrush _textBoxDayBrush = Brushes.DarkGray;

    // Cor do brush do mês
    [ObservableProperty]
    private IBrush _textBoxMouthBrush = Brushes.DarkGray;

    // Cor do brush do ano
    [ObservableProperty]
    private IBrush _textBoxYearBrush = Brushes.DarkGray;

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
        TextBoxNameText = DataGrid_SelectedProduct.Name;
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
        
    }
}

