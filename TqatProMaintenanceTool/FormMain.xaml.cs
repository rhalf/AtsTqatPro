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
using System.Windows.Navigation;
using System.Windows.Shapes;

using MahApps.Metro;
using MahApps.Metro.Controls;


using TqatProMaintenanceTool;
using TqatProMaintenanceTool.Properties;
using Controls.UserControls;

using TqatProModel;
using TqatProModel.Database;
using TqatProModel.Parser;
using TqatProViewModel;
using System.Threading;
using System.Data;
using System.Collections.Concurrent;

namespace TqatProMaintenanceTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FormMain : MetroWindow {

        ServerStatus serverStatus = ServerStatus.STOP;
        DateTime dateTimeLastTime;
        ThreadProperties threadProperties = new ThreadProperties();

        ConcurrentQueue<TrackerDatabaseSize> trackerDatabaseSizes;

        Company company;
        User user;
        List<Company> companies;
        Database database;
        DataGrid dataGrid = new DataGrid();

        static int logCounter = 0;

        public FormMain(Company company, User user, List<Company> companies, Database database) {
            this.company = company;
            this.user = user;
            this.companies = companies;
            this.database = database;
            InitializeComponent();
        }



        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
            loadCompany();
            loadDatabases();

        }

        private void loadCompany() {
            TreeView treeView = new TreeView();


            foreach (Company companyItem in companies) {
                TreeViewItem treeViewItem = new TreeViewItem { Name = companyItem.Username, Header = companyItem.Username + " (" + companyItem.Trackers.Count.ToString() + ")", Background = Brushes.AliceBlue };

                if (companyItem.Trackers == null)
                    continue;

                foreach (Tracker tracker in companyItem.Trackers) {
                    TreeViewItem treeViewSubItem = new TreeViewItem { Name = "tracker" + tracker.TrackerImei, Header = tracker.TrackerImei };
                    treeViewItem.Items.Add(treeViewSubItem);
                }

                treeView.Items.Add(treeViewItem);
            }

            TabItem tabPage = new TabItem();

            tabPage.Header = "Companies";
            tabPage.Content = treeView;

            foreach (TabItem tabItem in tabControl.Items) {
                if (tabItem.Header == tabPage.Header) {
                    tabControl.Items.Remove(tabItem);
                    break;
                }
            }

            tabControl.Items.Add(tabPage);
        }
        //    StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };


        //    foreach (Company companyItem in companies) {
        //        Expander expander = new Expander { Name = companyItem.Username, Header = companyItem.Username, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, IsExpanded = true };
        //        StackPanel stackPanelTrackers = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };

        //        expander.Content = companyItem;

        //        if (companyItem.Trackers == null)
        //            continue;

        //        foreach (Tracker tracker in companyItem.Trackers) {
        //            Label label = new Label { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };


        //            label.Content = tracker.TrackerImei.ToString();
        //            stackPanelTrackers.Children.Add(label);

        //        }

        //        expander.Content = stackPanelTrackers;
        //        stackPanel.Children.Add(expander);

        //    }

        //    TabItem tabItem = new TabItem();

        //    tabItem.Header = "Companies";
        //    tabItem.Content = stackPanel;

        //    tabControl.Items.Add(tabItem);

        //}
        private void loadDatabases() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(getDatabaseSizes), null);
        }

        private void getDatabaseSizes(object state) {

            Query query = new Query(database);
            trackerDatabaseSizes = query.getDatabasesSize();

            ConcurrentQueue<TrackerDatabaseSizeItem> trackerDatabaseSizeItems = new ConcurrentQueue<TrackerDatabaseSizeItem>();

            foreach (TrackerDatabaseSize trackerDatabaseSizeItem in trackerDatabaseSizes) {
                trackerDatabaseSizeItems.Enqueue(new TrackerDatabaseSizeItem(trackerDatabaseSizeItem));
            }


            Dispatcher.BeginInvoke(new Action(() => {
                DataTemplate dataTemplate = (DataTemplate)Application.Current.Resources["listViewTrackersDatabaseSize"];
                dataGrid.ItemTemplate = dataTemplate;

                dataGrid.ItemsSource = trackerDatabaseSizeItems;

                TabItem tabPage = new TabItem();
                tabPage.Header = "Databases";
                tabPage.Content = dataGrid;
                tabControl.Items.Add(tabPage);
            }));

        }



        private void run(object state) {
            try {


                while (serverStatus == ServerStatus.RUN) {

                    TimeSpan timeSpan = DateTime.Now.Subtract(dateTimeLastTime);
                    if (timeSpan.Minutes > 3) {
                        MaintenanceServerLog maintenanceServerLogStart = new MaintenanceServerLog();
                        maintenanceServerLogStart.Description = "Starting Maintenance Server.";
                        maintenanceServerLogStart.Status = MaintenanceServerStatus.SUCCESS;
                        log(maintenanceServerLogStart);

                        dateTimeLastTime = DateTime.Now;

                        if (dataGrid == null) {
                            continue;
                        }

                        MaintenanceServerLog maintenanceServerLog = new MaintenanceServerLog();
                        maintenanceServerLog.Status = MaintenanceServerStatus.RUNNING;
                        maintenanceServerLog.Description = "Starting to optimize " + dataGrid.Items.Count.ToString() + " databases.";
                        log(maintenanceServerLog);

                        threadProperties.MaxThreadCount = dataGrid.Items.Count;
                        foreach (TrackerDatabaseSizeItem trackerDatabaseSizeItem in dataGrid.Items) {

                            ThreadPool.QueueUserWorkItem(new WaitCallback(optimizeDatabase), trackerDatabaseSizeItem);

                        }

                       
                    }

                }

                MaintenanceServerLog maintenanceServerStop = new MaintenanceServerLog();
                maintenanceServerStop.Description = "Stopping Maintenance Server.";
                maintenanceServerStop.Status = MaintenanceServerStatus.SUCCESS;
                log(maintenanceServerStop);
            } catch (Exception exception) {
                MaintenanceServerLog maintenanceServerLog = new MaintenanceServerLog();
                maintenanceServerLog.Status = MaintenanceServerStatus.ERROR;
                maintenanceServerLog.Description = exception.Message;
                log(maintenanceServerLog);
            }
        }

        private void optimizeDatabase(object state) {
            TrackerDatabaseSizeItem trackerDatabaseSizeItem = (TrackerDatabaseSizeItem)state;
            try {
                Query query = new Query(database);
                TrackerDatabaseSize trackerDatabaseSize = query.getDatabaseSize(trackerDatabaseSizeItem.Name);
                trackerDatabaseSizeItem.TrackerDatabaseSize = trackerDatabaseSize;
                trackerDatabaseSizeItem.Status = MaintenanceServerStatus.DONE;
                trackerDatabaseSizeItem.DateTimeUpdated = DateTime.Now;


            } catch (Exception exception) {
                MaintenanceServerLog maintenanceServerLog = new MaintenanceServerLog();
                maintenanceServerLog.Status = MaintenanceServerStatus.ERROR;
                maintenanceServerLog.Description = exception.Message;
                log(maintenanceServerLog);


                trackerDatabaseSizeItem.DateTimeUpdated = DateTime.Now;
                trackerDatabaseSizeItem.Status = MaintenanceServerStatus.ERROR;
            } finally {
                threadProperties.CurrentThreadCount++;
                if (threadProperties.CurrentThreadCount == threadProperties.MaxThreadCount) {
                    MaintenanceServerLog maintenanceServerLog = new MaintenanceServerLog();
                    maintenanceServerLog.Status = MaintenanceServerStatus.DONE;
                    maintenanceServerLog.Description = "Optimizing " + threadProperties.CurrentThreadCount + " databases are done.";
                    log(maintenanceServerLog);
                }
                Dispatcher.Invoke(new Action(() => {
                    dataGrid.Items.Refresh();
                }));
            }
        }

        private void log(MaintenanceServerLog maintenanceServerLog) {
            lock (maintenanceServerLog) {
                maintenanceServerLog.Sequence = ++logCounter;
                maintenanceServerLog.DateTime = DateTime.Now;

                Dispatcher.Invoke(new Action(() => {
                    listViewStatus.Items.Add(maintenanceServerLog);
                    listViewStatus.ScrollIntoView(maintenanceServerLog);
                }));
            }
        }





        private void ribbonButtonStart_Click(object sender, RoutedEventArgs e) {
            if (RibbonButtonStart.Label == "Start") {
                RibbonButtonStart.Label = "Stop";
                serverStatus = ServerStatus.RUN;
                dateTimeLastTime = new DateTime();


                ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
                string path = "pack://application:,,/Resources/images/64/must_have/stop.png";
                ImageSource imageSource = (ImageSource)imageSourceConverter.ConvertFromString(path);

                RibbonButtonStart.LargeImageSource = imageSource;

                ThreadPool.QueueUserWorkItem(new WaitCallback(run), null);


            } else {
                RibbonButtonStart.Label = "Start";
                serverStatus = ServerStatus.STOP;

                ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
                string path = "pack://application:,,/Resources/images/64/must_have/play.png";
                ImageSource imageSource = (ImageSource)imageSourceConverter.ConvertFromString(path);

                RibbonButtonStart.LargeImageSource = imageSource;
            }
        }


    }
}
