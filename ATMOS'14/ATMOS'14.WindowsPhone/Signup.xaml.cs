using ATMOS_14.Common;
using ATMOS_14.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using System;
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
    public sealed partial class Signup : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Signup()
        {
            this.InitializeComponent();
            ImageBrush back = FindName("Back") as ImageBrush;
            if (App.themeB) { back.Opacity = 0.5; }
            else { back.Opacity = 1; }
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
            Events1.ItemsSource = HubPage.list;
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pivot.SelectedIndex = 1;
        }
        public static MobileServiceClient MobileService = new MobileServiceClient("http://atmos.azure-mobile.net/", "GWSiCGrxlNsXBgbnuLBgITrByqYwdJ90");
        private IMobileServiceTable<Register> Result = MobileService.GetTable<Register>();

        private async void register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Name.Text.Equals(""))
                {
                    MessageDialog ob1 = new MessageDialog("You need to enter your name to register.");
                    ob1.ShowAsync(); return;
                }
                else if (College.Text.Equals(""))
                {
                    MessageDialog ob1 = new MessageDialog("You need to enter the name of your college/school/university to register.");
                    ob1.ShowAsync(); return;
                }
                else if (Phone.Text.Equals(""))
                {
                    MessageDialog ob1 = new MessageDialog("You need to enter your phone-number to register.");
                    ob1.ShowAsync(); return;
                }
                else if (Email.Text.Equals(""))
                {
                    MessageDialog ob1 = new MessageDialog("You need to enter your email-address to register.");
                    ob1.ShowAsync(); return;
                }
                else if (Events1.SelectedItems.Count==0)
                {
                    MessageDialog ob1 = new MessageDialog("You need to select atleast one event, you are interested in.");
                    ob1.ShowAsync(); return;
                }


                String ev = "";
                foreach (var i in Events1.SelectedItems)
                {
                    ev += (i.ToString() + ",");
                }
                Register item = new Register { name = Name.Text, college = College.Text, phone = Phone.Text, email = Email.Text, ambassador = (bool)Ambas.IsChecked, accomodation = (bool)Acco.IsChecked, events = ev };
                await Result.InsertAsync(item);
                MessageDialog ob = new MessageDialog("You have successfully registered for the events in ATMOS'14. We await your presence.");
                ob.ShowAsync();
                this.Frame.GoBack();
            }
            catch (Exception eq)
            {
                MessageDialog ob = new MessageDialog("We encountered some problem. Kindly connect to internet.");
                ob.ShowAsync();
            }
        }
  
    }
}
