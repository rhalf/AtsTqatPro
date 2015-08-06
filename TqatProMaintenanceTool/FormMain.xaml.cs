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

using MahApps.Metro;
using MahApps.Metro.Controls;


using TqatProMaintenanceTool;
using TqatProMaintenanceTool.Properties;
using Controls.UserControls;

using TqatProModel;
using TqatProModel.Database;
using TqatProModel.Parser;

namespace TqatProMaintenanceTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FormMain : MetroWindow {
        public FormMain(Company company, User user, List<User> users, List<Tracker> trackers, Database database) {
            InitializeComponent();
        }
        
    }
}
