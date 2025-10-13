using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Input;
using PrazoCerto.Views;

namespace PrazoCerto.ViewModels;

public partial class AddProductPageViewModel : ViewModelBase
{
    [RelayCommand]
    private void NotificationButton()
    {
        Notification();
    }
    // Save button
    [RelayCommand]
    private void SaveButton()
    {
        if (!ValidateForm()) return;

        ProductFormField.SaveProduct(NewProductName,
            NewProductCodeBar,
            NewProductDay,
            NewProductMonth,
            NewProductYear,
            NewProductAmount,
            ConfigFilePath);
        UpdateProducts();
        ResetBrushes();
        ResetTexts();
        Notification();
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
