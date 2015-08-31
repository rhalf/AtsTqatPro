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


namespace TqatProTrackingTool {
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

                Database database = new Database(Settings.Default.databaseHost, Settings.Default.databaseUsername, Settings.Default.databasePassword);
                query = new Query(database);

                query.getCompany(company);
                query.getUser(company, user);

                if (user.AccessLevel != 1) {
                    if (!company.IsActive)
                        throw new QueryException(1, "Company is deactivated.");
                    if (company.DateTimeExpired.CompareTo(DateTime.Now) != -1)
                        throw new QueryException(1, "Can't Login! This user is expired.");

                    if (user.DateTimeExpired.CompareTo(DateTime.Now) != -1)
                        throw new QueryException(1, "Can't Login! This user is expired.");
                    if (!user.IsActive)
                        throw new QueryException(1, "User is deactivated.");
                }
                //=============================Login successful

                query.fillGeofences(company);

                query.fillUsers(company, user);

                query.fillCollection(company);

                query.fillTrackers(company);

                query.fillPois(company);

                Dispatcher.Invoke(new Action(() => {
                    Settings.Default.accountCompanyUsername = panelLogin.CompanyUsername;
                    Settings.Default.accountUsername = panelLogin.Username;
                    Settings.Default.accountPassword = panelLogin.Password;
                    Settings.Default.accountRememberMe = panelLogin.RememberMe;
                    Settings.Default.Save();
                    FormMain formMain = new FormMain(company, user, database);
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

            Settings.Default.databaseHost = "184.107.175.154";
            Settings.Default.databaseUsername = "reportapp";
            Settings.Default.databasePassword = "my5q1r3p0rt@pp!@#";
            Settings.Default.Save();

            //Settings.Default.databaseHost = "108.163.190.202";
            //Settings.Default.databaseUsername = "atstqatpro";
            //Settings.Default.databasePassword = "@t5tq@pr0!@#";
            //Settings.Default.Save();

            //Settings.Default.accountCompanyUsername = "mowasalat";
            //Settings.Default.accountUsername = "admin";
            //Settings.Default.accountPassword = "admin";
            //Settings.Default.accountRememberMe = true;
            //Settings.Default.Save();

            if (Settings.Default.accountRememberMe == true) {
                panelLogin.CompanyUsername = Settings.Default.accountCompanyUsername;
                panelLogin.Username = Settings.Default.accountUsername;
                panelLogin.Password = Settings.Default.accountPassword;
                panelLogin.RememberMe = Settings.Default.accountRememberMe;
            }
        }

        private void panelLogin_Loaded(object sender, RoutedEventArgs e) {

        }

        private void panelLogin_Loaded_1(object sender, RoutedEventArgs e) {

        }






    }
}
