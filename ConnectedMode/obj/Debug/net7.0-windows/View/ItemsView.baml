   M S B A M L   `   `   `  ����b  ^Microsoft.VisualBasic.Core, Version=12.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3aaGclr-namespace:Microsoft.VisualBasic;assembly=Microsoft.VisualBasic.CoreMicrosoft.VisualBasic  5       ConnectedMode"   ConnectedMode.View.ItemsView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _visualBasicGclr-namespace:Microsoft.VisualBasic;assembly=Microsoft.VisualBasic.Core     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3b+http://schemas.microsoft.com/xaml/behaviors  5      .��5
      �   � ColumnDefinitions  5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��3��5      $��30��5      $	��Top=�5      $��Center=�5      $��0��5       $��0��5!      ��5"      �� *SearchKeyword+�� 5$   
   $��3��5%      $��0��5&      $��30��5'      $	��Top=�5(      $��
🔍Search��5)      $��5,0,0,5q�5*      ��5+      �� *SearchCommand+�� 5-   
   $��0��6   $��4��5.      $��2��5/      $	��0.5q�50      
���   �$��30��52      $	��Top=�53      $��5,0,0,0q�54       �� Padding$ 5,0,5,0q�55      $��Right=�56      ._�57      �   58      �� 59      �� 5:      .+�5<      �� $��0��6   $��Items count : ��5=      $��Center=�5>      $��Medium"�5?      �� 5@      $��1��6   $��Medium"�5A      $��Center=�5B      $��Center=�5C      ��5D      �� *QuantityCount+5E      5F      � 5H   
   $��1��6   $��0��5I      $��2��5J      $��6��5K        5M      �� 5N      �� 5O      .+�5R      &   System.Windows.Controls.DataGrid  -   5          Name$ 	ItemsList��5V      $��0��5R         AutoGenerateColumns$ False��5U      ��5S      �� *	ItemsList+ �� SelectedItem 5T      �� *SelcetedItem+   Columns 5W      0  *System.Windows.Controls.DataGridTextColumn  5X      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6)      
IsReadOnly$
 True��6L   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 65   �� *Id+6L     5Y      $ 	Item Name��6)   $
 True��6Y   	 6<   �� *
ItemName+6Y     5Z      $ Category��6)   $
 True��6X   	 6;   �� *
Category+6X     5[      $ Price��6)   $
 True��6R   	 68   �� *Price+6R     5\      $ Quantity��6)   $
 True��6X   	 6;   �� *
Quantity+6X     5]      $ Description��6)   $
 True��6^   	 6>   �� *Description+6^   5_      �� 5b      -   5        
 ��Name$
 SearchedItems��5b   &   $��1��6   $��Stretch=�6c   ��6=   �� *SearchedItems+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5c      +  %Microsoft.Xaml.Behaviors.EventTrigger  5d         	EventName$ MouseDoubleClick��6%   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5e      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 60   �� *DescriptionCommand+   CommandParameter 6W   �� *SelectedItem+ �� ElementName$ SearchedItems��5f      ��5h      �� 5i      .R�5j      ]� $��False��6$   .+�5k      �� $
��Left=�6(   $��5q�6X   ��6>   �� *
ItemName+6X   5l      5m      5o      5r      � 5u   
   $��4��6   $��0��6   $��6��6,     5v      �� 5w      �� 5x      �� 5y      �� 5z      .+�5}      �� $��0��5~      $��Add Item��5      $��30��5�      $��5q�5�      ��5�      �� *AddItemCommand+�� 5�      $��1��5�      $��Description��5�      $��30��5�      $��5q�5�      ��5�      �� *DescriptionCommand+��5�      �� $ SearchedItems�� �� Path$ SelectedItem��� 5�      $��2��5�      $��Delete Item��5�      $��30��5�      $��5q�5�      ��5�      �� *DeleteCommand+��5�      �� $ 	ItemsList��$ SelectedItem��� 5�      $��3��5�      $��Update Item��5�      $��30��5�      $��5q�5�      ��5�      �� *UpdateItemCommand+��5�      �� $ 	ItemsList��$ SelectedItem�5�      5�      5�      