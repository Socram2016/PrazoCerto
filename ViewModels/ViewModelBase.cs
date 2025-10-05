using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PrazoCerto.Models;

namespace PrazoCerto.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    // Texts
    //================================================
    [ObservableProperty] private string _newProductName = string.Empty;
    [ObservableProperty] private string _newProductCodeBar = string.Empty;
    [ObservableProperty] private string _newProductDay = string.Empty;
    [ObservableProperty] private string _newProductMonth = string.Empty;
    [ObservableProperty] private string _newProductYear = string.Empty;
    [ObservableProperty] private string _newProductAmount = string.Empty;
    //================================================
    
    // Brushs
    //================================================
    [ObservableProperty] private IBrush _newProductNameBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductCodeBarBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductDayBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductMonthBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductYearBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductAmountBrush = Brushes.Black;
    //================================================
    
    // Get JsonProducts path
    protected readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductsDatabase.json");

    // Store current page
    [ObservableProperty]
    private static ViewModelBase _currentPage = new AddProductPageViewModel();
    
    private ObservableCollection<Product>? _products;

    public ObservableCollection<Product>? Products
    {
        get => _products!;
        set => SetProperty(ref _products, value);
    }
    
    protected ViewModelBase()
    {
        if (!File.Exists(ConfigFilePath))
            File.WriteAllText(ConfigFilePath, "[]");

        var stringFromJson = File.ReadAllText(ConfigFilePath);
        Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(stringFromJson) ?? new ObservableCollection<Product>();
    }

    protected bool ValidateForm()
    {
        // reset burshes
        ResetBrushes(); 
    
        // 2. Variável para rastrear se QUALQUER erro ocorreu
        var invalidForm = false;

        // Validate fields
    
        // Check if name is filled
        CheckFields(string.IsNullOrEmpty(NewProductName), ref invalidForm, 
            () => NewProductNameBrush = Brushes.Red);
        
        // Check if day is filled
        var dayError = string.IsNullOrEmpty(NewProductDay)
                         || !int.TryParse(NewProductDay, out _);
        CheckFields(dayError, ref invalidForm, 
            () => NewProductDayBrush = Brushes.Red);
        
        // Check if month is filled
        var monthError = string.IsNullOrEmpty(NewProductMonth)
                             || !int.TryParse(NewProductMonth, out _);
        CheckFields(monthError, ref invalidForm, 
            () => NewProductMonthBrush = Brushes.Red);
        
        // Check if year is filled
        var yearError = string.IsNullOrEmpty(NewProductYear)
                       || !int.TryParse(NewProductYear, out _);
        CheckFields(yearError, ref invalidForm, 
            () => NewProductYearBrush = Brushes.Red);


        // check if code bar is ok
        var codeBarError = string.IsNullOrEmpty(NewProductCodeBar) || 
                           !long.TryParse(NewProductCodeBar, out _);
        // set red if is not
        CheckFields(codeBarError, ref invalidForm, 
            () => NewProductCodeBarBrush = Brushes.Red);
        
        // check if amount is ok
        var amountError = string.IsNullOrEmpty(NewProductAmount) ||
                          !ToCheck.IsInt(NewProductAmount);
        
        // set red if is not
        CheckFields(amountError, ref invalidForm, 
            () => NewProductAmountBrush = Brushes.Red);

        // return 
        return !invalidForm;
    }

    protected void ResetTexts()
    {
        NewProductName = "";
        NewProductCodeBar = "";
        NewProductDay = "";
        NewProductMonth = "";
        NewProductYear = "";
        NewProductAmount = "";
    }

    protected void ResetBrushes()
    {
        IBrush corPadrão = Brushes.Black;

        NewProductNameBrush = corPadrão;
        NewProductCodeBarBrush = corPadrão;
        NewProductDayBrush = corPadrão;
        NewProductMonthBrush = corPadrão;
        NewProductYearBrush = corPadrão;
        NewProductAmountBrush = corPadrão;
    }
    
    // check fields
    private static void CheckFields(bool errorCondition, ref bool invalidForm, Action assignError)
    {
        if (!errorCondition) return;
        
        assignError();
        invalidForm = true;
    }

    public void RemoveItem(Product dataGridSelectedProduct)
    {
        Products?.Remove(dataGridSelectedProduct);
        var stringToJson = JsonConvert.SerializeObject(Products,Formatting.Indented);
        File.WriteAllText(ConfigFilePath,stringToJson);
    }

    public void UpdateProducts()
    {
        var stringFromJson = File.ReadAllText(ConfigFilePath);
        Products?.Clear();
        Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(stringFromJson);
    }
    
    
}
