   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5       TqatProMaintenanceTool%   TqatProMaintenanceTool.FormMain   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5      -   5          � Title$  
MainWindow��5      $	��350��6   $	��525��6%   .��5      �  � ColumnDefinitions 5   
   �� 5	      $
��185* �6   �� 5
      $
��332* �6    � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      $��* �6   6)   .+�5   
   +  %System.Windows.Controls.Ribbon.Ribbon  $��Stretch=�6   $��0��6/   $��2��6<   .@�5      .  (System.Windows.Controls.Ribbon.RibbonTab  $��Control��6   .@�5      0  *System.Windows.Controls.Ribbon.RibbonGroup  $��Control��6   .@�5      1  +System.Windows.Controls.Ribbon.RibbonButton  -   5          Name$ RibbonButtonStart��5   #      LargeImageSource$, &Resources/images/64/must_have/play.png��6>      Label$ Start��6x   6�   5      5      5      �� 5   
   -   5         ��Name$ 
tabControl��5      $��1��6)   $��Stretch=�66   $��2��6U   .@�5      �� $	��Log��6   $
��Auto��6#   .��5      �� -   5         ��Name$ listViewStatus��5      
�������$��Stretch=�6J   $��Stretch=�6h    �� View 5      �� 5      	 �� AllowsColumnReorder	 .  .P�5       �� $��Sequence��6-   
 �� Width$

 Auto��6?    �� DisplayMemberBinding 6L   �� *
Sequence+6x   �� 5!      $��DateTime��6-   $

 Auto��6@    6M   �� *
DateTime+6y   �� 5"      $��Description��6-   $

 Auto��6C    6P   �� *Description+6�   �� 5#      $��Status��6-   $

 Auto��6>    6K   �� *Status+6u   5$      5&      5'      5(      5)      5*      