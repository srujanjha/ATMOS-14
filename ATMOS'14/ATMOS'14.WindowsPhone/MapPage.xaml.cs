using ATMOS_14.Common;
using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ATMOS_14
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MapPage()
        {
            this.InitializeComponent();

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
            Maps.Center = new Geopoint(new BasicGeoposition()
            {
                Latitude = 17.544852,
                Longitude = 78.571962
            });
            Maps.ZoomLevel = 16;
            AddMapIcon();
            this.navigationHelper.OnNavigatedTo(e);
        }
        private void AddMapIcon()
        {
            double[] lat = { 17.544721, 17.544358, 17.543773, 17.544870, 17.545363, 17.545683, 17.545593, 17.545486, 17.547293, 17.545011, 17.544073, 17.542195, 17.5420439, 17.542645, 17.544670, 17.545008, 17.541961, 17.542075, };
            double[] lon = { 78.571001, 78.571760, 78.571835, 78.571838, 78.572243, 78.572796, 78.571460, 78.570577, 78.572513, 78.571629, 78.573705, 78.576017, 78.5761823, 78.574083, 78.575135, 78.570811, 78.575718, 78.575701 };
            String[] place = { "F-Block(LTC)", "D-Block", "E-Block", "C-Block", "B-Block", "A-Block", "G-Block", "Auditorium", "Main Gate", "Stage-1", "Stage-2", "ATM", "Connaught Place", "Dining Hall-1", "Dining Hall-2", "Cafeteria", "Restaurant", "Bakery" };
            MapIcon[] ic = new MapIcon[lat.Length];
            for (int i = 0; i < lat.Length; i++)
            {
                MapIcon MapIcon1 = new MapIcon();
                MapIcon1.Location = new Geopoint(new BasicGeoposition() { Latitude = lat[i], Longitude = lon[i] });
                MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                MapIcon1.Title = place[i];
                Maps.MapElements.Add(MapIcon1);
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private async void map_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("bingmaps:?rtp=~adr.17.544794,78.571051"));
        }
        private async void drive(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-drive-to:?destination.latitude=17.544794&amp;destination.longitude=78.571051&amp;destination.name=BITS-Pilani Hyderabad Campus"));
        }
        private async void walk(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-walk-to:?destination.latitude=17.544794&amp;destination.longitude=78.571051&amp;destination.name=BITS-Pilani Hyderabad Campus"));
        }
       
    }
}
