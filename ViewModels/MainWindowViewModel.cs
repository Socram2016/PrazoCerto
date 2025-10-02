using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PrazoCerto.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _texto = "PrazoCerto";

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;

        

        [ObservableProperty]
        private bool _isPaneOpen = false;

        [RelayCommand]
        private void OpenPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        public ObservableCollection<ListItemTemplate> Items { get; } = new()
        {
            new ListItemTemplate(typeof(AddProductPageViewModel), iconKey: "add_square_regular", label: "Adicionar Produto"),
            new ListItemTemplate(typeof(ExpiredProductPageViewModel), iconKey: "clock_regular", label: "Produtos Vencidos"),
            new ListItemTemplate(typeof(ProductsPageViewModel), iconKey: "bag_2d_regular", label: "Lista de Produtos")
        };

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null || value.modelType is null) return;
            var instance = Activator.CreateInstance(value.modelType);
            if (instance == null) return;
            CurrentPage = (ViewModelBase)instance;
        }

        [ObservableProperty]
        private double _screenWidth;

        [ObservableProperty]
        private double _screenHeight;
    }



    public class ListItemTemplate
    {
        public StreamGeometry? ListItemIcon { get; }
        public string? Label { get; }
        public Type? modelType { get; }

        public ListItemTemplate(Type? type, string? label, string? iconKey)
        {
            Label = label;
            modelType = type;

            StreamGeometry? geometry = null;
            if (iconKey != null && Application.Current != null && Application.Current.TryFindResource(iconKey, out var icon))
            {
                geometry = icon as StreamGeometry;
            }
            
            ListItemIcon = geometry;
        }
    }
}

