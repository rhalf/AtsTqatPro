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
using System.Collections.Specialized;

namespace TqatProSocketTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window {
        public MainWindow () {
            InitializeComponent();
        }

        ObservableCollection<TcpManager> tcpManagers;
        ConcurrentDictionary<String, String[]> bufferOut;
        Machine machine;

        #region BufferStart
        Thread threadUploaderManager;
        Boolean debug = true;

        private void threadUploaderManagerFunc (Object state) {
            while (true) {
                //for (int interval = 0; interval < 5; interval++) {
                for (int count = 0; count < tcpManagers.Count; count++) {
                    tcpManagers[count].Refresh();
                }
                //    Thread.Sleep(1000);
                //}


                if (tcpManagers.Count <= 0)
                    continue;

                for (Int32 index = 0; index < tcpManagers.Count; index++) {
                    TcpManager tcpManager = tcpManagers[index];
                    if (tcpManager == null)
                        continue;
                    if (tcpManager.BufferIn == null)
                        continue;
                    if (tcpManager.BufferIn.IsEmpty)
                        continue;
                    if (tcpManager.Device.Company == "Meitrack") {
                        processBufferIn(tcpManager.BufferIn);
                    }
                }

            }
        }

        private void processBufferIn (Object state) {
            ConcurrentQueue<Object> queue = (ConcurrentQueue<Object>)state;
            Object obj = null;
            Gm gm = null;

            while (queue.IsEmpty == false) {
                if (!queue.TryDequeue(out obj)) {
                    return;
                }

                try {
                    gm = (Gm)obj;
                    if (gm == null) {
                        return;
                    }
                } catch (Exception exception) {
                    if (debug) {
                        TextLog.Write(exception);
                    }
                    return;
                }

                try {
                    Database database = new Database {
                        IpAddress = Settings.Default.DatabaseIp,
                        Port = Int32.Parse(Settings.Default.DatabasePort),
                        DatabaseName = Settings.Default.DatabaseName,
                        Username = Settings.Default.DatabaseUsername,
                        Password = Settings.Default.DatabasePassword
                    };

                    Query query = new Query(database);
                    Tracker tracker = query.getTracker(gm.Unit);
                    query.insertTrackerData(tracker, gm);
                } catch (Exception exception) {
                    lock (queue) {
                        queue.Enqueue(obj);
                    }
                    if (debug) {
                        TextLog.Write(exception);
                    }
                }
            }
        }

        #endregion BufferEnd

