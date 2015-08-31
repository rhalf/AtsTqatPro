using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Data;
using System.Threading;
using System.Diagnostics;
using System.IO;
using MahApps.Metro.Controls;


using TqatProModel;
using TqatProModel.Database;
using TqatProViewModel;
using TqatProTrackingTool;


using TqatProTrackingTool.Control;

using System.Windows.Controls.Ribbon;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;


namespace TqatProTrackingTool {
    /// <summary>
    /// Interaction logic for FormMain.xaml
    /// </summary>
    public partial class FormMain : MetroWindow {

        Company company;
        User user;
        UserItem userClicked;
        Database database;
        Server server;

        public ConcurrentQueue<TrackerData> trackerDatas = new ConcurrentQueue<TrackerData>();
        //Variable Items
        Map map = new Map();

        public ThreadProperties threadProperties = new ThreadProperties();

        public List<UserItem> userItems;


        #region Threads

        Thread threadTrackerManagerPointer, threadGeofenceManagerPointer, threadPoiManagerPointer;


        private void threadTrackerUpdateManager() {

            int maxWorkerThreads = 1000;
            int maxCompletionPortThreads = 1000;
            int minWorkerThreads = 100;
            int minCompletionPortThreads = 100;
            ThreadPool.SetMaxThreads(maxWorkerThreads, maxCompletionPortThreads);
            ThreadPool.SetMinThreads(minWorkerThreads, minCompletionPortThreads);

            //threadProperties.MaxThreadCount = 1000;
            foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                ThreadPool.QueueUserWorkItem(new WaitCallback(threadFunctionTrackerUpdate), trackerItem);
                Dispatcher.BeginInvoke(new Action(() => {
                    threadCount.Content = threadProperties.CurrentThreadCount.ToString();
                }));
            }

