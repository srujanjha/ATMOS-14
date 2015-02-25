using ATMOS_14.Common;
using ATMOS_14.Data;
using ATMOS_14.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using NotificationsExtensions.TileContent;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Calls;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace ATMOS_14
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        //  private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public HubPage()
        {
            this.InitializeComponent();
            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            ImageBrush back = FindName("Back") as ImageBrush;
            //ImageBrush back1 = FindName("pBack") as ImageBrush;
            if (App.themeB) { back.Opacity = 0.5; } //pBack.Opacity = 0.5; }
            else { back.Opacity = 1; } //pBack.Opacity = 1; }
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

       public static List<SampleDataItem> list = new List<SampleDataItem>();
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            try
            {
                
                
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
            if (list.Count == 0)
            {
                for (int i = 0; i < 18; i++)
                {
                    foreach (var item in sampleDataGroups.ElementAt(i).Items)
                    {
                        list.Add(item);
                    }
                }
            }
            int gh = list.Count;
            var sampleDataGroups1 = await SampleDataSource.GetGroupsAsync1();
            HubSection1.DataContext = sampleDataGroups1.ElementAt(0);
            HubSection2.DataContext = sampleDataGroups1.ElementAt(1);
            }
            catch (Exception eq) { }
        }
        public static int abo = 0;
        private void UpdateTile()
        {
            Random rnd = new Random();
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            for (int i = 0; i < 6; i++)
            {
                ITileSquare310x310Image tileContent = TileContentFactory.CreateTileSquare310x310Image();
                tileContent.AddImageQuery = true;
                tileContent.Image.Src = "Assets/" + i+ ".png";
                tileContent.Image.Alt = "Web image";

                ITileWide310x150ImageAndText01 wide310x150Content = TileContentFactory.CreateTileWide310x150ImageAndText01();
                 wide310x150Content.Image.Src = "Assets/" + i + ".png";
                wide310x150Content.Image.Alt = "Web image";

                ITileSquare150x150Image square150x150Content = TileContentFactory.CreateTileSquare150x150Image();
                square150x150Content.Image.Src = "Assets/" + i + ".png";
                square150x150Content.Image.Alt = "Web image";

                wide310x150Content.Square150x150Content = square150x150Content;
                tileContent.Wide310x150Content = wide310x150Content;

                TileNotification tileNotification = tileContent.CreateNotification();
                
                string tag = "Image"+i;
                tileNotification.Tag = tag;
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            /*ITileSquare310x310Text09 square310x310TileContent = TileContentFactory.CreateTileSquare310x310Text09();
            square310x310TileContent.TextHeadingWrap.Text = "ATMOS '14";
            ITileWide310x150Text03 wide310x150TileContent = TileContentFactory.CreateTileWide310x150Text03();
            wide310x150TileContent.TextHeadingWrap.Text = "ATMOS '14";
            ITileSquare150x150Text04 square150x150TileContent = TileContentFactory.CreateTileSquare150x150Text04();
           square150x150TileContent.TextBodyWrap.Text = "ATMOS '14";
            wide310x150TileContent.Square150x150Content = square150x150TileContent;
            square310x310TileContent.Wide310x150Content = wide310x150TileContent;
            tileNotification = square310x310TileContent.CreateNotification();
            tag = "Title";
            tileNotification.Tag = tag;
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);*/
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Details about the click event.</param>
        

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception("Navigation Error");
            }
        }

        private async void Call(object sender, RoutedEventArgs e)
        {
            AppBarButton ell = sender as AppBarButton;
            int i = Convert.ToInt16(ell.Tag.ToString());
            switch(i)
            {
                case 1: Action_Fb("bits.atmos"); break;
                case 2: await Launcher.LaunchUriAsync(new Uri("https://twitter.com/BITSAtmos")); break;
                case 3: await Launcher.LaunchUriAsync(new Uri("mailto:info@bits-atmos.org")); break;
                case 4: await Launcher.LaunchUriAsync(new Uri("http://www.bits-atmos.org")); break;
                case 11: Action_Call("Nikhil Potluri (President)", "+919542554003"); break;
                case 21: Action_Call("Vishnu Saran (General Secretary)","+9199848294152"); break;
                case 31: Action_Call("Suhrudh Reddy (Publicity)","+918498964792"); break;
                case 41: Action_Call("Srikar (Sponsorship)","+919573190492"); break;
                case 51: Action_Call("Mudit Jain (Technical Convener)","+918498085518"); break;
                case 12: Action_Message("Nikhil Potluri (President)", "+919542554003"); break;
                case 22: Action_Message("Vishnu Saran (General Secretary)", "+9199848294152"); break;
                case 32: Action_Message("Suhrudh Reddy (Publicity)", "+918498964792"); break;
                case 42: Action_Message("Srikar (Sponsorship)", "+919573190492"); break;
                case 52: Action_Message("Mudit Jain (Technical Convener)", "+918498085518"); break;
                case 13: Action_Fb("nikhilpotluri"); break;
                case 23: Action_Fb("vishnusaranwriting"); break;
                case 33: Action_Fb("suhrudh.reddy.39"); break;
                case 43: Action_Fb("srikar.mokkapati"); break;
                case 53: Action_Fb("Mudit1729"); break;
                  
            }
        }
        private void Action_Call(String Name, String Number)
        {
            PhoneCallManager.ShowPhoneCallUI(Number, Name);
        }
        private async void Action_Fb(String Profile)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.facebook.com/"+Profile));
        }
        private async void Action_Message(String Name, String Number)
        {
    Windows.ApplicationModel.Chat.ChatMessage msg = new Windows.ApplicationModel.Chat.ChatMessage();
            msg.Body = "Hi " + Name+ ", I want to know more about ATMOS.";
            msg.Recipients.Add(Number);
            await Windows.ApplicationModel.Chat.ChatMessageManager
                     .ShowComposeSmsMessageAsync(msg);
        }
        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

           
            if (e.NavigationMode == NavigationMode.New)
            {
                UpdateTile();

            //   var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ATMOS_Cortana.xml"));
           //     await Windows.Media.SpeechRecognition.VoiceCommandManager.InstallCommandSetsFromStorageFileAsync(storageFile);
            }
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private async void drive(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-drive-to:?destination.latitude=17.544794&destination.longitude=78.571051&destination.name=BITS-Pilani Hyderabad Campus"));
        }
        private async void walk(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-walk-to:?destination.latitude=17.544794&destination.longitude=78.571051&destination.name=BITS-Pilani Hyderabad Campus"));
        }
        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Signup));
            }
        }
        private void about_Click(object sender, RoutedEventArgs e)
        {
            abo = 0;
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(About));
            }
        }
        private void timeline_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(TimeLine),"Whats up");
            }
        }
        private void results_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ResultsPage));
            }
        }
        private void map_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MapPage));
            } 
            //await Launcher.LaunchUriAsync(new Uri("explore-maps://v1.0/?latlon=17.5430626,78.5713704&zoom=12"));//bingmaps:?rtp=~adr.BITS Pilani Hyderabad Campus"));
        }

        public static String APP_ID = "";
        public static MobileServiceClient MobileService = new MobileServiceClient("http://atmos.azure-mobile.net/", "GWSiCGrxlNsXBgbnuLBgITrByqYwdJ90");
        private IMobileServiceTable<Available> Result = MobileService.GetTable<Available>();
        private MobileServiceCollection<Available, Available> items;

        private async void feedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           //     Available ob = new Available() { eventName = "", ip = "" };
           //     await Result.InsertAsync(ob);
           //     Available ob1 = new Available() { eventName = "Feedback", ip = "d7288e9e-32a4-4c44-b0cf-8b66598b2b83" };
           //     await Result.InsertAsync(ob1);
               // items = await Result.ToCollectionAsync();
                APP_ID = "d7288e9e-32a4-4c44-b0cf-8b66598b2b83";
                if (APP_ID.Equals(""))
                    await Launcher.LaunchUriAsync(new Uri("mailto:info@bits-atmos.org"));
                else
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + APP_ID));
            }
            catch (Exception) { Launcher.LaunchUriAsync(new Uri("mailto:info@bits-atmos.org")); }
        }
        
        private void contact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hub.ScrollToSection(contact);
            }
            catch (Exception) {  }
        }

        private async void StackPanel_Tapped(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("bingmaps:?rtp=~adr.17.544794,78.571051"));
        }

        
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lt = sender as ListView;
            int gh = lt.SelectedIndex;
            lt.SelectedIndex = -1;
            switch (gh)
            {
                case 0:
                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(Signup));
                    }
                    break;
                case 1:
                    if (this.Frame != null)
                    {
                        abo = 1;
                        this.Frame.Navigate(typeof(TimeLine));
                    }
                    break;
                case 2:
                    if (this.Frame != null)
                    {
                        abo = 1;
                        this.Frame.Navigate(typeof(About));
                    }
                    break;
                case 3:
                    if (this.Frame != null)
                    {
                        abo = 0;
                        this.Frame.Navigate(typeof(About));
                    }
                    break;
                case 4:
                    if (this.Frame != null)
                    {
                        abo = 2;
                        this.Frame.Navigate(typeof(About));
                    }
                    break;
            }
        }

        private void ListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception("Navigation Error");
            }
        }
        bool flag = false;
        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ListView lt = sender as ListView;
            if (flag)
            {
                var itemId = ((SampleDataItem)lt.Items.ElementAt(lt.SelectedIndex)).UniqueId;
               // 
                if (!Frame.Navigate(typeof(ItemPage), itemId))
                {
                    throw new Exception("Navigation Error");
                }

            }
            flag = true;
           // lt.SelectedIndex = -1;
        }


        
    }
}