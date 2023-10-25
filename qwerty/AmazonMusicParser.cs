using Avalonia.Controls;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using qwerty.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace qwerty
{
    public class AmazonMusicParser
    {
        List<AlbumViewModel> albums = new List<AlbumViewModel>();
        public List<AlbumViewModel> ParseAmazonAlbums()
        {
            string url = "http://music.amazon.com";
            

            using (IWebDriver driver = new ChromeDriver())
            {
                // Открыть веб-страницу
                driver.Navigate().GoToUrl(url);

                Thread.Sleep(5000);
                try
                {
                    var acceptCookiesButton = driver.FindElement(By.Id("dialogButton1"));
                    acceptCookiesButton.Click();
                }
                catch (NoSuchElementException)
                {
                    // Если кнопка не найдена, можно просто проигнорировать
                }

                var albumElements = driver.FindElements(By.XPath("//music-vertical-item"));

                


                foreach (var albumElement in albumElements)
                {
                    AlbumViewModel temp = new AlbumViewModel { Name = albumElement.GetAttribute("primary-text"), Href = "http://music.amazon.com"+albumElement.GetAttribute("primary-href") };
                    albums.Add(temp);    

                    
                }
            }

            Debug.WriteLine(albums.Count);

            foreach (var album in albums)
            {
                Debug.WriteLine("Album Name: " + album.Name);
                Debug.WriteLine("Album Href: " + album.Href);
            }

            return albums;
        }

        public List<SongViewModel> ParseSongs(string albumUrl)
        {
            var songs = new List<SongViewModel>();

            using (IWebDriver driver = new ChromeDriver())
            {
                // Открыть веб-страницу
                driver.Navigate().GoToUrl(albumUrl);

                Thread.Sleep(5000);
                int i = 1;
                try
                {
                    var acceptCookiesButton = driver.FindElement(By.Id("dialogButton1"));
                    acceptCookiesButton.Click();
                }
                catch (NoSuchElementException)
                {
                    // Если кнопка не найдена, можно просто проигнорировать
                }



                var albumnameelem = driver.FindElement(By.XPath("//*[@id=\"atf\"]/music-detail-header"));
                string albumname = albumnameelem.GetAttribute("headline");
                
                var albumElements = driver.FindElements(By.XPath("//music-text-row"));
                
                if (albumElements.Count <= 0)
                {
                    albumElements = driver.FindElements(By.XPath("//music-image-row"));
                    Debug.WriteLine("FFFFFWDQFF");
                    Debug.WriteLine(albumnameelem.Text);
                    foreach (var elem in albumElements)
                    {
                        
                        string primaryText = elem.GetAttribute("primary-text");
                        string duration = elem.FindElement(By.CssSelector("div.col4")).FindElement(By.TagName("span")).Text;
                        if (!string.IsNullOrEmpty(primaryText))
                        {
                            SongViewModel temp = new SongViewModel { SongName = primaryText, AlbumName =albumname, Duration=duration };
                            songs.Add(temp);
                            i++;
                        }

                       


                    }
                }
                else
                {



                    foreach (var elem in albumElements)
                    {
                        
                        string duration = elem.FindElement(By.CssSelector("div.col4")).FindElement(By.TagName("span")).Text;
                        SongViewModel temp = new SongViewModel { SongName = elem.GetAttribute("primary-text").ToString(), AlbumName= albumname, Duration=duration,};
                        songs.Add(temp);
                        i++;


                    }
                }








                foreach (var song in songs)
                {
                    Debug.WriteLine(song.Duration);
                }
            }




            return songs;
        }



      

    }
}