            while (true) {
                //======================================================
                try {
                    foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(threadFunctionTrackerUpdate), trackerItem);
                        Dispatcher.BeginInvoke(new Action(() => {
                            threadCount.Content = threadProperties.CurrentThreadCount.ToString();
                        }));

                        if (listViewTrackers.Items.Count == listViewTrackersData.Items.Count)
                            Thread.Sleep(100);
                        else
                            Thread.Sleep(50);

                    }
                    if (listViewTrackers.Items.Count == listViewTrackersData.Items.Count)
                        Thread.Sleep(5000);
                    else
                        Thread.Sleep(1000);

                } catch (Exception exception) {
                    Debug.Print(exception.Message);
                }
                //======================================================
            }
        }
        private void threadFunctionGeofenceUpdate(object obj) {
            Dispatcher.BeginInvoke(new Action(() => {
                while (!webBrowserMap.IsLoaded) {
                    ;
                }

                MapCommand mapCommand = new MapCommand();
                mapCommand.Name = "ClearGeofence";
                mapCommand.Value = "";

                map.processCommand(webBrowserMap, mapCommand);
            }));

            if (company.Geofences == null)
                return;

            foreach (Geofence geofence in company.Geofences) {
                Dispatcher.BeginInvoke(new Action(() => {
                    while (!webBrowserMap.IsLoaded) {
                        ;
                    }

                    map.loadGeofence(webBrowserMap, geofence);
                }));
            }
        }
        private void threadFunctionPoiUpdate(object obj) {
            User selectedUser = (User)obj;

            Dispatcher.BeginInvoke(new Action(() => {
                while (!webBrowserMap.IsLoaded) {
                    ;
                }

                MapCommand mapCommand = new MapCommand();
                mapCommand.Name = "ClearPoi";
                mapCommand.Value = "";
                map.processCommand(webBrowserMap, mapCommand);
            }));

            if (selectedUser.Pois == null)
                return;

            foreach (Poi poi in selectedUser.Pois) {
                Dispatcher.BeginInvoke(new Action(() => {
                    while (!webBrowserMap.IsLoaded) {
                        ;
                    }
                    map.loadPoi(webBrowserMap, poi);
                }));
            }

        }
        private void threadFunctionTrackerUpdate(object state) {
            lock (threadProperties) {
                threadProperties.CurrentThreadCount++;
            }
            TrackerItem trackerItem = (TrackerItem)state;
            Query query = new Query(database);
            TrackerData trackerData;

            if (trackerItem.IsChecked) {
                trackerData = query.getTrackerLatestData(company, trackerItem.getTracker(), server);
            } else {
                trackerData = new TrackerData();
                trackerData.Tracker = trackerItem.getTracker();
                trackerData.isDataEmpty = true;

                Dispatcher.BeginInvoke(new Action(() => {
                    for (int index = 0; listViewTrackersData.Items.Count > index; index++) {
                        TrackerData trackerDataItem = (TrackerData)listViewTrackersData.Items[index];
                        if (trackerDataItem.Tracker.TrackerImei == trackerData.Tracker.TrackerImei) {
                            listViewTrackersData.Items.RemoveAt(index);
                        }
                    }
                }));
            }

            Dispatcher.BeginInvoke(new Action(() => {
                if (!webBrowserMap.IsLoaded) {
                    lock (threadProperties) {
                        threadProperties.CurrentThreadCount--;
                    }
                    return;
                }

                //lock (map) {
                try {
                    map.loadTracker(webBrowserMap, trackerItem, trackerData, (string)ribbonGalleryComboBoxDisplayMember.SelectedValue);
                } catch (Exception exception) {
                    Log.write(exception);
                }
                //}

                if (trackerItem.IsChecked) {
                    bool isExisting = false;

                    for (int index = 0; listViewTrackersData.Items.Count > index; index++) {
                        TrackerData trackerDataItem = (TrackerData)listViewTrackersData.Items[index];
                        if (trackerDataItem.Tracker.Id == trackerData.Tracker.Id) {
                            listViewTrackersData.Items.RemoveAt(index);
                            listViewTrackersData.Items.Insert(index, trackerData);
                            isExisting = true;
                            break;
                        }
                    }

                    if (isExisting == false) {
                        listViewTrackersData.Items.Add(trackerData);
                    }
                }
            }));

            lock (threadProperties) {
                threadProperties.CurrentThreadCount--;
            }
        }

        #endregion
        #region Initialized
        private void loadMap() {
            Uri uri = new Uri(Directory.GetCurrentDirectory() + "\\" + "Web" + "\\" + "Maps" + "\\" + "index.html");
            webBrowserMap.Navigate(uri);
            webBrowserMap.LoadCompleted += webBrowserMap_LoadCompleted;
        }

        void webBrowserMap_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e) {
            threadTrackerManagerPointer = new Thread(threadTrackerUpdateManager);
            threadTrackerManagerPointer.Start();
            threadGeofenceManagerPointer = new Thread(threadFunctionGeofenceUpdate);
            threadGeofenceManagerPointer.Start();
        }

        public FormMain(Company company, User user, Database database) {
            InitializeComponent();
            this.user = user;
            this.database = database;
            this.company = company;

            server = new Server();
            server.Name = "67.205.85.177";
            server.Ip = "67.205.85.177";
            server.PortCommand = 8001;
            server.PortHttp = 8000;

            loadMap();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {

            userItems = new List<UserItem>();
            foreach (User user in company.Users) {
                UserItem userItem = new UserItem(user);
                userItems.Add(userItem);
            }

            ribbonMenuButtonUser.ItemsSource = userItems;
            labelCompany.Content = this.company.DisplayName;
            labelUser.Content = this.user.Username;
            labelDatabaseHost.Content = this.database.Host;
        }
        #endregion

        #region RibbonMenuButton
        private void StyleRibbonMenuButtonUser_Click(object sender, RoutedEventArgs e) {
            RibbonMenuItem ribbonMenuItemSender = (RibbonMenuItem)sender;

            userClicked = (UserItem)ribbonMenuItemSender.DataContext;

            comboBoxCollection.Items.Clear();
            listViewTrackersData.ItemsSource = null;

            MapCommand mapCommand = new MapCommand();
            mapCommand.Name = "ClearTracker";
            map.processCommand(webBrowserMap, mapCommand);


            foreach (Collection collection in userClicked.getUser().Collections) {
                comboBoxCollection.Items.Add(collection);
            }
            comboBoxCollection.DisplayMemberPath = "Name";
        }

        private void RibbonComboBoxDisplayMember_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            RibbonGallery ribbonGallery = (RibbonGallery)sender;
            RibbonGalleryItem ribbonGalleryItem = (RibbonGalleryItem)ribbonGallery.SelectedItem;

            DataTemplate dataTemplate = (DataTemplate)Application.Current.Resources["listView" + ribbonGalleryItem.Content.ToString()];
            listViewTrackers.ItemTemplate = dataTemplate;

            GridView gridView = (GridView)listViewTrackersData.View;

            for (int index = 0; gridView.Columns.Count > index; index++) {
                if (gridView.Columns[index].Header.ToString() == "Label") {
                    Binding binding = new Binding("Tracker." + ribbonGalleryItem.Content.ToString());

                    gridView.Columns[index].DisplayMemberBinding = binding;
                }
            }
        }

        private void ribbonButtonTrackers_Click(object sender, RoutedEventArgs e) {
            if (gridTrackers.Width == 200) {
                gridTrackers.Width = 0;
            } else {
                gridTrackers.Width = 200;
            }
        }

        #endregion


        private void formCommandTrackers_Click(object sender, RoutedEventArgs e) {


        }

        private void ribbonButtonTrackersData_Click(object sender, RoutedEventArgs e) {
            if (gridTrackersData.Height > 0) {
                gridTrackersData.Height = 0;
            } else {
                gridTrackersData.Height = 300;
            }

        }



        private void listViewTrackersData_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListView listViewTrackersData = (ListView)sender;
            TrackerData trackerData = (TrackerData)listViewTrackersData.SelectedItem;

            try {

                if (trackerData == null)
                    return;
                if (trackerData.isDataEmpty)
                    return;

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("[{\"Latitude\":\"");
                stringBuilder.Append(trackerData.Coordinate.latitude.ToString());
                stringBuilder.Append("\",\"Longitude\":\"");
                stringBuilder.Append(trackerData.Coordinate.longitude.ToString());
                stringBuilder.Append("\"}]");
                //stringBuilder.Append("[{\"TrackerId\":\"");
                //stringBuilder.Append(trackerData.Tracker.Id.ToString());
                //stringBuilder.Append("\"}]");

                if (webBrowserMap.IsLoaded) {
                    MapCommand mapCommand = new MapCommand();
                    mapCommand.Name = "SetFocus";
                    mapCommand.Value = stringBuilder.ToString();

                    map.processCommand(webBrowserMap, mapCommand);
                }
            } catch {
            }
        }

        private void listViewTrackers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int checkedItems = 0;
            foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                if (trackerItem.IsChecked == true)
                    checkedItems++;
            }

            if (listViewTrackersData == null)
                return;

            TrackerItem selectedTrackerItem = (TrackerItem)(sender as ListView).SelectedItem;


            foreach (TrackerData trackerData in listViewTrackersData.Items) {
                if (selectedTrackerItem == null)
                    break;

                if (trackerData.Tracker.Id == selectedTrackerItem.getTracker().Id) {
                    listViewTrackersData.SelectedItem = (trackerData);
                    listViewTrackersData.ScrollIntoView(trackerData);

                    break;
                }
            }



        }


        private void MenuItemlistViewTrackers_uncheckAll_Click(object sender, RoutedEventArgs e) {
            //MenuItem menuItem = (MenuItem)sender;
            //ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            //ListView listView = (ListView)contextMenu.PlacementTarget;

            foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                trackerItem.IsChecked = false;
            }
            listViewTrackers.Items.Refresh();
        }

        private void MenuItemlistViewTrackers_checkAll_Click(object sender, RoutedEventArgs e) {
            foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                trackerItem.IsChecked = true;
            }
            listViewTrackers.Items.Refresh();

        }



        private void RibbonApplicationMenuItemExit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }



        private void RibbonApplicationMenuItemLogout_Click(object sender, RoutedEventArgs e) {
            DialogLogin dialogLogin = new DialogLogin();
            dialogLogin.Show();
            this.Close();
        }



        private void RibbonApplicationMenuItemAts_Click(object sender, RoutedEventArgs e) {
            Process.Start("http://www.ats-qatar.com/");
        }



        private void findLabelInListViewTrackerItem(string value) {
            if (listViewTrackers == null)
                return;


            TrackerItem trackerItemFound = null;

            foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                switch (ribbonGalleryComboBoxDisplayMember.SelectedValue.ToString()) {
                    case "VehicleRegistration":

                        if (trackerItem.VehicleRegistration.IndexOf(value, 0) == 0) {
                            trackerItemFound = trackerItem;
                        }
                        break;
                    case "VehicleModel":
                        if (trackerItem.VehicleModel.IndexOf(value, 0) == 0) {
                            trackerItemFound = trackerItem;
                        }
                        break;
                    case "OwnerName":
                        if (trackerItem.OwnerName.IndexOf(value, 0) == 0) {
                            trackerItemFound = trackerItem;
                        }
                        break;
                    case "DriverName":
                        if (trackerItem.DriverName.IndexOf(value, 0) == 0) {
                            trackerItemFound = trackerItem;
                        }
                        break;
                    case "TrackerImei":
                        if (trackerItem.TrackerImei.IndexOf(value, 0) == 0) {
                            trackerItemFound = trackerItem;
                        }
                        break;
                    case "SimNumber":
                        if (trackerItem.SimNumber.IndexOf(value, 0) == 0) {
                            trackerItemFound = trackerItem;
                        }
                        break;
                }

                if (trackerItemFound != null) {
                    listViewTrackers.SelectedItem = (trackerItemFound);
                    listViewTrackers.ScrollIntoView(trackerItemFound);
                    return;
                }
            }
        }

        private void textBoxSearchTrackerData_KeyUp(object sender, KeyEventArgs e) {
            findLabelInListViewTrackerItem(textBoxSearchTrackerData.Text);
        }

        private void comboBoxCollection_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            Collection collectionSelected = (Collection)comboBoxCollection.SelectedItem;

            threadPoiManagerPointer = new Thread(threadFunctionPoiUpdate);
            threadPoiManagerPointer.Start(userClicked.getUser());

            List<TrackerItem> trackersUser = new List<TrackerItem>();
            List<TrackerItem> trackersUserCollection = new List<TrackerItem>();

            foreach (Tracker tracker in company.Trackers) {
                foreach (User userTracker in tracker.Users) {
                    if (userClicked.getUser().Id == userTracker.Id) {
                        trackersUser.Add(new TrackerItem(tracker));
                    }
                }
            }

            foreach (TrackerItem trackerItem in trackersUser) {
                foreach (Collection collectionTracker in trackerItem.getTracker().Collections) {
                    if (collectionSelected == null)
                        break;

                    if (collectionTracker.Id == collectionSelected.Id) {
                        var trackerItemSelected = trackerItem;
                        trackersUserCollection.Add(trackerItemSelected);
                    }
                }
            }

            listViewTrackers.ItemsSource = null;
            listViewTrackers.ItemsSource = trackersUserCollection;
            DataTemplate dataTemplate = (DataTemplate)Application.Current.Resources["listViewVehicleRegistration"];
            listViewTrackers.ItemTemplate = dataTemplate;
            listViewTrackers.Items.Refresh();
        }




    }

}
