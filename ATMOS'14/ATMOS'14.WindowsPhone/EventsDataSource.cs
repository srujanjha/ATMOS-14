using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Xaml.Data;
using System.Net.NetworkInformation;
using SQLite;
using Windows.UI.Popups;

namespace ATMOS_14
{
    
    public class EventsGroupByDate
    {
        public EventsGroupByDate(String uniqueId, String date)
        {
            this.UniqueId = uniqueId;
            this.Date = date;
            this.Items = new ObservableCollection<Events>();
        }

        public string UniqueId { get; private set; }
        public string Date { get; private set; }
        public ObservableCollection<Events> Items { get; private set; }

        public override string ToString()
        {
            return this.Date;
        }
    }
    public class EventsGroupByHour
    {
        public EventsGroupByHour(String uniqueId, String time)
        {
            this.UniqueId = uniqueId;
            this.Time = time;
            this.Items = new ObservableCollection<Events>();
        }
        public EventsGroupByHour()
        {
            this.UniqueId = "0";
            this.Time = "00:00";
            this.Items = new ObservableCollection<Events>();
        }

        public string UniqueId { get;  set; }
        public string Time { get;  set; }
        public ObservableCollection<Events> Items { get; set; }

        public override string ToString()
        {
            return this.Time;
        }
    }
    public sealed class EventsDataSource
    {
        private static EventsDataSource _sampleDataSource;// = new EventsDataSource();

        private ObservableCollection<EventsGroupByDate> _groupsd = new ObservableCollection<EventsGroupByDate>();
        public ObservableCollection<EventsGroupByDate> Groupsd
        {
            get { return this._groupsd; }
        }
        public static async Task<IEnumerable<EventsGroupByDate>> GetGroupsAsyncd()
        {
            _sampleDataSource = new EventsDataSource();
            await _sampleDataSource.GetSampleDataAsync();

            return _sampleDataSource.Groupsd;
        }
        public static async Task<EventsGroupByDate> GetGroupAsyncd(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groupsd.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        
        private ObservableCollection<EventsGroupByDate> _groupsw = new ObservableCollection<EventsGroupByDate>();
        public ObservableCollection<EventsGroupByDate> Groupsw
        {
            get { return this._groupsw; }
        }
        public static async Task<IEnumerable<EventsGroupByDate>> GetGroupsAsyncw()
        {
            _sampleDataSource = new EventsDataSource();
            await _sampleDataSource.GetSampleDataAsync();

            return _sampleDataSource.Groupsw;
        }
        public static async Task<EventsGroupByDate> GetGroupAsyncw(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groupsw.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        
        private ObservableCollection<EventsGroupByHour> _groupsh = new ObservableCollection<EventsGroupByHour>();
        public ObservableCollection<EventsGroupByHour> Groupsh
        {
            get { return this._groupsh; }
        }
        public static async Task<IEnumerable<EventsGroupByHour>> GetGroupsAsynch()
        {
            _sampleDataSource = new EventsDataSource();
            
            await _sampleDataSource.GetSampleDataAsync();
            return _sampleDataSource.Groupsh;
        }
        public static async Task<EventsGroupByHour> GetGroupAsynch(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groupsh.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<Events> GetItemAsyncd(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groupsd.SelectMany(group => group.Items).Where((item) => item.Id.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        public static async Task<Events> GetItemAsyncw(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groupsw.SelectMany(group => group.Items).Where((item) => item.Id.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        public static async Task<Events> GetItemAsynch(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groupsh.SelectMany(group => group.Items).Where((item) => item.Id.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        public static MobileServiceClient MobileService = new MobileServiceClient("http://atmos.azure-mobile.net/", "GWSiCGrxlNsXBgbnuLBgITrByqYwdJ90");
        private IMobileServiceTable<Events> Result = MobileService.GetTable<Events>();
        //private MobileServiceCollection<Events, Events> items;
        private List<Events> list = new List<Events>();
        private async Task GetSampleDataAsync()
        {
            bool flag = false;
            try
            {

                SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ATMOS.db");

                var allresults = await conn.QueryAsync<Events>("SELECT * FROM Schedule");
                if (list.Count == 0) list = allresults;

                if (this._groupsd.Count != 0)
                    return;


                list.Sort((ev1, ev2) => DateTime.Compare(ev1.startTime, ev2.startTime));
                EventsGroupByDate group1 = new EventsGroupByDate("1", "10th October");
                EventsGroupByDate group2 = new EventsGroupByDate("2", "11th October");
                EventsGroupByDate group3 = new EventsGroupByDate("3", "12th October");
                EventsGroupByDate group4 = new EventsGroupByDate("1", "Current Events");
                EventsGroupByDate group5 = new EventsGroupByDate("2", "Future Events");

                int uid = 1;
                foreach (Events i in list)
                {
                    if (TimeLine.stb ==1)
                    {
                        string d = i.startTime.Date.ToString("dd/MM/yyyy");
                        if (d.Equals("12/10/2014"))
                        {
                            group3.Items.Add(i);
                        }
                        else if (d.Equals("11/10/2014"))
                        {
                            group2.Items.Add(i);
                        }
                        else
                        {
                            group1.Items.Add(i);
                        }
                    }
                    else if (TimeLine.stb == 3)
                    {
                        if (DateTime.Compare(i.startTime, DateTime.Now) > 0)
                        {
                            group5.Items.Add(i);
                        }
                        else if (DateTime.Compare(i.endTime, DateTime.Now) > 0 && DateTime.Compare(i.startTime, DateTime.Now) <= 0)
                        {
                            group4.Items.Add(i);
                        }
                    }
                    else
                    {
                        if (this._groupsh.Count == 0)
                        {
                            EventsGroupByHour group = new EventsGroupByHour();
                            group.UniqueId = "1";
                            group.Time = i.startTime.ToString("hh:mm tt dd/MM/yyyy");
                            group.Items.Add(i);
                            this.Groupsh.Add(group);
                        }
                        else if (this.Groupsh[_groupsh.Count - 1].Time.Equals(i.startTime.ToString("hh:mm tt dd/MM/yyyy")))
                        {
                            EventsGroupByHour group = this._groupsh[_groupsh.Count - 1];
                            group.Items.Add(i);
                        }
                        else
                        {
                            uid++;
                            EventsGroupByHour group = new EventsGroupByHour();
                            group.UniqueId = uid.ToString();
                            group.Time = i.startTime.ToString("hh:mm tt dd/MM/yyyy");
                            group.Items.Add(i);
                            this.Groupsh.Add(group);
                        }
                    }
                }
                int k = this._groupsh.Count;
                if (TimeLine.stb == 1)
                { this.Groupsd.Add(group1); this.Groupsd.Add(group2); this.Groupsd.Add(group3); }
                else if (TimeLine.stb == 3) { this.Groupsw.Add(group4); this.Groupsw.Add(group5); 
                }
            }
            catch (Exception) { flag = true; }
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                   // await GetSampleDataFromAzure();
                }
                else if(flag)
                {
                    MessageDialog ob = new MessageDialog("Please connect to internet to download the schedule.");
                    await ob.ShowAsync();
                }
        }
        private async Task GetSampleDataFromAzure()
        {
            try{
                var items = await Result.Take(88).ToListAsync();
              //  var items = await Result.ToCollectionAsync();
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ATMOS.db");
                list.Clear();
                foreach (Events i in items)
                { 
                    list.Add(i); 
                }
                await conn.DropTableAsync<Events>();
                await conn.CreateTableAsync<Events>();
                list.Sort((ev1, ev2) => DateTime.Compare(ev1.startTime, ev2.startTime));
                await conn.InsertAllAsync(list);
                EventsGroupByDate group1 = new EventsGroupByDate("1", "10th October");
                EventsGroupByDate group2 = new EventsGroupByDate("2", "11th October");
                EventsGroupByDate group3 = new EventsGroupByDate("3", "12th October");
                EventsGroupByDate group4 = new EventsGroupByDate("1", "Current Events");
                EventsGroupByDate group5 = new EventsGroupByDate("2", "Future Events");
                this.Groupsh.Clear();
                int uid = 1;
                foreach (Events i in list)
                {
                    if (TimeLine.stb == 1)
                    {
                        string d = i.startTime.Date.ToString("dd/MM/yyyy");
                        if (d.Equals("12/10/2014"))
                        {
                            group3.Items.Add(i);
                        }
                        else if (d.Equals("11/10/2014"))
                        {
                            group2.Items.Add(i);
                        }
                        else
                        {
                            group1.Items.Add(i);
                        }
                    }
                    else if (TimeLine.stb == 3)
                    {
                        if (DateTime.Compare(i.startTime, DateTime.Now) > 0)
                        {
                            group5.Items.Add(i);
                        }
                        else if (DateTime.Compare(i.endTime, DateTime.Now) > 0 && DateTime.Compare(i.startTime, DateTime.Now) <= 0)
                        {
                            group4.Items.Add(i);
                        }
                    }
                    else
                    {
                        if (this._groupsh.Count == 0)
                        {
                            EventsGroupByHour group = new EventsGroupByHour();
                            group.UniqueId = "1";
                            group.Time = i.startTime.ToString("hh:mm tt dd/MM/yyyy");
                            group.Items.Add(i);
                            this.Groupsh.Add(group);
                        }
                        else if (this.Groupsh[_groupsh.Count - 1].Time.Equals(i.startTime.ToString("hh:mm tt dd/MM/yyyy")))
                        {
                            EventsGroupByHour group = this._groupsh[_groupsh.Count - 1];
                            group.Items.Add(i);
                        }
                        else
                        {
                            uid++;
                            EventsGroupByHour group = new EventsGroupByHour();
                            group.UniqueId = uid.ToString();
                            group.Time = i.startTime.ToString("hh:mm tt dd/MM/yyyy");
                            group.Items.Add(i);
                            
                            this.Groupsh.Add(group);
                        }
                    }
                }
                if (TimeLine.stb == 1) { this.Groupsd.Clear();
                    this.Groupsd.Add(group1); this.Groupsd.Add(group2); this.Groupsd.Add(group3); }
                else if (TimeLine.stb == 3)
                { this.Groupsw.Clear(); this.Groupsw.Add(group4); this.Groupsw.Add(group5); 
                }
            }
            catch (Exception e1)
            {
                MessageDialog ob = new MessageDialog("Please connect to internet to download the schedule.");
                ob.ShowAsync();
            }
        }
   
    }
}
