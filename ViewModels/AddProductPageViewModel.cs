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

    }

}
