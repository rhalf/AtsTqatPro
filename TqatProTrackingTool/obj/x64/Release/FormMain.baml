   M S B A M L   `   `   `  ���� K  GTqatProViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM8clr-namespace:TqatProViewModel;assembly=TqatProViewModelTqatProViewModel  N JTqatProTrackingTool, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TqatProTrackingToolTqatProTrackingTool =#clr-namespace:Controls.UserControlsControls.UserControls "   TqatProTrackingTool.FormMain   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     T PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  H	viewModel8clr-namespace:TqatProViewModel;assembly=TqatProViewModel  -local!clr-namespace:TqatProTrackingTool  6userControls#clr-namespace:Controls.UserControls  -   $	��850��$	��500��   AccentColorBrush(  "MahApps.Metro.Controls.MetroWindow    	GlowBrush#  �    � 
ResizeMode$ CanResizeWithGrip=� � WindowStartupLocation$ CenterScreen=�$
��1024��$	��600��   ShowIconOnTitleBar .    ShowMinButton .    ShowMaxRestoreButton . *   #WindowButtonCommandsOverlayBehavior$ Always=� � Icon$/ )Resources/images/128/Ats/satellite_01.png�� � 	Resources   StyleRibbonMenuButtonUser	 �� 
TargetType3  -System.Windows.Controls.Ribbon.RibbonMenuItem
 �� Property �� Value  StyleRibbonComboBoxDisplay��%  &	       &	 �     �� "	  .��� 
 �@ ImageSource$/ )Resources/images/128/Animated/user_02.png���� 
 � �� �� *
Username+�� 
 �@�DataContext �� *
UserItem+-   �� "	  .��� 
 �@ ImageSource$/ )Resources/images/128/Animated/user_02.png��-   .��� $��Stretch=�$��Stretch=� � RowDefinitions �� $
��auto ��� $��* � � ColumnDefinitions �� $��* �.+�+  %System.Windows.Controls.Ribbon.Ribbon  $��0��$��0��   ApplicationMenu :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  .@�>  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -      ImageSource$* $Resources/images/128/Ats/logo_01.png��$)��#Advanced Technologies And Solutions���� ��    AuxiliaryPaneContent ��  �� Orientation$ Vertical=�.+��� $ 
Horizontal=�.+��� $��
Company : ���� -    ��Name$ labelCompany���� $ 
Horizontal=�.+��� $��Username : ���� -   $ 	labelUser���� $ 
Horizontal=�.+��� $��Connected : ���� -   $ labelDatabaseHost��   FooterPaneContent �� $ 
Horizontal=�.+�  -   $1 +Resources/images/128/Animated/logout_01.png��$��Logout��  -	   $3 -Resources/images/128/Animated/shutdown_01.png��$
��Exit��.@�.  (System.Windows.Controls.Ribbon.RibbonTab  $��Live Tracking��.@�0  *System.Windows.Controls.Ribbon.RibbonGroup  $
��User��.@�5  /System.Windows.Controls.Ribbon.RibbonMenuButton  -
     Name$ ribbonMenuButtonUser��   LargeImageSource$/ )Resources/images/128/Animated/user_00.png��   Label$
 User��#��[   $��Display��.@�  -   $# ribbonMenuButtonDisplayMember��$2 ,Resources/images/128/Animated/monitor_01.png��$ VehicleRegistration��.@�2	  ,System.Windows.Controls.Ribbon.RibbonGallery	  -    	 Name$( "ribbonGalleryComboBoxDisplayMember�� 	  SelectedValue$ VehicleRegistration�� 	  SelectedValuePath$ Content�� 	  MaxColumnCount$ 1��.@�:
  4System.Windows.Controls.Ribbon.RibbonGalleryCategory
  .@�6  0System.Windows.Controls.Ribbon.RibbonGalleryItem  $��VehicleRegistration��  $��
DriverName��  $��VehicleModel��  $��	OwnerName��  $��TrackerImei��  $��	SimNumber��  $��Trackers��.@�1  +System.Windows.Controls.Ribbon.RibbonButton  -     Name$ ribbonButtonTrackers��   LargeImageSource$. (Resources/images/128/Animated/car_00.png��   Label$ List  Show Hide��  -   $ ribbonButtonTrackersData��$. (Resources/images/128/Animated/car_00.png��$ Data Show Hide��� $��3��$��1�� �� $
��auto �$	��200���� $��* �.+�� -    �Name$ gridTrackers��$��0��$��Stretch=�$��Stretch=�$	��200�� �� $��* ��� $
��auto �.+��� $��0��$��Trackers��$��2q�$��Stretch=�$��Stretch=�.��� $��Stretch=�$��Stretch=� �� $
��auto ��� $
��auto ��� $��* �.+�� $��0��$	��Top=�$��2q� �� $
��auto ��� $��* �.+��� $��0��$��Collection : ���� -    ��Name$ comboBoxCollection��$��1��� $��1��$	��Top=�$��2q� �� $
��auto ��� $��* �.+��� $��0��$��	Search : ���� -     ��Name$  textBoxSearchTrackerData��$��1��,  &Controls.UserControls.ListViewTrackers  -   !  Name$! listViewTrackers��,  &Controls.UserControls.ListViewTrackers"   Checked$" listViewTrackers_Checked��$��Stretch=�$��Stretch=�$��2��#   ItemsDisplayMember$#  ���� $��1��$ 
Horizontal=�.+��� $��Trackers : ���� ���� *Items.Count+$ �� ElementName$$ listViewTrackers��% �� Mode$% OneWay=�� $��1�� �� $��* ��� $
��auto �$	��300��.+�(  "System.Windows.Controls.WebBrowser  -   &  Name$& webBrowserMap��$��0��$��Stretch=�$��Stretch=�' K� ZIndex$
' -100��� -   $ gridTrackersData��$��1��$��Stretch=�$��Stretch=�$	��300�� �� $��* ��� $
��auto �.+�� $��0��$��Stretch=�$��Stretch=�.+��� $��Trackers Data��$��2q�$��Stretch=�$��Stretch=�.���� -   ( ��Name$( listViewTrackersData��$��Stretch=�$��Stretch=�$��2q�$) �� IsSynchronizedWithCurrentItem$
) TrueY�* �� View* �� + �� AllowsColumnReorder+ . .P��� $��Label��, �� Width$
, Auto��- �� DisplayMemberBinding- �� *Tracker.VehicleRegistration+�� $
��Date��$
, Auto��- �� *
DateTime+�� $��
Coordinate��$
, Auto��- �� *
Coordinate+�� $��Speed��$
, Auto��- �� *Speed+�� $��Mileage��$
, Auto��- �� *	Mileage+�� $��Altitude��$
, Auto��- �� *
Altitude+�� $��Degrees��$
, Auto��- �� *	Degrees+�� $��	Direction��$
, Auto��- �� *	Direction+�� $��GpsSatellites��$
, Auto��- �� *GpsSatellites+�� $��	GsmSignal��$
, Auto��- �� *	GsmSignal+�� $	��ACC��$
, Auto��. �� CellTemplate. �� .R��� / J� IsHitTestVisible/ .  0 d� 	IsChecked0 �� *ACC+�� $	��SOS��$
, Auto��. �� .R��� / .  0 �� *SOS+�� $	��EPC��$
, Auto��. �� .R��� / .  0 �� *EPC+�� $��	OverSpeed��$
, Auto��. �� .R��� / .  0 �� *	OverSpeed+�� $��	EventCode��$
, Auto��- �� *	EventCode+�� $��Battery��$
, Auto��- �� *	Battery+�� $��BatteryVoltage��$
, Auto��- �� *BatteryVoltage+�� $��ExternalVoltage��$
, Auto��- �� *ExternalVoltage+�� $��Geofence��$
, Auto��- �� *
Geofence+� $��1��$��Stretch=�$��Stretch=� �� $
��auto ��� $��1000* ��� $
��auto ��� .+��� $ 
Horizontal=�$��0��.+��� $��Trackers : ���� ���� *Items.Count+$$ listViewTrackersData��$% OneWay=��� $��
Threads : ���� -   $ threadCount��$��0���� $ 
Horizontal=�$��1��$��Stretch=�$��Stretch=