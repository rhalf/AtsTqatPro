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
using System.Threading;

namespace TqatProSocketTool {
    /// <summary>
    /// Interaction logic for DialogTcpClient.xaml
    /// </summary>
    public partial class DialogTcpClient : Window {

        public DialogTcpClient (TcpManager tcpManager) {
            InitializeComponent();
            dataGridTrackers.ItemsSource = tcpManager.TcpClients.Values;
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

        private void dataGridTrackers_Sorting (object sender, DataGridSortingEventArgs e) {

        }
    }
}
