   M S B A M L   `   `   `  ����L  HTqatProSocketTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TqatProSocketToolTqatProSocketTool  5   	   '    !TqatProSocketTool.DialogTcpClient   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:TqatProSocketTool     5   	     � Title$  DialogTcpClient��5   	   $	��400��6!   $	��600��6.    � WindowStartupLocation$ CenterOwner=�5	   	   .��5
      �  � RowDefinitions 5   
   �� 5      $��40 �6   6+   �� 5      $��* �6   6)   .+�5   
   �� $��0��6   $��2q�6    .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $
��Imei��6   $��3q�6(   65   �� 5      -   5        $��textBoxImei��5      $	��150��6-   $��3q�6:   6G   �� 5      -   5        $��buttonSearch��5      $��Search��6-   $��3q�6Y   6f   5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGridTrackers��5         AutoGenerateColumns .  $��1��6p   "   VerticalScrollBarVisibility$
 Auto=�6}      Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6)   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 65   
   CellTemplate
 5      �� 5      .R�5      �� ��6&   �� *Id+ �� Mode$ OneWay=�6J   5      5        5      $
 Imei��6)   $	 * 67   
 5       �� 5!      .R�5"      �� ��6&   �� *Imei+$ OneWay=�6L   5#      5%        5&      $ Ip��6)   $	 * 65   
 5'      �� 5(      .R�5)      �� ��6&   �� *Ip+$ OneWay=�6I   5*      5,        5-      $
 Port��6)   $	 * 67   
 5.      �� 5/      .R�50      �� ��6&   �� *Port+$ OneWay=�6K   51      53        54      $ DataIn��6)   $	 * 69   
 55      �� 56      .R�57      �� ��6&   �� *DataIn+$ OneWay=�6M   58      5:        5<      $ DataOut��6)   $	 * 6:   
 5=      �� 5>      .R�5?      �� ��6&   �� *	DataOut+$ OneWay=�6N   5@      5B        5D      $ DateTime��6)   $	 * 6;   
 5E      �� 5F      .R�5G      �� ��6&   �� *
DateTime+$ OneWay=�6O   5H      5J      5M      5N      5O      