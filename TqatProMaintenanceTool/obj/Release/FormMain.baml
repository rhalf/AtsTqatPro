   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   TqatProMaintenanceTool%   TqatProMaintenanceTool.FormMain   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     -     � Title$  
MainWindow��$	��350��$	��525��.���  � ColumnDefinitions �� $
��185* ��� $
��332* � � RowDefinitions �� $
��auto ��� $��* �.+�+  %System.Windows.Controls.Ribbon.Ribbon  $��Stretch=�$��0��$��2��.@�.  (System.Windows.Controls.Ribbon.RibbonTab  $��Control��.@�0  *System.Windows.Controls.Ribbon.RibbonGroup  $��Control��.@�1  +System.Windows.Controls.Ribbon.RibbonButton  -     Name$ RibbonButtonStart��   LargeImageSource$, &Resources/images/64/must_have/play.png��   Label$ Start���� -    ��Name$ 
tabControl��$��1��$��Stretch=�$��2��.@��� $	��Log��$
��Auto��.���� -    ��Name$ listViewStatus��
�������$��Stretch=�$��Stretch=� �� View �� 	 �� AllowsColumnReorder	 .  .P��� $��Sequence��
 �� Width$

 Auto�� �� DisplayMemberBinding �� *
Sequence+�� $��DateTime��$

 Auto�� �� *
DateTime+�� $��Description��$

 Auto�� �� *Description+�� $��Status��$

 Auto�� �� *Status+