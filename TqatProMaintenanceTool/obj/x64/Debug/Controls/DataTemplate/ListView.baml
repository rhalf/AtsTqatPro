   M S B A M L   `   `   `  ����K  GTqatProViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM8clr-namespace:TqatProViewModel;assembly=TqatProViewModelTqatProViewModel  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   listViewVehicleRegistration  �� DataType"    TqatProViewModel.TrackerItem �� Orientation d� 	IsChecked.  (System.Windows.Interactivity.Interaction   Triggers/  )System.Windows.Interactivity.EventTrigger   	EventName.  (System.Windows.Interactivity.TriggerBase   Actions6  0System.Windows.Interactivity.InvokeCommandAction   Command   CommandParameter  listViewDriverName  listViewVehicleModel  listViewOwnerName  listViewTrackerImei  listViewSimImei  listViewSimNumber  listViewTrackersData   listViewTrackersDatabaseSize&   System.Windows.Controls.DataGrid0  *System.Windows.Controls.DataGridTextColumn,  &System.Windows.Controls.DataGridColumn   Header1  +System.Windows.Controls.DataGridBoundColumn	   Binding�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      F	viewModel8clr-namespace:TqatProViewModel;assembly=TqatProViewModel  5      ViPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5      %�  &	        &	 9    &	 �    &	 �    &	 Y    &	     &	 �    &	 l    &	 �    �� 5      "    67   .R�5   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�5	      ��  6   �� *	IsChecked+ 5
        5      $ Checked��6%   . 5        $  ��60    6;   �� 5      5      5      �� 5      ��6   �� *VehicleRegistration+5      5      �� 5      .R�5   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�5      ��  6   �� *	IsChecked+�� 5      ��6   �� *
DriverName+5      5      �� 5      .R�5   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�5      ��  6   �� *	IsChecked+5      �� 5      ��6   �� *VehicleModel+5       5!      �� 5"      .R�5#   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�5$      ��  6   �� *	IsChecked+�� 5%      ��6   �� *	OwnerName+5&      5'      �� 5(      .R�5)   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�5*      ��  6   �� *	IsChecked+�� 5+      ��6   �� *TrackerImei+5,      5-      �� 5.      .R�5/   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�50      ��  6   �� *	IsChecked+�� 51      ��6   �� *	SimImei+52      53      �� 54      .R�55   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�56      ��  6   �� *	IsChecked+�� 57      ��6   �� *	SimNumber+58      59      �� 5<      .R�5=   
   �� $ 
Horizontal=�6   $��5,0,0,0q�6.   .+�5>      �� ��6   �� *
Tracker.Id+�� 5?      ��6   �� *
DateTime+�� 5@      ��6   �� *
Coordinate+�� 5A      ��6   �� *Speed+�� 5B      ��6   �� *	Mileage+�� 5C      ��6   �� *
Distance+�� 5D      ��6   �� *	EventCode+5F      5G      �� 5K      .R�5L   
     .@�5M        $
 Name��6<   	 6"   �� *Name+6L     5N      $ Database Size��6;   	 6"   �� *Size+6U     5O      $ Database Free Space��6@   	 6"   �� *	FreeSpace+6_     5P      $ Database Status��6F   	 6"   �� *DateTimeUpdated+6a     5Q      $ Database Status��6=   	 6"   �� *Status+6X   5R      5T      5U      