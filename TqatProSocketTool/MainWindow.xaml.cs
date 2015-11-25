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
using TqatProSocketTool.ViewModel;
using AtsGps;
using AtsGps.Meitrack;
using System.Collections.Concurrent;
using System.Threading;
using System.Net;
using TqatProModel.Database;
using TqatProSocketTool.Properties;
using System.Diagnostics;
using TqatProModel;
using System.Collections.ObjectModel;

namespace TqatProSocketTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window {
        public MainWindow () {
            InitializeComponent();
        }

        ObservableCollection<TcpManager> tcpManagers;

        #region BufferStart
        Thread threadUploaderManager;
        private void threadUploaderManagerFunc (Object state) {
            while (true) {
                ObservableCollection<TcpManager> tcpManagers = (ObservableCollection<TcpManager>)state;
                if (tcpManagers.Count <= 0)
                    continue;


                for (Int32 index = 0; index < tcpManagers.Count; index++) {
                    TcpManager tcpManager = (TcpManager)tcpManagers[index];
                    if (tcpManager == null)
                        continue;

                    lock (tcpManager) {
                        tcpManager.Refresh();
                        if (tcpManager.BufferIn == null)
                            continue;
                        if (tcpManager.BufferIn.IsEmpty)
                            continue;

                        try {
                            Database database = new Database {
                                IpAddress = Settings.Default.DatabaseIp,
                                Port = Int32.Parse(Settings.Default.DatabasePort),
                                DatabaseName = Settings.Default.DatabaseName,
                                Username = Settings.Default.DatabaseUsername,
                                Password = Settings.Default.DatabasePassword
                            };

                            Query query = new Query(database);
                            query.checkConnection();
                        } catch {
                            continue;
                        }

                        if (tcpManager.Manufacturer.Name == "Meitrack") {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(threadMeitrackInPacketFunc), tcpManager.BufferIn);
                        }
                    }
                }
                //Thread.Sleep(3000);
            }
        }
        private void threadMeitrackInPacketFunc (Object state) {
            ConcurrentQueue<Object> queue = (ConcurrentQueue<Object>)state;
            Object obj;

            if (queue == null)
                return;
            if (queue.IsEmpty)
                return;

            try {
                Database database = new Database {
                    IpAddress = Settings.Default.DatabaseIp,
                    Port = Int32.Parse(Settings.Default.DatabasePort),
                    DatabaseName = Settings.Default.DatabaseName,
                    Username = Settings.Default.DatabaseUsername,
                    Password = Settings.Default.DatabasePassword
                };
                Query query = new Query(database);

                queue.TryDequeue(out obj);

                Byte[] data = (Byte[])obj;
                Gm gm = new Gm(data);

                Tracker tracker = query.getTracker(gm.Unit);
                query.insertTrackerData(tracker, gm);

            } catch (Exception exception) {
                Dispatcher.BeginInvoke(new Action(() => {
                    var log = new AtsGps.Log(exception.Message, AtsGps.LogType.CLIENT);
                    dataGridLog.Items.Add(log);
                }));
            }
        }
        #endregion BufferEnd

        private void Window_Loaded (object sender, RoutedEventArgs e) {
            this.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            Machine machine = new Machine();

            groupMachine.DataContext = machine;

            Database database = new Database {
                IpAddress = Settings.Default.DatabaseIp,
                Port = Int32.Parse(Settings.Default.DatabasePort),
                DatabaseName = Settings.Default.DatabaseName,
                Username = Settings.Default.DatabaseUsername,
                Password = Settings.Default.DatabasePassword
            };

            if (database != null) {
                mysqlCredential.DataContext = database;
            }

            initializedTcpManagers();

            threadUploaderManager = new Thread(threadUploaderManagerFunc);
            threadUploaderManager.Start(tcpManagers);
        }

        private void initializedTcpManagers () {
            tcpManagers = new ObservableCollection<TcpManager>();
            MeitrackTcpManager meitractkcpManagerMvt100 = new MeitrackTcpManager();
            meitractkcpManagerMvt100.Manufacturer = new Manufacturer { Name = "Meitrack", Device = "Mvt100" };
            meitractkcpManagerMvt100.Port = 8887;
            listViewTcpManagersSetup.Items.Add(meitractkcpManagerMvt100);

            MeitrackTcpManager meitractTcpManagerT1 = new MeitrackTcpManager();
            meitractTcpManagerT1.Manufacturer = new Manufacturer { Name = "Meitrack", Device = "T1" };
            meitractTcpManagerT1.Port = 4000;
            listViewTcpManagersSetup.Items.Add(meitractTcpManagerT1);

            listViewTcpManagers.ItemsSource = tcpManagers;
        }

        private void ButtonServersStart_Click (object sender, RoutedEventArgs e) {
            try {

                foreach (MeitrackTcpManager meitrackTcpManager in listViewTcpManagersSetup.Items.OfType<MeitrackTcpManager>()) {
                    if (meitrackTcpManager.IsEnabled == false)
                        continue;
                    //Mvt100
                    if (meitrackTcpManager.Manufacturer.Device == "Mvt100" && meitrackTcpManager.Manufacturer.Name == "Meitrack") {
                        tcpManagers.Add(meitrackTcpManager);
                    }
                    //T1
                    if (meitrackTcpManager.Manufacturer.Device == "T1" && meitrackTcpManager.Manufacturer.Name == "Meitrack") {
                        tcpManagers.Add(meitrackTcpManager);
                    }
                }


                foreach (TcpManager tcpManager in tcpManagers) {
                    if (tcpManager.Manufacturer.Device == "Mvt100" && tcpManager.Manufacturer.Name == "Meitrack") {
                        tcpManager.Event += MeitracKMvt100_Event;
                        tcpManager.DataReceived += MeitrackMvt100_DataReceived;
                        tcpManager.Start();
                    }
                    if (tcpManager.Manufacturer.Device == "T1" && tcpManager.Manufacturer.Name == "Meitrack") {
                        tcpManager.Event += MeitrackT1_Event;
                        tcpManager.DataReceived += MeitrackT1_DataReceived;
                        tcpManager.Start();
                    }
                }

                groupTcpManagersSetup.IsEnabled = false;
                buttonServersStart.IsEnabled = false;
                buttonServersStop.IsEnabled = true;

            } catch (Exception exception) {
                groupTcpManagersSetup.IsEnabled = true;
                foreach (TcpManager tcpManager1 in tcpManagers) {
                    try {
                        tcpManager1.Stop();
                    } catch {
                        continue;
                    }
                }
                tcpManagers.Clear();
                GC.Collect();
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonServersStop_Click (object sender, RoutedEventArgs e) {
            groupTcpManagersSetup.IsEnabled = true;
            foreach (TcpManager tcpManager in tcpManagers) {
                tcpManager.Stop();
            }
            tcpManagers.Clear();
            GC.Collect();

            buttonServersStart.IsEnabled = true;
            buttonServersStop.IsEnabled = false;
        }

        private void MeitrackT1_DataReceived (Object sender, byte[] data) {
            if (data == null)
                return;
        }

        private void MeitrackT1_Event (Object sender, AtsGps.Log serverLog) {
            TcpManager tcpManager = (TcpManager)sender;
            serverLog.Description = tcpManager.Manufacturer.ToString() + " : " + serverLog.Description;
            Dispatcher.BeginInvoke(new Action(() => {
                dataGridLog.Items.Add(serverLog);
            }));
        }

        private void MeitrackMvt100_DataReceived (Object sender, byte[] data) {
            MeitrackTcpManager meitrackTcpManager = (MeitrackTcpManager)sender;

            if (data == null)
                return;

            try {
                Gm gm = new Gm(data);
                meitrackTcpManager.BufferIn.Enqueue(data);
            } catch (Exception exception) {
                AtsGps.Log log = new AtsGps.Log(exception.Message, AtsGps.LogType.ERROR);
                Dispatcher.BeginInvoke(new Action(() => {
                    dataGridLog.Items.Add(log);
                }));
            } 
        }

        private void MeitracKMvt100_Event (Object sender, AtsGps.Log serverLog) {
            TcpManager tcpManager = (TcpManager)sender;
            serverLog.Description = tcpManager.Manufacturer.ToString() + " : " + serverLog.Description;
            Dispatcher.BeginInvoke(new Action(() => {
                dataGridLog.Items.Add(serverLog);
            }));
        }

        private void buttonMysqlTest_Click (object sender, RoutedEventArgs e) {
            Database database = (Database)mysqlCredential.DataContext;

            Settings.Default.DatabaseIp = database.IpAddress;
            Settings.Default.DatabasePort = database.Port.ToString();
            Settings.Default.DatabaseName = database.DatabaseName;
            Settings.Default.DatabaseUsername = database.Username;
            Settings.Default.DatabasePassword = database.Password;

            Settings.Default.Save();

            Thread threadMysqlTest = new Thread(threadMysqlTestFunc);
            threadMysqlTest.Start();
        }

        private void threadMysqlTestFunc () {
            try {
                Database database = new Database {
                    IpAddress = Settings.Default.DatabaseIp,
                    Port = Int32.Parse(Settings.Default.DatabasePort),
                    DatabaseName = Settings.Default.DatabaseName,
                    Username = Settings.Default.DatabaseUsername,
                    Password = Settings.Default.DatabasePassword
                };

                Query query = new Query(database);
                query.checkConnection();
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show("Successful...", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
            } catch (Exception exception) {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(exception.Message, "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
            if (tcpManagers.Count > 0) {
                foreach (TcpManager tcpManager in tcpManagers) {
                    tcpManager.Stop();
                }
                tcpManagers.Clear();
            }
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void dataGridLog_SelectionChanged (object sender, SelectionChangedEventArgs e) {

        }
    }
}
