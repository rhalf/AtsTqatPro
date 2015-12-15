using MahApps.Metro;
using MahApps.Metro.Controls;


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


using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

using TqatProTrackingTool.Properties;
using Controls.UserControls;

using TqatProModel;
using TqatProModel.Database;
using TqatProModel.Parser;
using System.Net;

namespace TqatProTrackingTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DialogLogin : MetroWindow {

        public DialogLogin () {
            InitializeComponent();
        }

        Query query;
        Company company;
        User user;

        private void PanelLogin_OnSubmitEventHandler (object sender, RoutedEventArgs e) {
            //Validation
            panelLogin.ErrorNote = "";
            if (String.IsNullOrEmpty(panelLogin.CompanyUsername)) {
                panelLogin.ErrorNote = "Company field is empty.";
                return;
            }
            if (String.IsNullOrEmpty(panelLogin.Username)) {
                panelLogin.ErrorNote = "Username field is empty.";
                return;
            }
            if (String.IsNullOrEmpty(panelLogin.Password)) {
                panelLogin.ErrorNote = "Password field is empty.";
                return;
            }

            user = new User();
            company = new Company();
            company.Username = panelLogin.CompanyUsername;
            user.Username = panelLogin.Username;
            user.Password = Cryptography.md5(panelLogin.Password);
            user.RememberMe = panelLogin.RememberMe;

            ThreadPool.QueueUserWorkItem(new WaitCallback(run));
        }

        private void run (object state) {
            try {

                Dispatcher.Invoke(new Action(() => {
                    panelLogin.IsEnabled = false;
                    progressBarLoading.Visibility = Visibility.Visible;
                }));

                Database database = new Database { IpAddress = Settings.Default.databaseHost, Port = 3306, Username = Settings.Default.databaseUsername, Password = Settings.Default.databasePassword };
                query = new Query(database);

                displayMessage("Connecting to " + database.IpAddress + "...");

                query.getCompany(company);
                displayMessage("Checking Company...");

                query.getUser(company, user);
                displayMessage("Checking User...");


                if (user.AccessLevel != 1) {

                    int isExpired = company.DateTimeExpired.CompareTo(DateTime.Now);

                    if (isExpired == -1)
                        throw new QueryException(1, "This company is expired.");
                    if (!company.IsActive)
                        throw new QueryException(1, "Company is deactivated.");

                    isExpired = user.DateTimeExpired.CompareTo(DateTime.Now);

                    if (isExpired == -1)
                        throw new QueryException(1, "This user is expired.");
                    if (!user.IsActive)
                        throw new QueryException(1, "User is deactivated.");
                }
                //=============================Login successful
                query.fillUsers(company, user);
                displayMessage("Loading Users...");

                query.fillPois(company);
                displayMessage("Loading Pois...");

                query.fillGeofences(company);
                displayMessage("Loading Geofences...");

                query.fillCollection(company);
                displayMessage("Loading Collections...");

                query.fillTrackers(company);
                displayMessage("Loading Trackers...");


                Dispatcher.Invoke(new Action(() => {
                    Settings.Default.accountCompanyUsername = panelLogin.CompanyUsername;
                    Settings.Default.accountUsername = panelLogin.Username;
                    Settings.Default.accountPassword = panelLogin.Password;
                    Settings.Default.accountRememberMe = (bool)panelLogin.RememberMe;
                    Settings.Default.Save();
                    FormMain formMain = new FormMain(company, user, database);
                    formMain.Show();
                    this.Close();
                }));

            } catch (DatabaseException databaseException) {
                Debug.Print(databaseException.Message);
                displayMessage(databaseException.Message);
                TextLog.Write(databaseException);
            } catch (Exception exception) {
                Debug.Print(exception.Message);
                displayMessage(exception.Message);
                TextLog.Write(exception);
            } finally {
                Dispatcher.Invoke(new Action(() => {
                    panelLogin.IsEnabled = true;
                    progressBarLoading.Visibility = Visibility.Hidden;
                }));
            }
        }

        private void PanelLogin_OnCancelEventHandler (object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void MetroWindow_Loaded (object sender, RoutedEventArgs e) {
            this.Title = "TqatProTrackingTool";

            if (Settings.Default.accountRememberMe == true) {
                panelLogin.CompanyUsername = Settings.Default.accountCompanyUsername;
                panelLogin.Username = Settings.Default.accountUsername;
                panelLogin.Password = Settings.Default.accountPassword;
                panelLogin.RememberMe = Settings.Default.accountRememberMe;
            }
        }

        private void displayMessage (string message) {
            Dispatcher.Invoke(new Action(() => {
                panelLogin.ErrorNote = message;
            }));
        }

       

        private void Label_MouseUp (object sender, MouseButtonEventArgs e) {
            DialogDatabase dialogDatabase = new DialogDatabase(this);
            this.Hide();
            dialogDatabase.ShowDialog();

        }
    }
}
