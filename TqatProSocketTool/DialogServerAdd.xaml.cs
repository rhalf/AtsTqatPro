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
using AtsGps;
using TqatProSocketTool.ViewModel;
using AtsGps.Meitrack;
using AtsGps.Ats;

namespace TqatProSocketTool {
    /// <summary>
    /// Interaction logic for FormServerAdd.xaml
    /// </summary>
    public partial class DialogServerAdd : Window {
        public DialogServerAdd () {
            InitializeComponent();
        }

        private void comboBoxCompany_SelectionChanged (object sender, SelectionChangedEventArgs e) {
            ComboBox comboBox = (ComboBox)sender;

            ComboBoxItem comboBoxItem = (ComboBoxItem)comboBox.SelectedItem;

            if (comboBoxDevice == null) {
                return;
            }

            comboBoxDevice.Items.Clear();
            if ((String)comboBoxItem.Content == "Meitrack") {
                comboBoxDevice.Items.Add("Mvt100");
                comboBoxDevice.Items.Add("T1");
            } else if ((String)comboBoxItem.Content == "Teltonika") {
                comboBoxDevice.Items.Add("FM1100");
            }
        }

        private void Window_Loaded (object sender, RoutedEventArgs e) {
            Machine machine = new Machine();
            comboBoxIp.ItemsSource = machine.IpAddresses;
            comboBoxCompany_SelectionChanged(comboBoxCompany, null);
        }

        private void textBoxPort_PreviewTextInput (object sender, TextCompositionEventArgs e) {
            e.Handled = new System.Text.RegularExpressions.Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void buttonAdd_Click (object sender, RoutedEventArgs e) {
            //validation
            var company = ((ComboBoxItem)comboBoxCompany.SelectedItem).Content;
            var device = (String)(comboBoxDevice.SelectedItem);
            var ip = ((String)comboBoxIp.SelectedItem);
            var port = ((String)textBoxPort.Text);
            var isEnabled = checkBoxEnabled.IsChecked;

            if (String.IsNullOrEmpty((String)company)) {
                MessageBox.Show("Company is Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(device)) {
                MessageBox.Show("Device is Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(ip)) {
                MessageBox.Show("Ip is Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(port)) {
                MessageBox.Show("Port is Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TcpManager tcpManager = null;

            if ((String)company == "Meitrack") {
                tcpManager = new MeitrackTcpManager() {
                    Device = new Device() {
                        Company = (String)company,
                        Name = (String)device
                    },
                    IpAddress = (String)ip,
                    Port = Int32.Parse((String)port),
                    IsEnabled = (bool)isEnabled
                };
            } else if ((String)company == "Ats") {
                if ((String)device == "Command") {
                    tcpManager = new TqatCommandTcpManager() {
                        Device = new Device() {
                            Company = (String)company,
                            Name = (String)device
                        },
                        IpAddress = (String)ip,
                        Port = Int32.Parse((String)port),
                        IsEnabled = (bool)isEnabled
                    };
                }
            } else if ((String)company == "Teltonika") {
                if ((String)device == "FM1100") {
                    tcpManager = new TqatCommandTcpManager() {
                        Device = new Device() {
                            Company = (String)company,
                            Name = (String)device
                        },
                        IpAddress = (String)ip,
                        Port = Int32.Parse((String)port),
                        IsEnabled = (bool)isEnabled
                    };
                }
            }


            this.Tag = tcpManager;
            this.Close();
        }
    }
}
