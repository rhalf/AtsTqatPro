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
using MahApps.Metro;
using MahApps.Metro.Controls;
using TqatProTrackingTool.Properties;
using System.Threading;
using TqatProModel.Database;

namespace TqatProTrackingTool {
    /// <summary>
    /// Interaction logic for DialogDatabase.xaml
    /// </summary>
    public partial class DialogDatabase : MetroWindow {
        DialogLogin dialogLogin;

        public DialogDatabase (DialogLogin dialogLogin) {
            InitializeComponent();
            this.dialogLogin = dialogLogin;
        }

        private void MetroWindow_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
            if (groupBoxOther.IsEnabled == true) {
                try {
                    Settings.Default.databaseHost = textBoxIp.Text;
                    Settings.Default.databaseUsername = textBoxUsername.Text;
                    Settings.Default.databasePort = Int32.Parse(textBoxPort.Text);
                    Settings.Default.databasePassword = textBoxPassword.Password;
                    Settings.Default.Save();
                   
                } catch (Exception exception) {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
            }
            dialogLogin.Visibility = Visibility.Visible;
        }


        private void ButtonTry_Click (object sender, RoutedEventArgs e) {
            try {

                Settings.Default.databaseHost = textBoxIp.Text;
                Settings.Default.databaseUsername = textBoxUsername.Text;
                Settings.Default.databasePort = Int32.Parse(textBoxPort.Text);
                Settings.Default.databasePassword = textBoxPassword.Password;
                Settings.Default.databaseOther = textBoxIp.Text + "," + textBoxPort.Text + "," + textBoxUsername.Text + "," + textBoxPassword.Password;

                Settings.Default.webServiceIp = textBoxIp.Text;
                Settings.Default.webServicePort = 8000;

                Settings.Default.Save();

                Thread threadMysqlTest = new Thread(threadMysqlTestFunc);
                threadMysqlTest.Start();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
        }

        private void threadMysqlTestFunc () {
            try {
                Database database = new Database() {
                    IpAddress = Settings.Default.databaseHost,
                    Username = Settings.Default.databaseUsername,
                    Password = Settings.Default.databasePassword,
                    Port = Settings.Default.databasePort
                };

                Query query = new Query(database);
                query.checkConnection();

               Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show("Successful", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
            } catch (DatabaseException databaseException) {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(databaseException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            } catch (Exception exception) {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }

        }

        private void MetroWindow_Loaded (object sender, RoutedEventArgs e) {
            groupBoxOther.IsEnabled = false;
            if (Settings.Default.groupServersIndex == 1) {
                radioButtonServer1.IsChecked = true; }
            else if (Settings.Default.groupServersIndex == 2) {
                radioButtonServer2.IsChecked = true;
            } else if (Settings.Default.groupServersIndex == 3) {
                radioButtonServer3.IsChecked = true;
            } else if (Settings.Default.groupServersIndex == 0) {
                radioButtonServerX.IsChecked = true;
            }

            try {
                String[] db = Settings.Default.databaseOther.Split(',');
                textBoxIp.Text = db[0];
                textBoxPort.Text = db[1];
                textBoxUsername.Text = db[2];
                textBoxPassword.Password = db[3];
            } catch {

            }
        }

        private void radioButtonServer_Checked (object sender, RoutedEventArgs e) {
            RadioButton radioButton = (RadioButton)sender;
            groupBoxOther.IsEnabled = false;

            if (radioButton.Name.Equals("radioButtonServerX")) {
                groupBoxOther.IsEnabled = true;
                Settings.Default.groupServersIndex = 0;
                Settings.Default.Save();

            } else if (radioButton.Name.Equals("radioButtonServer1")) {
                //Ats Database Server
                Settings.Default.databaseHost = "108.163.190.202";
                Settings.Default.databaseUsername = "atstqatpro";
                Settings.Default.databasePort = 3306;
                Settings.Default.databasePassword = "@t5tq@pr0!@#";
                Settings.Default.groupServersIndex = 1;
                Settings.Default.webServiceIp = "67.205.85.177";
                Settings.Default.webServicePort = 8000;
                Settings.Default.Save();
            } else if (radioButton.Name.Equals("radioButtonServer2")) {
                //Ats Mowasalat Server
                Settings.Default.databaseHost = "184.107.175.154";
                Settings.Default.databaseUsername = "reportapp";
                Settings.Default.databasePort = 3306;
                Settings.Default.databasePassword = "my5q1r3p0rt@pp!@#";
                Settings.Default.groupServersIndex = 2;
                Settings.Default.webServiceIp = "184.107.175.154";
                Settings.Default.webServicePort = 8000;
                Settings.Default.Save();
            } else if (radioButton.Name.Equals("radioButtonServer3")) {
                //Ats Mowasalat Server
                Settings.Default.databaseHost = "108.163.190.202";
                Settings.Default.databaseUsername = "atstqatpro";
                Settings.Default.databasePort = 3306;
                Settings.Default.databasePassword = "@t5tq@pr0!@#";
                Settings.Default.groupServersIndex = 3;
                Settings.Default.webServiceIp = "72.55.132.40";
                Settings.Default.webServicePort = 8000;
                Settings.Default.Save();
            } else {
                groupBoxOther.IsEnabled = false;
                Settings.Default.groupServersIndex = 0;
                Settings.Default.Save();
            }
        }
    }
}
