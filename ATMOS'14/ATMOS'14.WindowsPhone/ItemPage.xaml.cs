using ATMOS_14.Common;
using ATMOS_14.Data;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Calls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace ATMOS_14
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemPage()
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
        /// Gets the view model for this <see cref="Page"/>. This can be changed to a strongly typed view model.
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
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            try
            {
               
                string groupid=(string)e.NavigationParameter;
                SampleDataItem item;
                if(groupid.Contains("Group-0-")|| groupid.Contains("Group-1-"))
                    item = await SampleDataSource.GetItemAsync1((string)e.NavigationParameter);
                else
                   item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
                this.DefaultViewModel["Item"] = item;
                ContentRoot.Title= item.Title;
                details.Text = item.Description;
                rules.Text = item.Content;
                string contact = item.Contacts;
                while (true)
                {
                    if (contact.IndexOf(':') == -1) break;
                    name.Add(contact.Substring(0, contact.IndexOf(':')));
                    contact = contact.Substring(contact.IndexOf(':') + 1);
                    phn.Add(contact.Substring(0, contact.IndexOf('\n')));
                    try { contact = contact.Substring(contact.IndexOf('\n') + 1); }
                    catch (Exception) { break; }
                }
                for (int i = 0; i < name.Count; i++)
                {
                    TextBlock contacts = new TextBlock();
                    contacts.Foreground = new SolidColorBrush(Colors.White);
                    contacts.FontSize = 18.0;
                    contacts.TextWrapping = TextWrapping.Wrap;
                    contacts.Text = name[i] + ": " + phn[i];
                    StackPanel st = new StackPanel();
                    st.Orientation = Orientation.Horizontal;
                    AppBarButton call = new AppBarButton();
                    SymbolIcon ic = new SymbolIcon(Symbol.Phone);
                    call.Icon = ic;
                    call.Tag = i;
                    call.Click += new RoutedEventHandler(Action_Call);
                    AppBarButton msg = new AppBarButton();
                    SymbolIcon ic1 = new SymbolIcon(Symbol.Message);
                    msg.Icon = ic1;
                    msg.Tag = i;
                    msg.Click += new RoutedEventHandler(Action_Message);
                    st.Children.Add(call);
                    st.Children.Add(msg);
                    ContactsList.Children.Add(contacts);
                    ContactsList.Children.Add(st);
                }
            }
            catch (Exception eq) { }
            
        }
        List<string> name = new List<string>();
        List<string> phn = new List<string>();

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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Action_Call(object sender, RoutedEventArgs e)
        {
            AppBarButton ell = sender as AppBarButton;
            PhoneCallManager.ShowPhoneCallUI(phn[Convert.ToInt16(ell.Tag.ToString())], name[Convert.ToInt16(ell.Tag.ToString())]);
        }
        private async void Action_Message(object sender, RoutedEventArgs e)
        {
            AppBarButton ell = sender as AppBarButton;
            string no = phn[Convert.ToInt16(ell.Tag.ToString())], nam = name[Convert.ToInt16(ell.Tag.ToString())];
            
            Windows.ApplicationModel.Chat.ChatMessage msg = new Windows.ApplicationModel.Chat.ChatMessage();
            msg.Body = "Hi " + nam + ", I want to know more about ATMOS.";
            msg.Recipients.Add(no);
            await Windows.ApplicationModel.Chat.ChatMessageManager
                     .ShowComposeSmsMessageAsync(msg);
        }
    }
}