   M S B A M L   `   `   `  ����K  GTqatProViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM8clr-namespace:TqatProViewModel;assembly=TqatProViewModelTqatProViewModel  5   	   N JTqatProTrackingTool, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TqatProTrackingToolTqatProTrackingTool 5   	   =#clr-namespace:Controls.UserControlsControls.UserControls 5	   	   "   TqatProTrackingTool.FormMain   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   T PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  5   	   H	viewModel8clr-namespace:TqatProViewModel;assembly=TqatProViewModel  5   	   -local!clr-namespace:TqatProTrackingTool  5   	   6userControls#clr-namespace:Controls.UserControls  5	   	   -   5        $	��600��5   
   $	��400��6      AccentColorBrush(  "MahApps.Metro.Controls.MetroWindow    	GlowBrush#  �   5   	    � 
ResizeMode$ CanResizeWithGrip=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   $
��1024��5   	   $	��600��5   	      ShowIconOnTitleBar .    ShowMinButton .    ShowMaxRestoreButton . *   #WindowButtonCommandsOverlayBehavior$ Always=�5   	    � Icon$/ )Resources/images/128/Ats/satellite_01.png��5   	    � 	Resources 5!        StyleRibbonMenuButtonUser	 �� 
TargetType3  -System.Windows.Controls.Ribbon.RibbonMenuItem
 �� Property �� Value  StyleRibbonComboBoxDisplay��%�  &	       &	 �     �� 5"   
   "	  62   .�5#      �� 
 �@ ImageSource$/ )Resources/images/128/Animated/user_02.png��6,   �� 5$      
 � �� 6'   �� *
Username+�� 5%      
 �@�DataContext 6,   �� *
UserItem+-   5        5'      �� 5(   
   "	  63   .�5)      �� 
 �@ ImageSource$/ )Resources/images/128/Animated/user_02.png��6,   -   5        5+      5-         RightWindowCommands 50      +  %MahApps.Metro.Controls.WindowCommands  51   
   .@�52      �� -   5        $��Trackers��52      6(   55      .��59      � $��Stretch=�6   $��Stretch=�6'    � RowDefinitions 5:   
   �� 5;      $
��auto �6   �� 5<      $��* �6    � ColumnDefinitions 5?   
   �� 5@      $��* �6    .+�5B   
   +  %System.Windows.Controls.Ribbon.Ribbon  $��0��6   $��0��6!      ApplicationMenu 5C      :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5D      .@�5E      >  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -   5           ImageSource$* $Resources/images/128/Ats/logo_01.png��5E   0   $)��#Advanced Technologies And Solutions��6c   6�   �� 5F      6   �� 5G      6   �� 5H      6   �� 5I      6      AuxiliaryPaneContent 5J      �� 5K       �� Orientation$ Vertical=�6%   .+�5L      �� $ 
Horizontal=�6)   .+�5M   "   �� $��
Company : ��6(   6?   �� 5N   "   -   5         ��Name$ labelCompany��5N   (   6@   5O      �� 5P      $ 
Horizontal=�6)   .+�5Q   "   �� $��Username : ��6(   6@   �� 5R   "   -   5        $ 	labelUser��5R   (   6=   5S      �� 5T      $ 
Horizontal=�6)   .+�5U   "   �� $��Connected : ��6(   6A   �� 5V   "   -   5        $ labelDatabaseHost��5V   (   6E   5W      5X         FooterPaneContent 5Z      �� 5[      $ 
Horizontal=�6%   .+�5\        -	   5        $1 +Resources/images/128/Animated/logout_01.png��5\   8   $��Logout��6r   6�     5]      -
   5        $3 -Resources/images/128/Animated/shutdown_01.png��5]   8   $
��Exit��6t   6�   5^      5`      .@�5b      .  (System.Windows.Controls.Ribbon.RibbonTab  $��Live Tracking��6   .@�5c      0  *System.Windows.Controls.Ribbon.RibbonGroup  $
��User��6   .@�5d      5	  /System.Windows.Controls.Ribbon.RibbonMenuButton	  -   5         	 Name$ ribbonMenuButtonUser��5d   '    	  LargeImageSource$/ )Resources/images/128/Animated/user_00.png��6E    	  Label$
 User��6�   #��[ 6�   5e        5f      $��Display��6   .@�5g      	  -   5        $# ribbonMenuButtonDisplayMember��5g   (   $2 ,Resources/images/128/Animated/monitor_01.png��6O   $ VehicleRegistration��6�   #��[ 6�   .@�5h      2
  ,System.Windows.Controls.Ribbon.RibbonGallery
  -   5         
 Name$( "ribbonGalleryComboBoxDisplayMember��5h   (    
  SelectedValue$ VehicleRegistration��6T    
  SelectedValuePath$ Content��5i       
  MaxColumnCount$ 1��5j      .@�5k      :  4System.Windows.Controls.Ribbon.RibbonGalleryCategory  .@�5l   "   6  0System.Windows.Controls.Ribbon.RibbonGalleryItem  $��VehicleRegistration��64     5m   "   $��
DriverName��64     5n   "   $��VehicleModel��64     5o   "   $��	OwnerName��64     5p   "   $��TrackerImei��64     5q   "   $��	SimNumber��64   5r      5s      5t      5v        5w      $��Trackers��6   .@�5x      1  +System.Windows.Controls.Ribbon.RibbonButton  -   5          Name$ ribbonButtonTrackers��5x   #      LargeImageSource$. (Resources/images/128/Animated/car_00.png��6A      Label$ List  Show Hide��6}   6�     5z      -   5        $ ribbonButtonTrackersData��5z   #   $. (Resources/images/128/Animated/car_00.png��6E   $ Data Show Hide��6�   6�   5{      5|      5}      � 5   
   $��3��6   $��1��6    5�      �� 5�      $
��auto �6#   $	��200��60   �� 5�      $��* �6#   .+�5�      � -   5         �Name$ gridTrackers��5�   ]   $��0��6   $��Stretch=�6#   $��Stretch=�6A   $	��200��6s    5�      �� 5�      $��* �6$   61   �� 5�      $
��auto �6$   64   .+�5�      �� $��0��6   $��Trackers��6(   $��2q�6:   $��Stretch=�6E   $��Stretch=�6c   .��5�      � $��Stretch=�6   $��Stretch=�69    5�      �� 5�      $
��auto �6,   6<   �� 5�      $
��auto �6,   6<   �� 5�      $��* �6,   69   .+�5�      � $��0��6   $	��Top=�6,   $��2q�6D    5�      �� 5�   "   $
��auto �63   6B   �� 5�   "   $��* �63   6?   .+�5�      �� $��0��6$   $��Collection : ��64   �� 5�      -   5          ��Name$  comboBoxCollection��5�   7   $��1��6'   6S   5�      � 5�      $��1��6   $	��Top=�6,   $��2q�6D    5�      �� 5�   "   $
��auto �63   6B   �� 5�   "   $��* �63   6?   .+�5�      �� $��0��6$   $��	Search : ��64   �� 5�      -   5        ! ��Name$! textBoxSearchTrackerData��5�   6   $��1��6&   6X   5�      ,  &Controls.UserControls.ListViewTrackers  5�      -   5        "  Name$" listViewTrackers��5�   8   ,  &Controls.UserControls.ListViewTrackers#   Checked$# listViewTrackers_Checked��6�   $��Stretch=�6R   $��Stretch=�6p   $��2��6�   $   ItemsDisplayMember$$  ��6�   5�      5�      �� 5�      $��1��6   $ 
Horizontal=�6*   .+�5�      �� $��Trackers : ��6   �� 5�      ��6   �� *Items.Count+% �� ElementName$% listViewTrackers��& �� Mode$& OneWay=�5�      5�      � 5�      $��1��6    5�      �� 5�      $��* �6$   �� 5�      $
��auto �6$   $	��300��62   .+�5�      (  "System.Windows.Controls.WebBrowser  -   5        '  Name$' webBrowserMap��5�   *   $��0��6   $��Stretch=�6B   $��Stretch=�6`   ( K� ZIndex$
( -100��6|   � 5�      -   5        $ gridTrackersData��5�   ^   $��1��6   $��Stretch=�6$   $��Stretch=�6B   $	��300��6y    5�      �� 5�      $��* �6(   �� 5�      $
��auto �6(   .+�5�      � $��0��6   $��Stretch=�6*   $��Stretch=�6H   .+�5�      �� $��Trackers Data��6$   $��2q�6;   $��Stretch=�6F   $��Stretch=�6d   .��5�      �� -   5        ) ��Name$) listViewTrackersData��5�   '   $��Stretch=�5�   '   $��Stretch=�5�   '   $��2q�5�   '   $* �� IsSynchronizedWithCurrentItem$
* TrueY�5�   '   + �� View+ 5�   "   �� 5�   &   , �� AllowsColumnReorder, . .P�5�   *   �� $��Label��69   - �� Width$
- Auto��6H   . �� DisplayMemberBinding. 6U   �� *Tracker.VehicleRegistration+�� 5�   *   $
��Date��69   $
- Auto��6G   . 6T   �� *
DateTime+�� 5�   *   $��
Coordinate��69   $
- Auto��6M   . 6Z   �� *
Coordinate+�� 5�   *   $��Speed��69   $
- Auto��6H   . 6U   �� *Speed+�� 5�   *   $��Mileage��69   $
- Auto��6J   . 6W   �� *	Mileage+�� 5�   *   $��Altitude��69   $
- Auto��6K   . 6X   �� *
Altitude+�� 5�   *   $��Degrees��69   $
- Auto��6J   . 6W   �� *	Degrees+�� 5�   *   $��	Direction��69   $
- Auto��6L   . 6Y   �� *	Direction+�� 5�   *   $��GpsSatellites��69   $
- Auto��6P   . 6]   �� *GpsSatellites+�� 5�   *   $��	GsmSignal��69   $
- Auto��6L   . 6Y   �� *	GsmSignal+�� 5�   *   $	��ACC��69   $
- Auto��6F   / �� CellTemplate/ 5�   .   �� 5�   2   .R�5�   6   �� 0 J� IsHitTestVisible0 .  1 d� 	IsChecked1 6?   �� *ACC+6Y   5�   3   5�   +   �� 5�   *   $	��SOS��69   $
- Auto��6F   / 5�   .   �� 5�   2   .R�5�   6   �� 0 .  1 6?   �� *SOS+6Y   5�   3   5�   +   �� 5�   *   $��	OverSpeed��69   $
- Auto��6L   / 5�   .   �� 5�   2   .R�5�   6   �� 0 .  1 6?   �� *	OverSpeed+6`   5�   3   5�   +   �� 5�   *   $��	EventCode��69   $
- Auto��6L   . 6Y   �� *	EventCode+�� 5�   *   $��Battery��69   $
- Auto��6J   . 6W   �� *	Battery+�� 5�   *   $��BatteryVoltage��69   $
- Auto��6Q   . 6^   �� *BatteryVoltage+�� 5�   *   $��ExternalVoltage��69   $
- Auto��6R   . 6_   �� *ExternalVoltage+�� 5�   *   $��Geofence��69   $
- Auto��6K   . 6X   �� *
Geofence+5�   '   5�      5�      5�      � 5�      $��1��6   $��Stretch=�6(   $��Stretch=�6F    5�      �� 5�      $
��auto �6/   6>   �� 5�      $��1000* �6/   6?   �� 5�      $
��auto �6/   6>   �� 5�      61   .+�5�      �� $ 
Horizontal=�6%   $��0��6>   .+�5�      �� $��Trackers : ��6$   �� 5�      ��6$   �� *Items.Count+$% listViewTrackersData��$& OneWay=��� 5�      $��
Threads : ��6$   �� 5�      -   5        $ threadCount��5�   $   $��0��69   5�      �� 5�      $ 
Horizontal=�6%   $��1��6?   $��Stretch=�6O   $��Stretch=�6m   5�      5     5     5     5     5     5L     