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

namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for PanelLogin.xaml
    /// </summary>
    public partial class PanelLogin : UserControl {

        public bool CompanyIsEnabled {
            get { return textBoxCompany.IsEnabled; }
            set { textBoxCompany.IsEnabled = value; }
        }

        public bool UsernameIsEnabled {
            get { return textBoxUsername.IsEnabled; }
            set { textBoxUsername.IsEnabled = value; }
        }


        public string CompanyUsername {
            get {
                return textBoxCompany.Text;
            }
            set {
                textBoxCompany.Text = value;
            }
        }
        public string Username {
            get {
                return textBoxUsername.Text;
            }
            set {
               textBoxUsername.Text = value;
            }
        }
        public string Password {
            get {
                return textBoxPassword.Password;
            }
            set {
                textBoxPassword.Password = value;
            }
        }
        public string ErrorNote {
            get {
                return textBlockErrorNote.Text;
            }
            set {
                textBlockErrorNote.Text = value;
            }
        }
        public bool? RememberMe {
            get {
                return checkBoxRememberMe.IsChecked;
            }
            set {
                checkBoxRememberMe.IsChecked = true;
            }
        }

        //Events
        public static readonly RoutedEvent OnSubmitEvent = EventManager.RegisterRoutedEvent(
            "OnSubmit",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PanelLogin));

        public static readonly RoutedEvent OnCancelEvent = EventManager.RegisterRoutedEvent(
            "OnCancel",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PanelLogin));

        //EventsHandeler
        public event RoutedEventHandler OnSubmitEventHandler {
            add {
                AddHandler(OnSubmitEvent, value);
            }
            remove {
                RemoveHandler(OnSubmitEvent, value);
            }
        }

        public event RoutedEventHandler OnCancelEventHandler {
            add {
                AddHandler(OnCancelEvent, value);
            }
            remove {
                RemoveHandler(OnCancelEvent, value);
            }
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e) {
            RaiseEvent(new RoutedEventArgs(PanelLogin.OnSubmitEvent));
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e) {
            RaiseEvent(new RoutedEventArgs(PanelLogin.OnCancelEvent));
        }

        public void Clear() {
            textBoxCompany.Text = "";
            textBoxUsername.Text = "";
            textBoxPassword.Password = "";
        }
        public PanelLogin() {
            InitializeComponent();
        }

    }
}
