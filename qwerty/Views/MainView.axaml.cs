using Avalonia;
using Avalonia.Controls;
using qwerty.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.ConstrainedExecution;
using static qwerty.AmazonMusicParser;

namespace qwerty.Views;

public partial class MainView : UserControl
{
    
    public MainView()
    {
        InitializeComponent();
        
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {

        Debug.WriteLine("Ya vizvalos");
        if (e.AddedItems.Count > 0)
        {
            

            var selectedAlbum = e.AddedItems[0] as AlbumViewModel;

            // Отримуємо ViewModel з контексту даних вікна
            var viewModel = this.DataContext as MainViewModel;

            if (viewModel != null)
            {
                Debug.WriteLine($"Ya vizvalos i seichas functioa s ssilkoi {selectedAlbum.Href}");
                // Викликаємо подію для запуску завантаження пісень
                viewModel.LoadSongs(selectedAlbum.Href);
            }
        }
    }
}
