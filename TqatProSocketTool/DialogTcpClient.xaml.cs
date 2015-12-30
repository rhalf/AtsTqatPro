using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using AtsGps;
using System.Collections;

namespace TqatProSocketTool {
    /// <summary>
    /// Interaction logic for DialogTcpClient.xaml
    /// </summary>
    public partial class DialogTcpClient : Window {



        public DialogTcpClient (TcpManager tcpManager) {
            InitializeComponent();
            dataGridTrackers.Items.Clear();

            if (tcpManager.Device.Company != "Ats") {
                foreach (TcpTracker tcpTracker in tcpManager.TcpTrackers.Values) {
                    TcpTracker tcpTracker1 = new TcpTracker() { Imei = tcpTracker.Imei, TcpClient = tcpTracker.TcpClient };
                    dataGridTrackers.Items.Add(tcpTracker1);
                }
            } else {
                foreach (TcpClient tcpClient in tcpManager.TcpClients) {
                    TcpTracker tcpPair1 = new TcpTracker() {Imei = "Not A Tracking Device", TcpClient = tcpClient };
                    dataGridTrackers.Items.Add(tcpPair1);
                }
            }

        }

        private void dialogTrackers_LoadingRow (object sender, DataGridRowEventArgs e) {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void buttonSearch_Click (object sender, RoutedEventArgs e) {
            foreach (TcpTracker tcpPair in dataGridTrackers.Items) {
                if (tcpPair.Imei.Contains(textBoxImei.Text)) {
                    dataGridTrackers.ScrollIntoView(tcpPair);
                    dataGridTrackers.SelectedItem = tcpPair;
                }
            }
        }
    }
}
