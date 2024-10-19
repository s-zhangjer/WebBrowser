﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.WebRequestMethods;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebBrowser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        String currentUrl;
        bool favoritesBar;
        List<String> favorites = new List<String>();
        List<String> history = new List<String>();
        int historyIndex = -1;
        bool favesFirstCreation = true;

        bool backNavigate = false;
        bool frontNavigate = false;
        bool refreshNavigate = false;
        bool lstHistoryNavigate = false;

        bool addCurrentFav = false;
        bool removeCurrentFav = false;
        bool firstLoadFromHistory = false;
        bool showLstHistory = false;
        bool settingsBar = false;


        int uiIndex = 0;

        String homepageUrl = "";
        
        public MainPage()
        {
            this.InitializeComponent();
            tbUrl.Text = "";
            currentUrl = "";
            favoritesBar = false;
            wvMain.Width = 2560;
            wvMain.Height = 1390;

            btnAddFavorites.Opacity = 0;
            btnRemoveFavorites.Opacity = 0;
            btnSaveFavorites.Opacity = 0;
            btnSaveHistory.Opacity = 0;
            btnClearHistory.Opacity = 0;
            btnClearFavorites.Opacity = 0;
            lstHistory.Opacity = 0;
            btnShowHistory.Opacity = 0;     

            btnChangeBg.Opacity = 0;
            tbBgColor.Opacity = 0;
            btnSetHomepage.Opacity = 0;
            btnSaveSettings.Opacity = 0;
        }


        private void updateTbUrl(String url)
        {
            currentUrl = url;
            tbUrl.Text = url;
        }

        private void btnFavorites_Click(object sender, RoutedEventArgs e)
        {
            onFavoritesClick();
        }

        private void onFavoritesClick()
        {
            int horizontalEdgeUiWidth = 204 + 42 * uiIndex;
            int verticalEdgeUiWidth = 32 + 6 * uiIndex;
            int spacing = 10 + 2 * uiIndex;

            if (favoritesBar)
            {
                favoritesBar = false;
                reframeWvMain();
                
                btnAddFavorites.Opacity = 0;
                btnAddFavorites.IsEnabled = false;
                btnRemoveFavorites.Opacity = 0;
                btnRemoveFavorites.IsEnabled = false;
                addCurrentFav = false;
                removeCurrentFav = false;
                btnSaveFavorites.Opacity = 0;
                btnSaveFavorites.IsEnabled = false;
                btnSaveHistory.Opacity = 0;
                btnSaveHistory.IsEnabled = false;
                btnClearHistory.Opacity = 0;
                btnClearHistory.IsEnabled = false;
                btnClearFavorites.Opacity = 0;
                btnClearFavorites.IsEnabled = false;
                btnShowHistory.Opacity = 0;
                btnShowHistory.IsEnabled = false;

                lstHistory.Opacity = 0;
                lstHistory.IsEnabled = false;
                showLstHistory = false;

                for (int i = 0; i < favorites.Count; i++)
                {
                    Object findBtn = Grid.FindName("favorite" + i);
                    if (findBtn is Button)
                    {
                        Button found = findBtn as Button;
                        found.Opacity = 0;
                        found.IsEnabled = false;
                    }
                }

            }
            else
            {
                favoritesBar = true;
                reframeWvMain();

                btnAddFavorites.Opacity = 100;
                btnAddFavorites.IsEnabled = true;
                btnRemoveFavorites.Opacity = 100;
                btnRemoveFavorites.IsEnabled = true;
                btnAddFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count) * (verticalEdgeUiWidth + spacing)), (204 + 42 * uiIndex) / 2 + 12, 0);
                btnRemoveFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count) * (verticalEdgeUiWidth + spacing)), 10, 0);

                btnSaveFavorites.Opacity = 100;
                btnSaveFavorites.IsEnabled = true;
                btnSaveHistory.Opacity = 100;
                btnSaveHistory.IsEnabled = true;
                btnClearHistory.Opacity = 100;
                btnClearHistory.IsEnabled = true;
                btnClearFavorites.Opacity = 100;
                btnClearFavorites.IsEnabled = true;
                btnShowHistory.Opacity = 100;
                btnShowHistory.IsEnabled = true;

                btnShowHistory.VerticalAlignment = VerticalAlignment.Top;
                btnShowHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 1) * (verticalEdgeUiWidth + spacing)), 10, 0);
                btnClearFavorites.VerticalAlignment = VerticalAlignment.Top;
                btnClearFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 2) * (verticalEdgeUiWidth + spacing)), 10, 0);
                btnClearHistory.VerticalAlignment =VerticalAlignment.Top;
                btnClearHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 3) * (verticalEdgeUiWidth + spacing)), 10, 0);
                btnSaveFavorites.VerticalAlignment = VerticalAlignment.Top;
                btnSaveFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 4) * (verticalEdgeUiWidth + spacing)), 10, 0);
                btnSaveHistory.VerticalAlignment = VerticalAlignment.Top;
                btnSaveHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 5) * (verticalEdgeUiWidth + spacing)), 10, 0);

                lstHistory.VerticalAlignment = VerticalAlignment.Top;
                lstHistory.Margin = new Thickness(0, wvMain.Margin.Top, 240 + 50 * uiIndex + 15, 0);

                if (favesFirstCreation)
                {
                    for (int i = 0; i < favorites.Count; i++)
                    {
                        createFavButton(i);
                    }
                }

                else
                {
                    for (int i = 0; i < favorites.Count; i++)
                    {
                        Object findBtn = Grid.FindName("BtnFavorite" + i);
                        if (findBtn is Button)
                        {
                            Button found = findBtn as Button;
                            found.Opacity = 100;
                            found.IsEnabled = true;
                        }
                    }
                }

            }
        }
        void createFavButton(int i)
        {
            Button favorite = new Button();
            favorite.Name = "favorite" + i;
            favorite.VerticalAlignment = VerticalAlignment.Top;
            favorite.Margin = new Thickness(0, wvMain.Margin.Top + 10 + i * (32 + 6 * uiIndex + 5 + uiIndex), 10, 0);
            favorite.Tag = favorites[i];

            btnAddFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count) * (32 + 6 * uiIndex + 10 + 2 * uiIndex)), (204 + 42 * uiIndex)/2 + 12, 0);
            btnRemoveFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count) * (32 + 6 * uiIndex + 10 + 2 * uiIndex)), 10, 0);
            btnAddFavorites.Width = (204 + 42 * uiIndex) / 2 - 2;
            btnRemoveFavorites.Width = (204 + 42 * uiIndex) / 2 - 2;
            btnAddFavorites.Height = 32 + 6 * uiIndex;
            btnRemoveFavorites.Height = 32 + 6 * uiIndex;

            int horizontalEdgeUiWidth = 204 + 42 * uiIndex;
            int verticalEdgeUiWidth = 32 + 6 * uiIndex;
            int spacing = 10 + 2 * uiIndex;
            btnShowHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 1) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnClearFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 2) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnClearHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 3) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnSaveFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 4) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnSaveHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 5) * (verticalEdgeUiWidth + spacing)), 10, 0);

            if (favorites[i].Contains("www"))
            {
                String shortenedUrl = favorites[i].Substring(favorites[i].IndexOf('w') + 4, favorites[i].Length - (favorites[i].IndexOf('w') + 4));
                shortenedUrl = shortenedUrl.Substring(0, shortenedUrl.Length - (shortenedUrl.Length - shortenedUrl.IndexOf('.')));
                shortenedUrl = shortenedUrl.Substring(0, 1).ToUpper() + shortenedUrl.Substring(1).ToLower();
                favorite.Content = shortenedUrl;
            }
            else
            {
                String shortenedUrl = favorites[i].Substring(favorites[i].IndexOf('/') + 2, favorites[i].Length - (favorites[i].IndexOf('/') + 4));
                shortenedUrl = shortenedUrl.Substring(0, shortenedUrl.Length - (shortenedUrl.Length - shortenedUrl.IndexOf('.')));
                shortenedUrl = shortenedUrl.Substring(0, 1).ToUpper() + shortenedUrl.Substring(1).ToLower();
                favorite.Content = shortenedUrl;
            }

            favorite.Width = 204 + 42 * uiIndex;
            favorite.Height = 32 + 6 * uiIndex;
            favorite.HorizontalAlignment = HorizontalAlignment.Right;
            favorite.Click += favorite_click;
            Grid.Children.Add(favorite);

            reframeWvMain();
        }

        private void favorite_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            Object url = button.Tag;
            wvMain.Source = (new Uri((string)url));

            updateTbUrl((string)url);
        }

        private void btnAddRemoveFavs_Click(object sender, RoutedEventArgs e)
        {
            String url = tbUrl.Text;
            String validUrl = makeValidUrl(url);
            if (addCurrentFav)
            {

                if (!favorites.Contains(validUrl))
                {
                    favorites.Add(makeValidUrl(validUrl));
                    createFavButton(favorites.Count - 1);
                }
            }
            else if (removeCurrentFav)
            {

                if (favorites.Contains(validUrl))
                {
                    removeFavoriteBtn(validUrl);
                }
            }
        }

        public void removeFavoriteBtn(String url)
        {

            for (int i = 0; i < favorites.Count; i++)
            {
                Object findBtn = Grid.FindName("favorite" + i);
                if (findBtn is Button)
                {
                    Button found = findBtn as Button;
                    Grid.Children.Remove(found);
                }
            }

            favorites.Remove(url);

            for (int i = 0; i < favorites.Count; i++)
            {
                createFavButton(i);
            }


            btnAddFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count) * (32 + 6 * uiIndex + 10 + 2 * uiIndex)), (204 + 42 * uiIndex) / 2 + 12, 0);
            btnRemoveFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count) * (32 + 6 * uiIndex + 10 + 2 * uiIndex)), 10, 0);
            btnAddFavorites.Width = (204 + 42 * uiIndex) / 2 - 2;
            btnRemoveFavorites.Width = (204 + 42 * uiIndex) / 2 - 2;
            btnAddFavorites.Height = 32 + 6 * uiIndex;
            btnRemoveFavorites.Height = 32 + 6 * uiIndex;

            int horizontalEdgeUiWidth = 204 + 42 * uiIndex;
            int verticalEdgeUiWidth = 32 + 6 * uiIndex;
            int spacing = 10 + 2 * uiIndex;
            btnShowHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 1) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnClearFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 2) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnClearHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 3) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnSaveFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 4) * (verticalEdgeUiWidth + spacing)), 10, 0);
            btnSaveHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 5) * (verticalEdgeUiWidth + spacing)), 10, 0);
        }

        private void btnAddFavorites_Click(object sender, RoutedEventArgs e)
        {
            /*if (!addCurrentFav)
            {
                tbAddRemoveURL.Opacity = 100;
                tbAddRemoveURL.IsEnabled = true;

                btnAddRemoveFavs.Opacity = 100;
                btnAddRemoveFavs.IsEnabled = true;

                btnCopyCurrentURLFav.Opacity = 100;
                btnCopyCurrentURLFav.IsEnabled = true;

                addCurrentFav = true;
                removeCurrentFav = false;
            }
            else
            {
                tbAddRemoveURL.Opacity = 0;
                tbAddRemoveURL.IsEnabled = false;

                btnAddRemoveFavs.Opacity = 0;
                btnAddRemoveFavs.IsEnabled = false;

                btnCopyCurrentURLFav.Opacity = 0;
                btnCopyCurrentURLFav.IsEnabled = false;

                addCurrentFav = false;
                removeCurrentFav = false;
            }*/
            String url = tbUrl.Text;
            String validUrl = makeValidUrl(url);

            if (!favorites.Contains(validUrl))
            {
                favorites.Add(makeValidUrl(validUrl));
                createFavButton(favorites.Count - 1);
            }
            reframeWvMain();
        }

        private void btnRemoveFavorites_Click(object sender, RoutedEventArgs e)
        {
            /*if (!removeCurrentFav)
            {
                tbAddRemoveURL.Opacity = 100;
                tbAddRemoveURL.IsEnabled = true;

                btnAddRemoveFavs.Opacity = 100;
                btnAddRemoveFavs.IsEnabled = true;

                btnCopyCurrentURLFav.Opacity = 100;
                btnCopyCurrentURLFav.IsEnabled = true;

                removeCurrentFav = true;
                addCurrentFav = false;
            }
            else
            {
                tbAddRemoveURL.Opacity = 0;
                tbAddRemoveURL.IsEnabled = false;

                btnAddRemoveFavs.Opacity = 0;
                btnAddRemoveFavs.IsEnabled = false;

                btnCopyCurrentURLFav.Opacity = 0;
                btnCopyCurrentURLFav.IsEnabled = false;

                removeCurrentFav = false;
                addCurrentFav = false;
            }*/
            String url = tbUrl.Text;
            String validUrl = makeValidUrl(url);

            if (favorites.Contains(validUrl))
            {
                removeFavoriteBtn(validUrl);
            }
            reframeWvMain();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count() > 0 && historyIndex > 0)
            {
                historyIndex--;
                String url = history[historyIndex];
                wvMain.Source = new Uri(url);
                backNavigate = true;
            }
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count() > 0 && historyIndex < history.Count() - 1)
            {
                historyIndex++;
                String url = history[historyIndex];
                wvMain.Source = new Uri(url);
                frontNavigate = true;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            searchFunction();
        }

        private void searchFunction()
        {
            String url = tbUrl.Text;
            String validUrl = makeValidUrl(url);

            if (!url.StartsWith("https://")){
                url = "https://" + url;
            }
            wvMain.Source = (new Uri(validUrl));
            updateTbUrl(validUrl);
        }

        private void tbUrl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                searchFunction();
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            String url = homepageUrl;
            wvMain.Source = (new Uri(url));
            updateTbUrl(url);
        }

        private void btnCopyCurrentURLFav_Click(object sender, RoutedEventArgs e)
        {
        }

        private void wvMain_NavigationCompleted(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            String url = wvMain.Source.ToString();
            tbUrl.Text = url;

            if (!backNavigate && !frontNavigate && !firstLoadFromHistory && !refreshNavigate && !lstHistoryNavigate)
            {
                history.Insert(historyIndex + 1, url);
                historyIndex++;

                updateLstHistory();
            }
            else
            {
                backNavigate = false;
                frontNavigate = false;
                firstLoadFromHistory = false;
                refreshNavigate = false;
                lstHistoryNavigate = false;
            }
        }

        private void updateLstHistory()
        {
            lstHistory.Items.Clear();
            for (int i = 0; i < history.Count; i++)
            {
                lstHistory.Items.Add(history[i]);
            }
        }
        private void wvMain_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSaveData();
            LoadSavedHistoryData();
            LoadSavedSettingsData();
        }

        private void btnSaveFavorites_Click(object sender, RoutedEventArgs e)
        {
            saveFavoritesFile();
        }

        public async Task<String> saveSettingsFile()
        {
            Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile savedHomepageFile =
                    await storageFolder.CreateFileAsync("savedHomepage.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            String data = getSettingsSaveData();

            await Windows.Storage.FileIO.WriteTextAsync(savedHomepageFile, data);

            return data;
        }

        public String getSettingsSaveData()
        {
            String save = "";
            Color bgColor = (Grid.Background as SolidColorBrush).Color;
            int rInt = bgColor.R;
            int gInt = bgColor.G;
            int bInt = bgColor.B;
            save += rInt + "\n";
            save += gInt + "\n";
            save += bInt + "\n";
            save += homepageUrl;

            return save;
        }

        public async Task<String> saveFavoritesFile()
        {
            Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile savedFavoritesFile =
                    await storageFolder.CreateFileAsync("savedFavorites.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            String data = getFavoritesSaveData();

            await Windows.Storage.FileIO.WriteTextAsync(savedFavoritesFile, data);

            return data;
        }

        public String getFavoritesSaveData()
        {
            String favoritesString = "";
            for(int i = 0; i < favorites.Count; i++)
            {
                favoritesString += favorites[i].ToString() + "\n";
            }
            return favoritesString.Substring(0, favoritesString.Length - 1);
        }

        private void btnSaveHistory_Click(object sender, RoutedEventArgs e)
        {
            saveHistoryFile();
        }

        public async Task<String> saveHistoryFile()
        {
            Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile savedHistoryFile =
                    await storageFolder.CreateFileAsync("savedHistory.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            String data = getHistorySaveData();

            await Windows.Storage.FileIO.WriteTextAsync(savedHistoryFile, data);

            return data;
        }

        public String getHistorySaveData()
        {
            String historyString = "";
            for (int i = 0; i < history.Count; i++)
            {
                historyString += history[i].ToString() + "\n";
            }
            return historyString.Substring(0, historyString.Length - 1);
        }

        private async void LoadSaveData()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                        Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile savedFavoritesFile =
                    await storageFolder.GetFileAsync("savedFavorites.txt");

                string favoritesText = await Windows.Storage.FileIO.ReadTextAsync(savedFavoritesFile);

                String[] favoritesString = favoritesText.Split('\n');

                if (!favoritesString.Contains("https://www.nasa.gov/"))
                {
                    favorites.Insert(0, "https://www.nasa.gov/");
                }
                if (!favoritesString.Contains("https://www.amazon.com/"))
                {
                    favorites.Insert(0, "https://www.amazon.com/");
                }
                
                foreach (var line in favoritesString)
                {
                    favorites.Add(line.Trim());
                }
            }
            catch (Exception)
            {
                // This is just in case the file doesn't exist
                // this is important on the FIRST run, and in case something goes wrong!
            }

            if (!favorites.Contains("https://www.nasa.gov/"))
            {
                favorites.Insert(0, "https://www.nasa.gov/");
            }
            if (!favorites.Contains("https://www.amazon.com/"))
            {
                favorites.Insert(0, "https://www.amazon.com/");
            }

            favorites.Remove("");
        }

        private async void LoadSavedHistoryData()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile savedHistoryFile =
                    await storageFolder.GetFileAsync("savedHistory.txt");
                string historyText = await Windows.Storage.FileIO.ReadTextAsync(savedHistoryFile);
                String[] historyString = historyText.Split('\n');

                foreach (var line in historyString)
                {
                    history.Add(line.Trim());
                }
            }
            catch 
            { 
            }
            history.Remove("");

            historyIndex = history.Count - 1;

            if (historyIndex >= 0)
            {
                wvMain.Source = new Uri(history[historyIndex]);
                bool firstLoadFromHistory = true;
            }
        }

        private async void LoadSavedSettingsData()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile savedHomepageFile =
                    await storageFolder.GetFileAsync("savedHomepage.txt");
                string homepageText = await Windows.Storage.FileIO.ReadTextAsync(savedHomepageFile);

                String[] settingsString = homepageText.Split('\n');

                Color bgColor = Color.FromArgb(255, (byte) Int32.Parse(settingsString[0]), (byte)Int32.Parse(settingsString[1]), (byte)Int32.Parse(settingsString[2]));
                Grid.Background = new SolidColorBrush(bgColor);
                lstHistory.Background = new SolidColorBrush(bgColor);

                homepageUrl = (settingsString[3]);
            }
            catch
            {
            }

            if (homepageUrl.Equals(""))
            {
                homepageUrl = "https://www.yahoo.com/";
            }
            
            homepageUrl =  makeValidUrl(homepageUrl);
        }

        private void btnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            history = new List<string>();
            historyIndex = -1;
            lstHistory.Items.Clear();
        }

        private void btnClearFavorites_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < favorites.Count; i++)
            {
                Object findBtn = Grid.FindName("favorite" + i);
                if (findBtn is Button)
                {
                    Button found = findBtn as Button;
                    Grid.Children.Remove(found);
                }
            }
            favorites = new List<string>();

            favorites.Add("https://www.amazon.com/");
            favorites.Add("https://www.nasa.gov/");

            for (int i = 0; i < favorites.Count; i++)
            {
                createFavButton(i);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            wvMain.Source = new Uri(sender.ToString());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            wvMain.Reload();
            refreshNavigate = true;
        }

        private void lstHistory_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lstHistory.SelectedItem != null)
            {
                wvMain.Source = new Uri(lstHistory.SelectedItem.ToString());
                lstHistoryNavigate = true;
            }
        }

        private void btnShowHistory_Click(object sender, RoutedEventArgs e)
        {
            if (showLstHistory)
            {
                lstHistory.Opacity = 0;
                lstHistory.IsEnabled = false;
                showLstHistory = false;
            }
            else
            {
                lstHistory.Opacity = 100;
                lstHistory.IsEnabled = true;
                showLstHistory = true;
            }
        }

        private void wvMain_NavigationStarting(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
        }
        private void tbUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private void btnChangeBg_Click(object sender, RoutedEventArgs e)
        {
            String r = tbBgColor.Text.Substring(1, 2);
            String g = tbBgColor.Text.Substring(3, 2);
            String b = tbBgColor.Text.Substring(5, 2);
            int rInt = 0;
            int gInt = 0;
            int bInt = 0;
            try
            {
                rInt = (int)Convert.ToInt64(r, 16);
                gInt = (int)Convert.ToInt64(g, 16);
                bInt = (int)Convert.ToInt64(b, 16);
            }
            catch 
            {
                rInt = 0;
                gInt = 0;
                bInt = 0;
            }

            Color bgColor = Color.FromArgb(255, (byte) rInt, (byte) gInt, (byte) bInt);
            Grid.Background = new SolidColorBrush(bgColor);
        }
        private void btnSetHomepage_Click(object sender, RoutedEventArgs e)
        {
            homepageUrl = makeValidUrl(tbUrl.Text);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            onSettingsClick();
        }

        private void onSettingsClick()
        {
            if (settingsBar)
            {
                settingsBar = false;
                reframeWvMain();

                btnChangeBg.Opacity = 0;
                btnChangeBg.IsEnabled = false;
                tbBgColor.Opacity = 0;
                tbBgColor.IsEnabled = false;
                btnSetHomepage.Opacity = 0;
                btnSetHomepage.IsEnabled = false;
                btnSaveSettings.Opacity = 0;
                btnSaveSettings.IsEnabled = false;

            }
            else
            {
                settingsBar = true;
                reframeWvMain();

                btnChangeBg.Opacity = 100;
                btnChangeBg.IsEnabled = true;
                tbBgColor.Opacity = 100;
                tbBgColor.IsEnabled = true;
                btnSetHomepage.Opacity = 100;
                btnSetHomepage.IsEnabled = true;
                btnSaveSettings.Opacity = 100;
                btnSaveSettings.IsEnabled = true;
            }
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            saveSettingsFile();
        }

        private string makeValidUrl(String url)
        {
            if (url.Contains("://"))
            {
                url = url.Substring(url.IndexOf("://") + 3, url.Length - url.IndexOf("://") - 3);
            }
            if (url.Contains("w."))
            {
                url = url.Substring(url.IndexOf("w.") + 2, url.Length - url.IndexOf("w.") - 2);
            }

            if(url.Equals(""))
            {
                url += "yahoo";
            }
            if (!url.Contains("."))
            {
                url += ".com/";
            }
            
            if(!url.EndsWith("/"))
            {
                url += "/"; 
            }


            url = "https://www." + url;

            return url;
        }

        private void btnUIBigger_Click(object sender, RoutedEventArgs e)
        {
            if (uiIndex < 5) 
            {
                uiIndex += 1;

                onSettingsClick();
                onSettingsClick();

                onFavoritesClick();
                onFavoritesClick();

                resizeUIFeatures();
            }
        }

        private void btnUISmaller_Click(object sender, RoutedEventArgs e)
        {
            if (uiIndex > 0)
            {
                uiIndex -= 1;
                onSettingsClick();
                onSettingsClick();

                onFavoritesClick();
                onFavoritesClick();

                resizeUIFeatures();
            }
        }

        private void reframeWvMain()
        {
            int wvMainWidth = 2560;
            int wvMainX = 0;
            int wvMainHeight = 1440;
            int wvMainY = 0;

            if(settingsBar)
            {
                wvMainWidth -= 240 + uiIndex * 50;
                wvMainX += 240 + uiIndex * 50;
            }
            if (favoritesBar)
            {
                wvMainWidth -= 240 + uiIndex * 50;
            }

            wvMainY = 50 + 25 * uiIndex;
            wvMainHeight = 1440 - wvMainY;


            wvMain.Width = wvMainWidth;
            wvMain.Height = wvMainHeight;
            wvMain.Margin = new Thickness(wvMainX, wvMainY, 0, 0);
        }

        private void resizeUIFeatures()
        {
            tbUrl.Width = 317 + 158 * uiIndex;
            tbUrl.Height = 32 + 16 * uiIndex;

            btnSearch.Width = 61 + 30 * uiIndex;
            btnSearch.Height = 32 + 16 * uiIndex;
            btnSearch.Margin = new Thickness(tbUrl.Width + btnSearch.Width, 10, 0, 0);

            btnRefresh.Width = 32 + 16 * uiIndex;
            btnRefresh.Height = 32 + 16 * uiIndex;
            btnRefresh.Margin = new Thickness(- tbUrl.Width - btnSearch.Width / 2, 10, 0, 0);

            int horizontalEdgeUiWidth = 204 + 42 * uiIndex;
            int verticalEdgeUiWidth = 32 + 6 * uiIndex;
            int spacing = 10 + 2 * uiIndex;

            tbBgColor.Width = horizontalEdgeUiWidth;
            tbBgColor.Height = verticalEdgeUiWidth;
            tbBgColor.Margin = new Thickness(10, wvMain.Margin.Top + 10, 0, 0);

            btnChangeBg.Width = horizontalEdgeUiWidth;
            btnChangeBg.Height = verticalEdgeUiWidth; 
            btnChangeBg.Margin = new Thickness(10, wvMain.Margin.Top + 10 + verticalEdgeUiWidth, 0, 0);

            btnSetHomepage.Width = horizontalEdgeUiWidth;
            btnSetHomepage.Height = verticalEdgeUiWidth;
            btnSetHomepage.Margin = new Thickness(10, wvMain.Margin.Top + 10 + 2 * verticalEdgeUiWidth + spacing, 0, 0);

            for (int i = 0; i < favorites.Count; i++)
            {
                Object findBtn = Grid.FindName("favorite" + i);
                if (findBtn is Button)
                {
                    Button currentFavBtn = findBtn as Button;
                    currentFavBtn.Width = horizontalEdgeUiWidth;
                    currentFavBtn.Height = verticalEdgeUiWidth;
                    currentFavBtn.Margin = new Thickness(0, wvMain.Margin.Top + 10 + i * (verticalEdgeUiWidth + spacing / 2), 10, 0);

                }
            }

            int endOfFavsY = (int)(wvMain.Margin.Top + 10 + (favorites.Count) * (verticalEdgeUiWidth + spacing / 2));
            btnRemoveFavorites.Width = horizontalEdgeUiWidth / 2 - 2;
            btnRemoveFavorites.Height = verticalEdgeUiWidth;
            btnRemoveFavorites.Margin = new Thickness(0, endOfFavsY + 10, 10, 0);

            btnAddFavorites.Width = horizontalEdgeUiWidth / 2 - 2;
            btnAddFavorites.Height = verticalEdgeUiWidth;
            btnAddFavorites.Margin = new Thickness(0, endOfFavsY + 10, horizontalEdgeUiWidth / 2 + 12, 0);

            btnShowHistory.Width = horizontalEdgeUiWidth;
            btnShowHistory.Height = verticalEdgeUiWidth;
            btnShowHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 1) * (verticalEdgeUiWidth + spacing)), 10, 0);

            btnClearFavorites.Width = horizontalEdgeUiWidth;
            btnClearFavorites.Height = verticalEdgeUiWidth;
            btnClearFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 2) * (verticalEdgeUiWidth + spacing)), 10, 0);

            btnClearHistory.Width = horizontalEdgeUiWidth;
            btnClearHistory.Height = verticalEdgeUiWidth;
            btnClearHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 3) * (verticalEdgeUiWidth + spacing)), 10, 0);

            btnSaveFavorites.Width = horizontalEdgeUiWidth;
            btnSaveFavorites.Height = verticalEdgeUiWidth;
            btnSaveFavorites.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 4) * (verticalEdgeUiWidth + spacing)), 10, 0);

            btnSaveHistory.Width = horizontalEdgeUiWidth;
            btnSaveHistory.Height = verticalEdgeUiWidth;
            btnSaveHistory.Margin = new Thickness(0, (int)(wvMain.Margin.Top + 10 + (favorites.Count + 5) * (verticalEdgeUiWidth + spacing)), 10, 0);

        }
    }
}