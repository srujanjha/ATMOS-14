using ATMOS_14.Common;
using System;
using Windows.ApplicationModel.Calls;
using Windows.System;
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
    public sealed partial class About : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public About()
        {
            this.InitializeComponent();
            ImageBrush back = FindName("Back") as ImageBrush;
            if (App.themeB) { back.Opacity = 0.5; }
            else { back.Opacity = 1; }
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        private async void Feedback_event(object sender, RoutedEventArgs e)
        {
            try
            {
               String APP_ID = "d7288e9e-32a4-4c44-b0cf-8b66598b2b83";
               
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + APP_ID));
            }
            catch (Exception) { await Launcher.LaunchUriAsync(new Uri("mailto:info@bits-atmos.org")); }
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
        private async void tutorial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/watch?v=3BQOQV1nAlY"));
            }
            catch (Exception) { Launcher.LaunchUriAsync(new Uri("mailto:info@bits-atmos.org")); }
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
            switch(HubPage.abo)
            {
                case 0:
                    Ab.ScrollToSection(FAQ);
                    break;
                case 1:
                    Ab.ScrollToSection(sponsors);
                    break;
                case 2: Ab.ScrollToSection(us);
                    break;
                case 3: Ab.ScrollToSection(author);
                    break;
                case 4: Ab.ScrollToSection(privacy);
                    break;
            }
                
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private void Call(object sender, RoutedEventArgs e)
        {
            AppBarButton ell = sender as AppBarButton;
            int i = Convert.ToInt16(ell.Tag.ToString());
            switch (i)
            {
                case 11: Action_Call("Srujan Jha", "+919010718698"); break;
                case 21: Action_Call("Naveen Jafer", "+919603445607"); break;
                case 12: Action_Message("Srujan Jha", "+919010718698"); break;
                case 22: Action_Message("Naveen Jafer", "+919603445607"); break;
                case 13: Action_Fb("srujanjha.jha"); break;
                case 23: Action_Fb("naveenjafer13"); break;
            }
        }
        private void Action_Call(String Name, String Number)
        {
            PhoneCallManager.ShowPhoneCallUI(Number, Name);
        }
        private async void Action_Fb(String Profile)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.facebook.com/" + Profile));
        }
        private async void Action_Message(String Name, String Number)
        {
            Windows.ApplicationModel.Chat.ChatMessage msg = new Windows.ApplicationModel.Chat.ChatMessage();
            msg.Body = "Hi " + Name + ", I want to know more about ATMOS.";
            msg.Recipients.Add(Number);
            await Windows.ApplicationModel.Chat.ChatMessageManager
                     .ShowComposeSmsMessageAsync(msg);
        }
        
    }
}
