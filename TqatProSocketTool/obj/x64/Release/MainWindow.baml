   M S B A M L   `   `   `  ���� L  HTqatProSocketTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TqatProSocketToolTqatProSocketTool  M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089C.clr-namespace:Microsoft.CSharp;assembly=SystemMicrosoft.CSharp "    TqatProSocketTool.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:TqatProSocketTool     >sys.clr-namespace:Microsoft.CSharp;assembly=System     -     � Title$  
MainWindow��$	��400��$	��800�� � WindowStartupLocation$ CenterScreen=�.���  � RowDefinitions �� $��40 ��� $��* ��� $��30 �.+�� $��0�� � ColumnDefinitions �� $��75 ��� $��75 ��� $��75 ��� $��* �.+��� -   $��buttonServersStart��$��5q�$��Start��$��0���� -   $��buttonServersStop��$��5q�$
��Stop��$��1��{�.  �� -   $��buttonCollect��$��5q�$��Clean��$��2��� $��1��.+��� $��Stretch=�$��Stretch=�.@��� $��Servers��.���  �� $��* �.+��� -   $��groupTcpManagersSetup��$��Tcp Manager��$��0��.���  �� $��* ��� $��30 �.+��� -   $��listViewTcpManagersSetup��$��Stretch=�$��Stretch=� �� View �� .P��� $��Company�� �� Width$	 100�� �� CellTemplate �� .R��� ���� *Device.Company+�� $��Device��$	 100�� �� .R��� ���� *Device.Name+�� $��
Ip Address��$	 140�� �� .R��� $	��120������ *	IpAddress+ �� UpdateSourceTrigger$ PropertyChanged=��� $
��Port��$ 80�� �� .R��� $��60������ *Port+$ PropertyChanged=��� $��	IsEnabled��$	 100�� �� .R���  d� 	IsChecked �� *	IsEnabled+�� $��1��	 �� Orientation$	 Vertical=�$��1q�.+��� $	 
Horizontal=�.+��� -   $	��Add��$��3q��� -   $
��Save��$��3q��� -	   $��Delete��$��3q��� $��Mysql��$
��Left=�$��48��.���� $��Mysql Database��$��2��.���� -
   $��mysqlCredential��$	 Vertical=�.+��� $	 
Horizontal=�$��3q�.+��� $��
Ip Address��$	��100���� $	��100������ *	IpAddress+
 �� TargetNullValue$
 0.0.0.0���� $	 
Horizontal=�$��3q�.+��� $
��Port��$	��100���� $	��100������ *Port+$
 0���� $	 
Horizontal=�$��3q�.+��� $��Database��$	��100���� $	��100������ *DatabaseName+$
  ���� $	 
Horizontal=�$��3q�.+��� $��Username��$	��100���� $	��100������ *
Username+$
  ���� $	 
Horizontal=�$��3q�.+��� $��Password��$	��100���� $	��100������ *
Password+$
  ���� $	 
Horizontal=�$��3q�.+��� $��Test Connection��$	��100���� -   $��buttonMysqlTest��$
��Test��$	��100���� $
��Logs��.��&   System.Windows.Controls.DataGrid  -   $��dataGridLog��$��Stretch=�$��Stretch=�$��0q�
�������   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ DateTime��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100    
IsReadOnly .    CellTemplate �� .R��� ���� *
DateTime+  $ Description��$ *  .  �� .R��� ���� *Description+  $ LogType��$	 100  .  �� .R��� ���� *	LogType+�� -   $��tabItemDashBoard��$��	Dashboard��.���  �� $	��100 ��� $��* ��� $��30 �.+��� -   $��groupMachine��$��Machine��$��0��.���  �� $��25 ��� $��25 ��� $��25 ��� $��25 � �� $	��100 ��� $��* ��� $��90 ��� $��* �.+��� $��OperatingSystem��$��0��$��0���� -   $��labelOperatingSystem��$��0��$��1��$��0q����� *OperatingSystem+ �� Mode$ OneWay=��� $��MachineName��$��0��$��2���� -   $��	labelName��$��0��$��3��$��0q����� *Name+$ OneWay=��� $��IP Addresses��$��1��$��2���� -   $��comboBoxIps��$��1��$��3��$��0q� �� 
IsEditable . ���� *IpAddresses+$ OneWay=��� $��	Processor��$��1��$��0���� -   $��	labelCore��$��1��$��1��$��0q����� *	Processor+$ OneWay=��� $��TimeSpan��$��2��$��0��$
��Left=�$	��100���� $��2��$��1������ *
TimeSpan+$ OneWay=��� $��UtcTime��$��2��$��2���� -   $��labelUtcDateTime��$��2��$��3��$��0q����� *UtcDateTime+$ OneWay=��� $��Status��$��1��.���� -   $��listViewTcpManagers��$��Stretch=� �� .P��� $��Company��$ 70�� �� DisplayMemberBinding �� *Device.Company+$ OneWay=��� $��Device��$ 70�� �� *Device.Name+$ OneWay=��� $��	IpAddress��$	 100�� �� *	IpAddress+$ OneWay=��� $
��Port��$ 50�� �� *Port+$ OneWay=��� $	��Tcp��$ 60�� �� *TcpClients.Count+$ OneWay=��� $��Trackers��$ 60�� �� *TcpClients.TrackersCount+$ OneWay=��� $��Packet S��$ 60�� �� *
PacketSent+$ OneWay=��� $��Packet R��$ 60�� �� *PacketReceived+$ OneWay=��� $��Byte S��$ 75�� �� *
ByteSent+$ OneWay=� �� StringFormat$ {0:N0}���� $��Byte R��$ 75�� �� *ByteReceived+$ OneWay=�$ {0:N0}���� $��BufferIn��$ 75�� �� *BufferIn.Count+$ OneWay=��� $��	BufferOut��$ 75�� �� *BufferOut.Count+$ OneWay=��� $��IsActivated��$ 75�� �� .R���  �� *IsActivated+$ OneWay=��� $	 
Horizontal=�$��2��.+��� -   $��Clear��$��3q�$��5,0,5,0q