        private void Window_Loaded (object sender, RoutedEventArgs e) {
            this.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            TextLog.Name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            machine = new Machine();

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

            bufferOut = new ConcurrentDictionary<String, String[]>();

        }
        private void initializedTcpManagers () {
            tcpManagers = new ObservableCollection<TcpManager>();

            StringCollection servers = Settings.Default.Servers;

            foreach (string server in servers) {
                string[] attributes = server.Split(',');
                string[] device = attributes[0].Split('|');

                if (device[0] == "Meitrack") {
                    if (device[1] == "Mvt100") {
                        MeitrackTcpManager meitrackMvt100 = new MeitrackTcpManager();
                        meitrackMvt100.Device = new Device { Company = "Meitrack", Name = "Mvt100" };
                        meitrackMvt100.IpAddress = attributes[1];
                        meitrackMvt100.Port = Int32.Parse(attributes[2]);
                        meitrackMvt100.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(meitrackMvt100);
                    }
                    if (device[1] == "T1") {
                        MeitrackTcpManager meitractTcpManagerT1 = new MeitrackTcpManager();
                        meitractTcpManagerT1.Device = new Device { Company = "Meitrack", Name = "T1" };
                        meitractTcpManagerT1.IpAddress = attributes[1];
                        meitractTcpManagerT1.Port = Int32.Parse(attributes[2]);
                        meitractTcpManagerT1.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(meitractTcpManagerT1);
                    }

                } else if (device[0] == "Teltonika") {
                    if (device[1] == "FM1100") {
                        TqatCommandTcpManager tqatCommandTcpManager = new TqatCommandTcpManager();
                        tqatCommandTcpManager.Device = new Device { Company = "Teltonika", Name = "FM1100" };
                        tqatCommandTcpManager.IpAddress = attributes[1];
                        tqatCommandTcpManager.Port = Int32.Parse(attributes[2]);
                        tqatCommandTcpManager.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(tqatCommandTcpManager);
                    }
                } else if (device[0] == "Ats") {
                    if (device[1] == "Command") {
                        TqatCommandTcpManager tqatCommandTcpManager = new TqatCommandTcpManager();
                        tqatCommandTcpManager.Device = new Device { Company = "Ats", Name = "Command" };
                        tqatCommandTcpManager.IpAddress = attributes[1];
                        tqatCommandTcpManager.Port = Int32.Parse(attributes[2]);
                        tqatCommandTcpManager.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(tqatCommandTcpManager);
                    }
                }





            }


            listViewTcpManagers.ItemsSource = tcpManagers;
        }
        private void ButtonServersStart_Click (object sender, RoutedEventArgs e) {
            try {
                tcpManagers.Clear();
                foreach (TcpManager tcpManager in listViewTcpManagersSetup.Items) {
                    lock (tcpManager) {
                        if (tcpManager.IsEnabled == false)
                            continue;
                        tcpManagers.Add(tcpManager);
                    }
                }

                foreach (TcpManager tcpManager in tcpManagers) {
                    lock (tcpManager) {
                        if (buttonServersPause.IsEnabled == false) {
                            tcpManager.Event += TcpManager_Event;
                            tcpManager.DataReceived += TcpManager_DataReceived;
                            tcpManager.Packets = 0;
                            tcpManager.ReceiveBytes = 0;
                            tcpManager.SendBytes = 0;
                        }
                        tcpManager.BufferOut = bufferOut;
                        tcpManager.BufferIn = new ConcurrentQueue<object>();
                        tcpManager.Refresh();
                        tcpManager.Start();
                    }
                }

                groupTcpManagersSetup.IsEnabled = false;
                buttonServersStart.IsEnabled = false;
                buttonServersStop.IsEnabled = true;
                buttonServersPause.IsEnabled = true;
                machine.TimeSpanStart = true;

            } catch (Exception exception) {
                groupTcpManagersSetup.IsEnabled = true;
                foreach (TcpManager tcpManager1 in tcpManagers) {
                    try {
                        tcpManager1.Stop();
                        tcpManager1.Event -= TcpManager_Event;
                        tcpManager1.DataReceived -= TcpManager_DataReceived;
                    } catch {
                        continue;
                    }
                }
                tcpManagers.Clear();
                GC.Collect();
                if (debug) {
                    TextLog.Write(exception);
                }
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonServersStop_Click (object sender, RoutedEventArgs e) {
            try {
                groupTcpManagersSetup.IsEnabled = true;
                foreach (TcpManager tcpManager in tcpManagers) {
                    lock (tcpManager) {
                        tcpManager.Stop();
                        tcpManager.Event -= TcpManager_Event;
                        tcpManager.DataReceived -= TcpManager_DataReceived;
                    }
                }
                tcpManagers.Clear();
                GC.Collect();

                buttonServersStart.IsEnabled = true;
                buttonServersPause.IsEnabled = false;
                buttonServersStop.IsEnabled = false;
                machine.TimeSpanStart = false;
            } catch (Exception exception) {
                if (debug) {
                    TextLog.Write(exception);
                }
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonServersPause_Click (object sender, RoutedEventArgs e) {
            try {
                foreach (TcpManager tcpManager in tcpManagers) {
                    lock (tcpManager) {
                        tcpManager.Stop();
                    }
                }
                buttonServersPause.IsEnabled = false;
                buttonServersStart.IsEnabled = true;
                machine.TimeSpanStart = null;
            } catch (Exception exception) {
                if (debug) {
                    TextLog.Write(exception);
                }
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TcpManager_DataReceived (object sender, object data) {

            if (data == null)
                return;

            TcpManager tcpManager = (TcpManager)sender;


            if (data.GetType() == typeof(String[])) {
                String[] array = (String[])data;

                if (array != null) {
                    while (!bufferOut.TryAdd(array[0], array)) {
                        ;
                    }
                    tcpManager.Refresh();
                    return;
                }
            } else if (data.GetType() == typeof(Gm)) {
                Gm gm = (Gm)data;
                if (gm != null) {
                    tcpManager.BufferIn.Enqueue(gm);
                    tcpManager.Refresh();
                    return;
                }
            }

        }
        private void TcpManager_Event (object sender, AtsGps.Log log) {
            TcpManager tcpManager = (TcpManager)sender;
            log.Description = tcpManager.Device.ToString() + " : " + log.Description;

            if (debug) {
                TextLog.Write(log.Description, TextLogType.EVENT, TextLogFileType.TXT);
            }

            Dispatcher.BeginInvoke(new Action(() => {
                dataGridLog.Items.Add(log);
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

        private void ButtonTcpManagerSetupSave_Click (object sender, RoutedEventArgs e) {

            StringCollection servers = new StringCollection();

            foreach (TcpManager tcpManager in listViewTcpManagersSetup.Items) {
                String data = tcpManager.Device + "," + tcpManager.IpAddress + "," + tcpManager.Port.ToString() + "," + (tcpManager.IsEnabled == true ? "1" : "0");
                servers.Add(data);
            }

            Settings.Default.Servers = servers;
            Settings.Default.Save();

            MessageBox.Show("Successful!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
