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
        private ListItemTemplate? _selectedListItem;
        

        [ObservableProperty]
        private bool _isPaneOpen;

        [RelayCommand]
        private void OpenPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        public ObservableCollection<ListItemTemplate> Items { get; } =
        [
            new (typeof(AddProductPageViewModel), iconKey: "add_square_regular", label: "Adicionar Produto"),
            new (typeof(ExpiredProductPageViewModel), iconKey: "clock_regular", label: "Produtos Vencidos"),
            new (typeof(ProductsPageViewModel), iconKey: "bag_2d_regular", label: "Lista de Produtos"),
            new (typeof(ConfigPageViewModel), iconKey: "Gear_regular", label: "Configurações")
        ];

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null || value.ModelType is null) return;
            var instance = Activator.CreateInstance(value.ModelType);
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
        public Type? ModelType { get; }

        public ListItemTemplate(Type? type, string? label, string? iconKey)
        {
            Label = label;
            ModelType = type;

            StreamGeometry? geometry = null;
            if (iconKey != null && Application.Current != null && Application.Current.TryFindResource(iconKey, out var icon))
            {
                geometry = icon as StreamGeometry;
            }
            
            ListItemIcon = geometry;
        }
    }
}

