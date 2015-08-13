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


namespace TqatProMaintenanceTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FormMain : MetroWindow {

        ServerStatus serverStatus = ServerStatus.STOP;
        DateTime dateTimeLastTime;


        Company company;
        User user;
        List<Company> companies;
        Database database;
        DataGrid dataGrid = new DataGrid();

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
                TreeViewItem treeViewItem = new TreeViewItem { Name = companyItem.Username, Header = companyItem.Username + " (" + companyItem.Trackers.Count.ToString()+")", Background =  Brushes.AliceBlue};

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
            List<TrackerDatabaseSize> trackerDatabaseSizes = query.getDatabasesSize();
            List<TrackerDatabaseSizeItem> trackerDatabaseSizeItems = new List<TrackerDatabaseSizeItem>();

            foreach (TrackerDatabaseSize trackerDatabaseSizeItem in trackerDatabaseSizes) {
                trackerDatabaseSizeItems.Add(new TrackerDatabaseSizeItem(trackerDatabaseSizeItem));
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
            while (serverStatus == ServerStatus.RUN) {

                TimeSpan timeSpan = DateTime.Now.Subtract(dateTimeLastTime);
                if (timeSpan.Minutes > 0) {
                    MaintenanceServerLog maintenanceServerLogStart = new MaintenanceServerLog();
                    maintenanceServerLogStart.Description = "Starting Maintenance Server.";
                    maintenanceServerLogStart.Status = MaintenanceServerStatus.SUCCESS;
                    log(maintenanceServerLogStart);

                    dateTimeLastTime = DateTime.Now;
                }

            }

            MaintenanceServerLog maintenanceServerStop = new MaintenanceServerLog();
            maintenanceServerStop.Description = "Stopping Maintenance Server.";
            maintenanceServerStop.Status = MaintenanceServerStatus.SUCCESS;
            log(maintenanceServerStop);
        }

        private void log(MaintenanceServerLog maintenanceServerLog) {
            Dispatcher.BeginInvoke(new Action(() => {
                listViewStatus.Items.Add(maintenanceServerLog);
            }));
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
