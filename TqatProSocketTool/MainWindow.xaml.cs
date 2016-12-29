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
using AtsGps.Teltonika;

namespace TqatProSocketTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window {
        public MainWindow () {
            InitializeComponent();
        }

        ConcurrentDictionary<Int64, TcpManager> tcpManagers;
        ConcurrentDictionary<String, String[]> bufferOut;

        Machine machine;

        #region BufferStart
        Thread threadGuiUpdater;
        Boolean debug = true;
        Boolean taskUploader = false;

        Database _database;

        private void createTaskUploader () {
            try {
                if (tcpManagers.Count <= 0)
                    throw new Exception("tcpManagers is empty.");

                taskUploader = true;

                _database = new Database {
                    IpAddress = Settings.Default.DatabaseIp,
                    Port = Int32.Parse(Settings.Default.DatabasePort),
                    DatabaseName = Settings.Default.DatabaseName,
                    Username = Settings.Default.DatabaseUsername,
                    Password = Settings.Default.DatabasePassword
                };

                foreach (TcpManager tcpManager in tcpManagers.Values) {
                    if (tcpManager == null)
                        continue;

                    //if (tcpManager.Device.Company == "Meitrack") {
                    //    processBufferIn(tcpManager);
                    //}

                    //if (tcpManager.Device.Company == "Teltonika") {
                    //    processBufferIn(tcpManager);
                    //}

                    int taskCount = 10;

                    for (int count = 0; count < taskCount; count++) {
                        Task task = new Task(new Action(() => {
                            processBufferIn(tcpManager);
                        }));

                        task.Start();
                    }


                }
            } catch (Exception exception) {
                if (debug) {
                    TextLog.Write(exception);
                }
            }
        }
        private void disposeTaskUploader () {
            taskUploader = false;
        }

        private void processBufferIn (Object state) {

            TcpManager tcpManager = (TcpManager)state;

            using (Query query = new Query(_database)) {

                while (taskUploader == true) {

                    if (tcpManager.BufferIn == null) {
                        Thread.Sleep(100);
                        continue;
                    }
                    if (tcpManager.BufferIn.IsEmpty) {
                        Thread.Sleep(100);
                        continue;
                    }

                    Object obj = null;
                    Gm gm = null;
                    Tracker tracker = null;

                    //Get data from the buffer.
                    if (!tcpManager.BufferIn.TryDequeue(out obj))
                        continue;

                    //Cast Obj into Gm
                    try {
                        gm = (Gm)obj;
                        if (gm == null) {
                            return;
                        }
                    } catch (Exception exception) {
                        if (debug) {
                            TextLog.Write(exception);
                        }
                        continue;
                    }


                    //Check tracker if registered to the database, return 
                    try {
                        tracker = query.getTracker(gm.Unit);
                    } catch (Exception exception) {
                        if (debug) {
                            TextLog.Write(exception);
                        }
                        continue;
                    }

                    //Insert Data to the database server
                    try {

                        query.insertTrackerData(tracker, gm);
                        tracker.Dispose();

                    } catch (Exception exception) {
                        tcpManager.BufferIn.Enqueue(obj);
                        if (debug) {
                            TextLog.Write(exception);
                        }
                    }
                }
            }
        }

