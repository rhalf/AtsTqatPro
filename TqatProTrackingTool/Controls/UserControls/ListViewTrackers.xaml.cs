using System;
using System.Collections;
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


using TqatProViewModel;

namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for ListViewTrackers.xaml
    /// </summary>
    public partial class ListViewTrackers : UserControl {
        public ListViewTrackers() {
            InitializeComponent();
        }

        public ItemCollection Items {
            get {
                return listViewTrackers.Items;
            }
        }

        public object ItemsDataContext {
            get {
                return listViewTrackers.DataContext;
            }
            set {
                listViewTrackers.DataContext = value;
            }
        }
        public IEnumerable ItemsItemSource {
            get {
                return listViewTrackers.ItemsSource;
            }
            set {
                listViewTrackers.ItemsSource = value;
            }
        }

        public String ItemsDisplayMember{
            private get { return "";  }
      
            set {
                string property = (string)value;
                DataTemplate dataTemplate = (DataTemplate)Application.Current.Resources["listView" + property];
                listViewTrackers.ItemTemplate = dataTemplate;
            }
        }

        public object ItemsSelectedItem {
            get {
                return listViewTrackers.SelectedItem;
            }
            set {
                listViewTrackers.SelectedItem = value;
                listViewTrackers.ScrollIntoView(value);
            }
        }

        public int ItemsCheckedCount {
            get {
                int count = 0;
                foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                    if (trackerItem.IsChecked) {
                        count++;
                    }
                }
                return count;
            }
        }

        private void listViewTrackers_Checked(object sender, RoutedEventArgs e) {
            CheckBox checkBox = (CheckBox)sender;
            RaiseEvent(new RoutedEventArgs(ListViewTrackers.routedEventChecked));
           
        }




        //Events
        public static readonly RoutedEvent routedEventChecked = EventManager.RegisterRoutedEvent(
            "Checked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ListViewTrackers));

        //public static readonly RoutedEvent routedEventOnUnchecked = EventManager.RegisterRoutedEvent(
        //    "OnUnchecked",
        //    RoutingStrategy.Bubble,
        //    typeof(RoutedEventHandler),
        //    typeof(ListViewTrackers));

        //EventsHandler
        public event RoutedEventHandler Checked {
            add {
                AddHandler(routedEventChecked, value);
            }
            remove {
                RemoveHandler(routedEventChecked, value);
            }
        }

        //public event RoutedEventHandler routedEventHandlerOnUnchecked {
        //    add {
        //        AddHandler(routedEventOnUnchecked, value);
        //    }
        //    remove {
        //        RemoveHandler(routedEventOnUnchecked, value);
        //    }
        //}

        private void MenuItemlistViewTrackers_checkAll_Click(object sender, RoutedEventArgs e) {
            foreach(TrackerItem trackerItem in listViewTrackers.Items) {
                trackerItem.IsChecked = true;
            }
            listViewTrackers.Items.Refresh();
            RaiseEvent(new RoutedEventArgs(ListViewTrackers.routedEventChecked));

        }

        private void MenuItemlistViewTrackers_uncheckAll_Click(object sender, RoutedEventArgs e) {
            foreach (TrackerItem trackerItem in listViewTrackers.Items) {
                trackerItem.IsChecked = false;
            }
            listViewTrackers.Items.Refresh();
            RaiseEvent(new RoutedEventArgs(ListViewTrackers.routedEventChecked));
        }
    }
}
