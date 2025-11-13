using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PrazoCerto.Models;
using PrazoCerto.Views;

namespace PrazoCerto.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected ViewModelBase()
    {
        // check if dataDirectory exist
        if (!Directory.Exists(_dataDirectory))
            Directory.CreateDirectory(_dataDirectory);
        
        // check if ProductsFilePath exist
        if (!File.Exists(ProductsFilePath))
            File.WriteAllText(ProductsFilePath, "[]");
        
        UpdateProducts();
    }
    // Window Length
    //================================================
    [ObservableProperty] private double _screenWidth;
    [ObservableProperty] private double _screenHeight;
    //================================================
    
    // DataGrid height
    //================================================
    [ObservableProperty] private double _datagridHeight = 350;
    //================================================
    
    // Texts
    //================================================
    [ObservableProperty] private string _newProductName = string.Empty;
    [ObservableProperty] private string _newProductCodeBar = string.Empty;
    [ObservableProperty] private string _newProductDay = string.Empty;
    [ObservableProperty] private string _newProductMonth = string.Empty;
    [ObservableProperty] private string _newProductYear = string.Empty;
    [ObservableProperty] private string _newProductAmount = string.Empty;
    //================================================
    
    // Brushes
    //================================================
    [ObservableProperty] private IBrush _newProductNameBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductCodeBarBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductDayBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductMonthBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductYearBrush = Brushes.Black;
    [ObservableProperty] private IBrush _newProductAmountBrush = Brushes.Black;
    //================================================

    // Save Notification
    //================================================
    [ObservableProperty] private bool _saveNotificationPopup;
    [ObservableProperty] private double _saveNotificationOpacity;
    //================================================
    
    // Delete Notification
    //================================================
    [ObservableProperty] private bool _deleteNotificationPopup;
    [ObservableProperty] private double _deleteNotificationOpacity;
    //================================================
    
    // Add Product Popup
    //================================================
    [ObservableProperty] private bool _addProductPopup;
    //================================================
    
    // Expiration Notification
    //================================================
    [ObservableProperty] private int _configToExpirationNotif;
    [ObservableProperty] private bool _isExpiredNotificationOpen;
    [ObservableProperty] private PlacementMode _popupPlacement = PlacementMode.RightEdgeAlignedBottom;
    //================================================
    
    // Get Jsons path
    //================================================
    private static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private static string _dataDirectory = _baseDirectory+Path.DirectorySeparatorChar+"dados";
    protected readonly string ProductsFilePath = Path.Combine(_dataDirectory, "ProductsDatabase.json");
    protected readonly string ProgramConfigPath = Path.Combine(_baseDirectory, "Configs");
    //================================================

    // Store current page
    //================================================
    [ObservableProperty]
    private static ViewModelBase _currentPage = new ProductsPageViewModel();
    //================================================
    
    // Products
    //================================================
    private ObservableCollection<Product>? _products;
    public ObservableCollection<Product>? Products
    {
        get => _products!;
        set => SetProperty(ref _products, value);
    }
    //================================================
    
    

    protected bool ValidateForm()
    {
        // reset burshes
        ResetBrushes(); 
    
        // Variable to track if any error occurred
        var invalidForm = false;

        // Validate fields
    
        // Check if name is filled and set red if is not
        //================================================
        CheckFields(string.IsNullOrEmpty(NewProductName), ref invalidForm, 
            () => NewProductNameBrush = Brushes.Red);
        //================================================
        
        
        // Check if Date is valid
        //================================================
        int month = 0;
        int year = 0;
        
        var monthError = string.IsNullOrEmpty(NewProductMonth) ||
                            !int.TryParse(NewProductMonth, out month) ||
                            month <= 0 || month >12;

        var yearError = string.IsNullOrEmpty(NewProductYear) ||
                        !int.TryParse(NewProductYear, out year);
        
        var dayError = string.IsNullOrEmpty(NewProductDay) ||
                       !int.TryParse(NewProductDay, out var day) ||
                       !DateTime.TryParse($"{day}/{month}/{year}",out _);
        //================================================
        
        
        // set red if day is not valid
        CheckFields(dayError, ref invalidForm, 
            () => NewProductDayBrush = Brushes.Red);
        //================================================
        
        // set red if month is not
        CheckFields(monthError, ref invalidForm, 
            () => NewProductMonthBrush = Brushes.Red);
        //================================================
        
        // set red if year is not
        CheckFields(yearError, ref invalidForm, 
            () => NewProductYearBrush = Brushes.Red);
        //================================================


        // check if code bar is ok
        //================================================
        var codeBarError = string.IsNullOrEmpty(NewProductCodeBar) || 
                           !long.TryParse(NewProductCodeBar, out _);
        // set red if is not
        CheckFields(codeBarError, ref invalidForm, 
            () => NewProductCodeBarBrush = Brushes.Red);
        //================================================
        
        // check if amount is ok
        //================================================
        var amountError = string.IsNullOrEmpty(NewProductAmount) ||
                          !ToCheck.IsInt(NewProductAmount);
        // set red if is not
        CheckFields(amountError, ref invalidForm, 
            () => NewProductAmountBrush = Brushes.Red);
        //================================================

        
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
    
    private static void CheckFields(bool errorCondition, ref bool invalidForm, Action assignError)
    {
        if (!errorCondition) return;
        
        assignError();
        invalidForm = true;
    }

    // remove item from data grid
    public void RemoveItem(Product dataGridSelectedProduct)
    {
        // remove 
        Products?.Remove(dataGridSelectedProduct);
        // update json
        var stringToJson = JsonConvert.SerializeObject(Products,Formatting.Indented);
        File.WriteAllText(ProductsFilePath,stringToJson);
    }
    
    // update Products list
    public void UpdateProducts(bool getJson = true)
    {
        // get json
        string stringFromJson = File.ReadAllText(ProductsFilePath);
        Products?.Clear();
        
        // update Products variable
        Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(stringFromJson);
    }

    // Notification
    public async Task SaveNotification()
    {
        SaveNotificationPopup = true;
        SaveNotificationOpacity = 1.0;
        await Task.Delay(1000);
        SaveNotificationOpacity = 0.0;
        await Task.Delay(1000);
        SaveNotificationPopup = false;
    }
}
