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

using TqatProTrackingTool.Control;

using System.Windows.Controls.Ribbon;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;


namespace TqatProTrackingTool {
    /// <summary>
    /// Interaction logic for FormMain.xaml
    /// </summary>
    public partial class FormMain : MetroWindow {

        public Company company;
        public User user;
        public Database database;

        public List<User> users;
        public List<Tracker> trackers;
        public ConcurrentQueue<TrackerData> trackerDatas = new ConcurrentQueue<TrackerData>();
        //Variable Items
        Map map = new Map();


        public List<UserItem> userItems;

        Thread threadTrackerUpdateManagerPointer;

        private void threadTrackerUpdateManager() {
            while (true) {
                try {
                    foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                        if (trackerItem.IsChecked == true) {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(updateTrackerData), trackerItem);
                        }
                    }
                    Thread.Sleep(3000);
                } catch (Exception exception) {
                }
            }
        }

        private void updateTrackerData(object state) {
            TrackerItem trackerItem = (TrackerItem)state;
            Query query = new Query(database);

            //trackerDatas.Enqueue(query.getTrackerLatestData(company, trackerItem.getTracker()));
            TrackerData trackerData = query.getTrackerLatestData(company, trackerItem.getTracker());
            if (trackerData != null) {
                Dispatcher.BeginInvoke(new Action(() => {
                    Map map = new Map();
                    map.loadTracker(webBrowserMap, trackerItem.getTracker(), trackerData);
                }));
            }
        }


        //private void threadFuncUpdateMap() {
        //    Dispatcher.BeginInvoke(new Action(() => {
        //        webBrowserMap.Refresh();
        //    }));

        //    for (int count = 0; count < 1500; count++) {
        //        Thread.Sleep(1);
        //    }
        //}

        private void loadMap() {
            Uri uri = new Uri(Directory.GetCurrentDirectory() + "\\" + "Web" + "\\" + "Maps" + "\\" + "index.html");
            webBrowserMap.Navigate(uri);
        }

        public FormMain(Company company, User user, List<User> users, List<Tracker> trackers, Database database) {
            InitializeComponent();
            this.user = user;
            this.users = users;
            this.database = database;
            this.company = company;
            this.trackers = trackers;

            loadMap();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {

            userItems = new List<UserItem>();
            foreach (User user in users) {
                UserItem userItem = new UserItem(user);
                userItems.Add(userItem);
            }

            RibbonMenuButtonUser.ItemsSource = userItems;

            threadTrackerUpdateManagerPointer = new Thread(threadTrackerUpdateManager);
            threadTrackerUpdateManagerPointer.Start();

        }


        private void formCommandTrackers_Click(object sender, RoutedEventArgs e) {


        }

        //private void comboBoxUser_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        //    ComboBox comboBox = (ComboBox)sender;
        //    User userClicked = (User)comboBox.SelectedItem;
        //    List<TrackerItem> trackerItem = new List<TrackerItem>();
        //    foreach (User selectedUser in users) {
        //        if (userClicked.Id == selectedUser.Id) {
        //            foreach (Tracker trackerSelected in trackers) {
        //                foreach (User trackerUser in trackerSelected.Users) {
        //                    if (trackerUser.Username == selectedUser.Username) {
        //                        trackerItem.Add(new TrackerItem(trackerSelected));
        //                    }
        //                }
        //            }
        //            break;
        //        }
        //    }
        //    listViewTrackers.ItemsSource = trackerItem;



        //}

        private void comboBoxDisplayMember_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            ComboBox comboBox = (ComboBox)sender;
            Label labelSelected = (Label)comboBox.SelectedItem;

            //listViewTrackers.ItemTemplate = (DataTemplate)Resources["listView" + labelSelected.Content.ToString()];
        }

        //private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e) {
        //    if (threadMapUpdater == null)
        //        return;

        //    if (!threadMapUpdater.IsAlive) {
        //        threadMapUpdater = new Thread(threadFuncUpdateMap);
        //        threadMapUpdater.Start();
        //    }
        //}


        private void StyleRibbonMenuButtonUser_Click(object sender, RoutedEventArgs e) {
            RibbonMenuItem ribbonMenuItemSender = (RibbonMenuItem)sender;

            UserItem userClicked = (UserItem)ribbonMenuItemSender.DataContext;

            List<TrackerItem> trackerItem = new List<TrackerItem>();
            foreach (User selectedUser in users) {
                if (userClicked.Id == selectedUser.Id) {
                    if (selectedUser.Pois != null) {
                        foreach (Poi poi in selectedUser.Pois) {
                            map.loadPoi(webBrowserMap, poi);
                        }
                    }

                    foreach (Tracker trackerSelected in trackers) {
                        foreach (User trackerUser in trackerSelected.Users) {
                            if (trackerUser.Username == selectedUser.Username) {
                                trackerItem.Add(new TrackerItem(trackerSelected));
                            }
                        }
                    }
                    break;
                }
            }
            listViewTrackers.ItemsSource = trackerItem;
            DataTemplate dataTemplate = (DataTemplate)Application.Current.Resources["listViewVehicleRegistration"];
            listViewTrackers.ItemTemplate = dataTemplate;

            //User userInstance = userClicked.getUser();

        }

        private void RibbonGalleryComboBoxTrackers_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            RibbonGallery ribbonGallery = (RibbonGallery)sender;
        }

        private void RibbonComboBoxDisplayMember_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            RibbonGallery ribbonGallery = (RibbonGallery)sender;
            RibbonGalleryItem ribbonGalleryItem = (RibbonGalleryItem)ribbonGallery.SelectedItem;

            DataTemplate dataTemplate = (DataTemplate)Application.Current.Resources["listView" + ribbonGalleryItem.Content];
            listViewTrackers.ItemTemplate = dataTemplate;
        }

        private void ribbonButtonTrackers_Click(object sender, RoutedEventArgs e) {
            if (gridTrackers.Width == 200) {
                gridTrackers.Width = 0;
            } else {
                gridTrackers.Width = 200;
            }
        }








    }

}
