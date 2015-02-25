using ATMOS_14.Common;
using ATMOS_14.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ATMOS_14
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ResultsPage()
        {
            this.InitializeComponent();
            ImageBrush back = FindName("Back") as ImageBrush;
            if (App.themeB) { back.Opacity = 0.5; }
            else { back.Opacity = 1; }
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        public static MobileServiceClient MobileService = new MobileServiceClient("https://atmos.azure-mobile.net/", "GWSiCGrxlNsXBgbnuLBgITrByqYwdJ90");

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

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            read();
            
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
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private IMobileServiceTable<Available> Result = MobileService.GetTable<Available>();  
        private MobileServiceCollection<Available,Available> items;
    
        
        private async void read()
        {

            bool flag = false;
            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ATMOS.db");
                    
                try
                {
                    var allresults = await conn.QueryAsync<Results>("SELECT * FROM Event");
                    listResults.ItemsSource = allresults;
                    Progress.Visibility = Visibility.Collapsed;
                    return;
                }
                catch (Exception) { flag = true; }
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    items = await Result.ToCollectionAsync();
                    if (items[0].ip.Length == 0)
                    {
                        MessageDialog ob = new MessageDialog("Coming Soon!");
                        await ob.ShowAsync();
                        if (this.Frame.CanGoBack)
                        {
                            this.Frame.GoBack();
                        }
                    }
                    else
                    {
                        HttpClient client = new HttpClient();
                        string ip = items[0].ip;
                        string atmosJsonText = await client.GetStringAsync(ip);

                        var resultsData = JsonConvert.DeserializeObject<Results>(atmosJsonText);
                        listResults.ItemsSource = items;
                        await conn.DropTableAsync<Results>();
                        await conn.CreateTableAsync<Results>();
                        await conn.InsertAllAsync(items);
                        Progress.Visibility = Visibility.Collapsed;

                    }
                    
                }
                else if (flag)
                {
                    MessageDialog ob = new MessageDialog("Please connect to internet to download the results.");
                    await ob.ShowAsync();
                    if (this.Frame.CanGoBack)
                    {
                        this.Frame.GoBack();
                    }
                }
                else
                {
                    MessageDialog ob = new MessageDialog("Something went wrong!");
                    await ob.ShowAsync();
                    if (this.Frame.CanGoBack)
                    {
                        this.Frame.GoBack();
                    }
                }
            }
            catch (Exception eq) {
                MessageDialog ob = new MessageDialog("The results of the corresponding events will be displayed after it has been completed.");
                ob.ShowAsync();
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            }
        }
    }
}