        private void threadGuiUpdaterFunc (Object state) {

            Action action = new Action(() => {
                this.listViewTcpManagers.Items.Refresh();
            });

            while (true) {
                Thread.Sleep(5000);
                try {
                    foreach (TcpManager tcpManager in tcpManagers.Values) {
                        tcpManager.Refresh();
                    }

                    Dispatcher.Invoke(action);

                } catch (Exception exception) {
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




            threadGuiUpdater = new Thread(threadGuiUpdaterFunc);
            threadGuiUpdater.Start(tcpManagers);


            bufferOut = new ConcurrentDictionary<String, String[]>();
        }
        private void initializedTcpManagers () {
            tcpManagers = new ConcurrentDictionary<long, TcpManager>();

            StringCollection servers = Settings.Default.Servers;


            Int32 id = 0;

            foreach (string server in servers) {
                string[] attributes = server.Split(',');
                string[] device = attributes[0].Split('|');

                id++;

                if (device[0] == "Meitrack") {
                    if (device[1] == "Mvt100") {
                        MeitrackTcpManager meitrackMvt100 = new MeitrackTcpManager();
                        meitrackMvt100.Id = id;
                        meitrackMvt100.Device = new Device { Company = "Meitrack", Name = "Mvt100" };
                        meitrackMvt100.IpAddress = attributes[1];
                        meitrackMvt100.Port = Int32.Parse(attributes[2]);
                        meitrackMvt100.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(meitrackMvt100);
                    }
                    if (device[1] == "T1") {
                        MeitrackTcpManager meitractTcpManagerT1 = new MeitrackTcpManager();
                        meitractTcpManagerT1.Id = id;
                        meitractTcpManagerT1.Device = new Device { Company = "Meitrack", Name = "T1" };
                        meitractTcpManagerT1.IpAddress = attributes[1];
                        meitractTcpManagerT1.Port = Int32.Parse(attributes[2]);
                        meitractTcpManagerT1.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(meitractTcpManagerT1);
                    }

                } else if (device[0] == "Teltonika") {
                    if (device[1] == "FM1100") {
                        TeltonikaTcpManager teltonikaTcpManager = new TeltonikaTcpManager();
                        teltonikaTcpManager.Id = id;
                        teltonikaTcpManager.Device = new Device { Company = "Teltonika", Name = "FM1100" };
                        teltonikaTcpManager.IpAddress = attributes[1];
                        teltonikaTcpManager.Port = Int32.Parse(attributes[2]);
                        teltonikaTcpManager.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(teltonikaTcpManager);
                    }
                } else if (device[0] == "Ats") {
                    if (device[1] == "Command") {
                        TqatCommandTcpManager tqatCommandTcpManager = new TqatCommandTcpManager();
                        tqatCommandTcpManager.Id = id;
                        tqatCommandTcpManager.Device = new Device { Company = "Ats", Name = "Command" };
                        tqatCommandTcpManager.IpAddress = attributes[1];
                        tqatCommandTcpManager.Port = Int32.Parse(attributes[2]);
                        tqatCommandTcpManager.IsEnabled = attributes[3] == "0" ? false : true;
                        listViewTcpManagersSetup.Items.Add(tqatCommandTcpManager);
                    }
                }
            }



        }
        private void ButtonServersStart_Click (object sender, RoutedEventArgs e) {
            try {


                if (tcpManagers.Count <= 0) {
                    foreach (TcpManager tcpManager in listViewTcpManagersSetup.Items) {

                        if (tcpManager.IsEnabled == false)
                            continue;

                        tcpManagers.TryAdd(tcpManager.Id, tcpManager);
                    }
                }

                listViewTcpManagers.ItemsSource = tcpManagers.Values;
                listViewTcpManagers.Items.Refresh();


                foreach (TcpManager tcpManager in tcpManagers.Values) {
                    tcpManager.Event += TcpManager_Event;
                    tcpManager.DataReceived += TcpManager_DataReceived;


                    if (tcpManager.Device.Name != "Command") {
                        tcpManager.BufferOut = bufferOut;
                        tcpManager.BufferIn = new ConcurrentQueue<object>();
                        tcpManager.TcpClients = new TcpClients();
                    }
                    tcpManager.Start();
                }

                createTaskUploader();



                groupTcpManagersSetup.IsEnabled = false;
                buttonServersStart.IsEnabled = false;
                buttonServersStop.IsEnabled = true;
                machine.TimeSpanStart = true;

            } catch (Exception exception) {
                groupTcpManagersSetup.IsEnabled = true;
                foreach (TcpManager tcpManager1 in tcpManagers.Values) {
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

                if (debug) {
                    TextLog.Write(exception);
                }
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonServersStop_Click (object sender, RoutedEventArgs e) {
            try {

                disposeTaskUploader();

                groupTcpManagersSetup.IsEnabled = true;
                foreach (TcpManager tcpManager in tcpManagers.Values) {
                    lock (tcpManager) {
                        if (tcpManager.IsActivated && tcpManager != null) {
                            tcpManager.Stop();
                            Thread.Sleep(10);
                            tcpManager.Event -= TcpManager_Event;
                            tcpManager.DataReceived -= TcpManager_DataReceived;
                        }
                    }
                }

                listViewTcpManagers.ItemsSource = null;
                listViewTcpManagers.Items.Refresh();
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
                if (log.LogType == LogType.FM1100) {
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
                foreach (TcpManager tcpManager in tcpManagers.Values) {
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
            try {
                if (buttonServersStart.IsEnabled) {
                    tcpManagers.Clear();
                }
            } catch {
                ;
            }

        }

        private void buttonCollect_Click (object sender, RoutedEventArgs e) {
            GC.Collect();
        }
    }
}
