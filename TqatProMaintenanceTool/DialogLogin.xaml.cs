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

using TqatProMaintenanceTool;
using TqatProMaintenanceTool.Properties;
using Controls.UserControls;

using TqatProModel;
using TqatProModel.Database;
using TqatProModel.Parser;
using System.Net;

namespace TqatProMaintenanceTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DialogLogin : MetroWindow {

        public DialogLogin() {
            InitializeComponent();
        }

        Query query;
        Company company;
        User user;

        List<Company> companies;
        //List<User> users;
        //List<Tracker> trackers;

        private void PanelLogin_OnSubmitEventHandler(object sender, RoutedEventArgs e) {
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

        private void run(object state) {
            try {

                Dispatcher.Invoke(new Action(() => {
                    panelLogin.IsEnabled = false;
                    progressBarLoading.Visibility = Visibility.Visible;
                }));
           
                Database database = new Database { IpAddress = Settings.Default.databaseHost, Username = Settings.Default.databaseUsername, Password = Settings.Default.databasePassword };
                query = new Query(database);


                companies = query.getCompanies();
                //query.fillGeofences(company);
                //users = query.getUsers(company, user);
                int count = 0;
                foreach (Company companyItem in companies) {
                    query.getUser(companyItem, user);
                    //users = query.fillUsers(companyItem, user);
                    //companyItem.Trackers = query.getTrackers(companyItem, users);
                    Dispatcher.Invoke(new Action(() => {
                        panelLogin.ErrorNote = "Loading companies... " + (++count).ToString() + "/" + companies.Count.ToString();
                    }));
                }


                Dispatcher.Invoke(new Action(() => {
                    Settings.Default.accountCompanyUsername = panelLogin.CompanyUsername;
                    Settings.Default.accountUsername = panelLogin.Username;
                    Settings.Default.accountPassword = panelLogin.Password;
                    Settings.Default.accountRememberMe = (bool)panelLogin.RememberMe;
                    Settings.Default.Save();
                    FormMain formMain = new FormMain(company, user, companies, database);
                    formMain.Show();
                    this.Close();
                }));

            } catch (DatabaseException databaseException) {
                Debug.Print(databaseException.Message);
                Dispatcher.Invoke(new Action(() => {
                    panelLogin.ErrorNote = databaseException.Message;
                    Log.write(databaseException);
                }));
            } catch (Exception exception) {
                Debug.Print(exception.Message);
                Dispatcher.Invoke(new Action(() => {
                    panelLogin.ErrorNote = exception.Message;
                    Log.write(exception);
                }));
            } finally {
                Dispatcher.Invoke(new Action(() => {
                    panelLogin.IsEnabled = true;
                    progressBarLoading.Visibility = Visibility.Hidden;
                }));
            }
        }

        private void PanelLogin_OnCancelEventHandler(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {

            //Settings.Default.databaseHost = "184.107.175.154";
            //Settings.Default.databaseUsername = "reportapp";
            //Settings.Default.databasePassword = "my5q1r3p0rt@pp!@#";
            //Settings.Default.Save();

            Settings.Default.databaseHost = "108.163.190.202";
            Settings.Default.databaseUsername = "atstqatpro";
            Settings.Default.databasePassword = "@t5tq@pr0!@#";
            Settings.Default.Save();

            Settings.Default.accountCompanyUsername = "ats";
            Settings.Default.accountUsername = "master";
            Settings.Default.accountPassword = "m@5t3r!@#";
            Settings.Default.accountRememberMe = false;
            Settings.Default.Save();

            panelLogin.CompanyIsEnabled = false;
            panelLogin.UsernameIsEnabled = false;

            //if (Settings.Default.accountRememberMe == true) {
            panelLogin.CompanyUsername = Settings.Default.accountCompanyUsername;
            panelLogin.Username = Settings.Default.accountUsername;
            panelLogin.Password = Settings.Default.accountPassword;
            //   panelLogin.RememberMe = Settings.Default.accountRememberMe;
            //}
        }

    }
}
