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
using AtsGps.Ats;
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

        List<TcpManager> tcpManagers;
        ConcurrentDictionary<String, String[]> bufferOut;
        Machine machine;

        #region BufferStart
        Thread threadUploaderManager, threadGuiUpdater;
        Boolean debug = true;

        private void threadUploaderManagerFunc (Object state) {
            while (true) {
                Thread.Sleep(5000);
                if (tcpManagers.Count <= 0)
                    continue;

                try {
                    lock (tcpManagers) {
                        foreach (TcpManager tcpManager in tcpManagers) {
                            if (tcpManager == null)
                                continue;
                            if (tcpManager.BufferIn == null)
                                continue;
                            if (tcpManager.BufferIn.IsEmpty)
                                continue;
                            if (tcpManager.Device.Company == "Meitrack") {
                                processBufferIn(tcpManager);
                            }
                        }
                    }
                } catch {
                    continue;
                }
            }
        }

        private void threadGuiUpdaterFunc(Object state) {
            while (true) {
                Thread.Sleep(1000);
                lock (tcpManagers) {
                    foreach (TcpManager tcpManager in tcpManagers) {
                        tcpManager.Refresh();
                    }
                }
            }
        }

        private void processBufferIn (Object state) {
            Task newTask = Task.Factory.StartNew(() => {

                TcpManager tcpManager = (TcpManager)state;
                while (!tcpManager.BufferIn.IsEmpty) {

                    Object obj = null;
                    if (!tcpManager.BufferIn.TryDequeue(out obj)) {
                        continue;
                    }

                    Gm gm = null;

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

                        object object1 = obj;

                    } catch (Exception exception) {
                        tcpManager.BufferIn.Enqueue(obj);
                        if (debug) {
                            TextLog.Write(exception);
                        }
                    }
                }
            });
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


            threadGuiUpdater = new Thread(threadGuiUpdaterFunc);
            threadGuiUpdater.Start(tcpManagers);


            bufferOut = new ConcurrentDictionary<String, String[]>();

        }

     

        private void initializedTcpManagers () {
            tcpManagers = new List<TcpManager>();

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
                if (tcpManagers.Count <= 0) {
                    foreach (TcpManager tcpManager in listViewTcpManagersSetup.Items) {

                        if (tcpManager.IsEnabled == false)
                            continue;
                        tcpManagers.Add(tcpManager);
                    }
                }


                foreach (TcpManager tcpManager in tcpManagers) {
                    tcpManager.Event += TcpManager_Event;
                    tcpManager.DataReceived += TcpManager_DataReceived;


                    if (tcpManager.Device.Name != "Command") {
                        tcpManager.BufferOut = bufferOut;
                        tcpManager.BufferIn = new ConcurrentQueue<object>();
                        tcpManager.TcpTrackers = new ConcurrentDictionary<String, TcpTracker>();
                    }
                    tcpManager.Start();
                }


                groupTcpManagersSetup.IsEnabled = false;
                buttonServersStart.IsEnabled = false;
                buttonServersStop.IsEnabled = true;
                machine.TimeSpanStart = true;

            } catch (Exception exception) {
                groupTcpManagersSetup.IsEnabled = true;
                foreach (TcpManager tcpManager1 in tcpManagers) {
                    try {
                        tcpManager1.Stop();
                        Thread.Sleep(1);
                        tcpManager1.Event -= TcpManager_Event;
                        tcpManager1.DataReceived -= TcpManager_DataReceived;
                    } catch {
                        continue;
                    }
                }
                tcpManagerClear();

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
                        if (tcpManager.IsActivated && tcpManager != null) {
                            tcpManager.Stop();
                            Thread.Sleep(1);
                            tcpManager.Event -= TcpManager_Event;
                            tcpManager.DataReceived -= TcpManager_DataReceived;
                        }
                    }
                }

                GC.Collect();

                buttonServersStart.IsEnabled = true;
                buttonServersStop.IsEnabled = false;
                machine.TimeSpanStart = false;
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
                    return;
                }
            } else if (data.GetType() == typeof(Gm)) {
                Gm gm = (Gm)data;
                if (gm != null) {
                    tcpManager.BufferIn.Enqueue(gm);
                    return;
                }
            }

        }
        private void TcpManager_Event (object sender, AtsGps.Log log) {
            TcpManager tcpManager = (TcpManager)sender;
            log.Description = tcpManager.Device.ToString() + " : " + log.Description;


            if (debug) {
                if (log.LogType == LogType.MVT100) {
                    TextLog.Write(log.Description, "MVT100", TextLogFileType.TXT);
                }
                if (log.LogType == LogType.T1) {
                    TextLog.Write(log.Description, "T1", TextLogFileType.TXT);
                }
                if (log.LogType == LogType.FM110) {
                    TextLog.Write(log.Description, "FM1100", TextLogFileType.TXT);
                }

                if (log.LogType == LogType.SERVER) {
                    TextLog.Write(log.Description, "SERVER", TextLogFileType.TXT);
                    Dispatcher.BeginInvoke(new Action(() => {
                        dataGridLog.Items.Add(log);
                    }));
                }
            }
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
                    if (tcpManager.IsActivated) {
                        tcpManager.Stop();
                        Thread.Sleep(1);
                    }
                }
                tcpManagerClear();
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
        private void ButtonTcpManagerSetupAdd_Click (object sender, RoutedEventArgs e) {
            DialogServerAdd formServerAdd = new DialogServerAdd();
            formServerAdd.ShowDialog();
            if (formServerAdd.Tag != null) {
                listViewTcpManagersSetup.Items.Add((TcpManager)formServerAdd.Tag);
            }
        }
        private void ButtonTcpManagerSetupDelete_Click (object sender, RoutedEventArgs e) {
            listViewTcpManagersSetup.Items.Remove(listViewTcpManagersSetup.SelectedItem);
        }

        private void listViewTcpManagersSetup_SelectionChanged (object sender, SelectionChangedEventArgs e) {

        }

        private void listViewTcpManagers_MouseDoubleClick (object sender, MouseButtonEventArgs e) {
            ListView listView = (ListView)sender;

            if (listView.SelectedItem == null)
                return;

            TcpManager tcpManager = (TcpManager)listView.SelectedItem;

            DialogTcpClient dialogTcpClient = new DialogTcpClient(tcpManager);
            dialogTcpClient.Show();
        }

        private void ButtonClear_Click (object sender, RoutedEventArgs e) {
            tcpManagerClear();
        }

        private void tcpManagerClear () {
            if (buttonServersStart.IsEnabled) {
                //while (!tcpManagers.IsEmpty) {
                //    TcpManager tcpManager = null;
                tcpManagers.Clear();
                //}
            }
        }
    }
}
