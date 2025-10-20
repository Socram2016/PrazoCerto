using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Input;
using PrazoCerto.Views;

namespace PrazoCerto.ViewModels;

public partial class AddProductPageViewModel : ViewModelBase
{
    // Save button
    [RelayCommand]
    private void SaveButton()
    {
        
        if (!ValidateForm()) return;
        
        // save in json
        ProductFormField.SaveProduct(NewProductName,
            NewProductCodeBar,
            NewProductDay,
            NewProductMonth,
            NewProductYear,
            NewProductAmount,
            ProductsFilePath);
        
        UpdateProducts();
        ResetBrushes();
        ResetTexts();
        _ = SaveNotification();
    }
    
    public ObservableCollection<SyncItemTemplate> SyncItems { get; } = new()
    {
        new SyncItemTemplate("Teste"),
        new SyncItemTemplate("Teste"),
        new SyncItemTemplate("Teste"),
        new SyncItemTemplate("Teste")
    };
}

public class SyncItemTemplate
{
    public string? Label { get; }

    public SyncItemTemplate(string? label)
    {
        Label = label;
    }
}
