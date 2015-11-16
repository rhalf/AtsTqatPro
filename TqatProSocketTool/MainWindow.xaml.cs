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
using TqatProSocketTool.Model;
using System.Collections.Concurrent;
using System.Threading;

namespace TqatProSocketTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window {
        public MainWindow () {
            InitializeComponent();
        }

        List<TcpManager> tcpManagers = new List<TcpManager>();
        List<ServerProfile> serverProfiles = new List<ServerProfile>();
        ConcurrentBag<Byte[]> mvt100Bag = new ConcurrentBag<Byte[]>();
        Thread threadManager;


        private void Window_Loaded (object sender, RoutedEventArgs e) {
            this.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            Machine machine = new Machine();

            labelOperatingSystem.DataContext = machine;
            labelName.DataContext = machine;
            comboBoxIps.DataContext = machine;
            labelLocalDateTime.DataContext = machine;
            labelUtcDateTime.DataContext = machine;
            labelCore.DataContext = machine;



            ServerProfile spMeitrackMvt100 = new ServerProfile();
            spMeitrackMvt100.IpAddresses = machine.IpAddresses;
            spMeitrackMvt100.Brand = "Meitrack";
            spMeitrackMvt100.Model = "Mvt100";
            spMeitrackMvt100.Port = 8887;
            spMeitrackMvt100.IsEnabled = true;
            serverProfiles.Add(spMeitrackMvt100);

            ServerProfile spMeitrackT1 = new ServerProfile();
            spMeitrackT1.IpAddresses = machine.IpAddresses;
            spMeitrackT1.Brand = "Meitrack";
            spMeitrackT1.Model = "T1";
            spMeitrackT1.Port = 4000;
            spMeitrackT1.IsEnabled = true;
            serverProfiles.Add(spMeitrackT1);



            foreach (ServerProfile sp in serverProfiles) {
                listViewServerProfile.Items.Add(sp);
            }
        }

        private void Button_Click (object sender, RoutedEventArgs e) {
            try {
                Button button = (Button)sender;
                if ((String)button.Content == "Start") {
                   
                    foreach (ServerProfile sp in serverProfiles) {
                        //Mvt100
                        if (sp.IsEnabled == true && sp.Brand == "Meitrack" && sp.Model == "Mvt100") {
                            listViewServerProfileStatus.Items.Add(sp);
                            MeitractTcpManager meitractTcpManagerMvt100 = new MeitractTcpManager(sp.IpAddress, sp.Port);
                            meitractTcpManagerMvt100.Event += MeitractTcpManagerMvt100_Event;
                            meitractTcpManagerMvt100.DataReceived += MeitractTcpManagerMvt100_DataReceived;
                            meitractTcpManagerMvt100.Start();
                            tcpManagers.Add(meitractTcpManagerMvt100);

                            threadManager = new Thread(threadManagerFunc);
                            threadManager.Start();
                        }
                        //T1
                        if (sp.IsEnabled == true && sp.Brand == "Meitrack" && sp.Model == "T1") {
                            listViewServerProfileStatus.Items.Add(sp);
                            MeitractTcpManager meitractTcpManagerT1 = new MeitractTcpManager(sp.IpAddress, sp.Port);
                            meitractTcpManagerT1.Event += MeitractTcpManagerT1_Event;
                            meitractTcpManagerT1.DataReceived += MeitractTcpManagerT1_DataReceived;
                            meitractTcpManagerT1.Start();
                            tcpManagers.Add(meitractTcpManagerT1);
                            threadManager.Abort();
                            GC.Collect();
                        }
                    }
                    button.Content = "Stop";
                    groupServerProfiles.IsEnabled = false;
                } else {
                    button.Content = "Start";
                    groupServerProfiles.IsEnabled = true;
                    listViewServerProfileStatus.Items.Clear();
                    foreach (TcpManager tcpManager in tcpManagers) {
                        tcpManager.Stop();
                    }
                    tcpManagers.Clear();
                }
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void threadManagerFunc () {
           while(true) {
                Byte[] bytes;
                mvt100Bag.TryTake(out bytes);
                ThreadPool.QueueUserWorkItem(new WaitCallback(threadProcessBytes), bytes);
            }
        }

        private void threadProcessBytes (object state) {
            Byte[] data = (Byte[])state;

            if (data == null)
                return;

            Gm gm = new Gm(data);

        }

        private void MeitractTcpManagerT1_DataReceived (byte[] data) {
            if (data == null)
                return;
        }

        private void MeitractTcpManagerT1_Event (ServerLog serverLog) {
            serverLog.Description = "T1 - " + serverLog.Description;
            Dispatcher.BeginInvoke(new Action(() => {
                dataGridLog.Items.Add(serverLog);
            }));
        }

        private void MeitractTcpManagerMvt100_DataReceived (byte[] data) {
            if (data == null)
                return;

            Gm gm = new Gm(data);

            //mvt100Bag.Add(data);
        }

        private void MeitractTcpManagerMvt100_Event (ServerLog serverLog) {
            serverLog.Description = "Mvt100 - " + serverLog.Description;
            Dispatcher.BeginInvoke(new Action(() => {
                dataGridLog.Items.Add(serverLog);
            }));
        }
    }
}
