using Avalonia.Controls;
using Avalonia.Interactivity;
using OpenQA.Selenium;
using qwerty.Views;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static qwerty.AmazonMusicParser;

namespace qwerty.ViewModels;

public class MainViewModel : ViewModelBase
{
    public event PropertyChangedEventHandler PropertyChanged;
    private AlbumViewModel selectedItem;
    public AlbumViewModel SelectedItem
    {
        get => selectedItem;
        set => this.RaiseAndSetIfChanged(ref selectedItem, value);
    }



    private List<AlbumViewModel> _albums;
    public List<AlbumViewModel> Albums
    {
        get => _albums;
        set
        {
            _albums = value;
            OnPropertyChanged();
        }
    }

    private List<SongViewModel> _albumSongs;
    public List<SongViewModel> AlbumSongs
    {
        get => _albumSongs;
        set
        {
            _albumSongs = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<AlbumViewModel> _albumes;
    public ObservableCollection<AlbumViewModel> albumes
    {
        get => _albumes;
        set => this.RaiseAndSetIfChanged(ref _albumes, value);
    }

    private ObservableCollection<SongViewModel> _songs;
    public ObservableCollection<SongViewModel> Songs
    {
        get => _songs;
        set => this.RaiseAndSetIfChanged(ref _songs, value);
    }

    public void ParseAlbumsButton_Click()
    {
        var parser = new AmazonMusicParser();
        
        Albums = parser.ParseAmazonAlbums();

        albumes = new ObservableCollection<AlbumViewModel>();

        foreach (var alb in Albums)
        {
            albumes.Add(alb);
        }
        
    }
    public void LoadSongs(string albumUrl)
    {
        Debug.WriteLine(albumUrl);
        var parser = new AmazonMusicParser();

        Songs = new ObservableCollection<SongViewModel>();

        if (!string.IsNullOrEmpty(albumUrl))
        {
            Debug.WriteLine(albumUrl);
            var songLinks = parser.ParseSongs(albumUrl);
            Songs.Clear();
            foreach (var songLink in songLinks)
            {
                Songs.Add(songLink);
            }
        }
    }

 
    

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
