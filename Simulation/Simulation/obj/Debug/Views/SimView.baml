   M S B A M L   `   `   `  ����E  ASimulation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Simulation.ViewsSimulation.Views  5          Simulation.Views.SimView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Simulation.Views     5      -   5        .��5	      � -   5        $��SimGrid��5	        � ShowGridLines  .  ��5
   
   �� 5       �� ImageSource$ /Resources/tableBack.jpg��6    � ColumnDefinitions 5   
   �� 5      -   5         �Name$ 
ColumnDef1��5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      -   5        $ RowDef1��5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       �� 5!      �� 5"      �� 5#      .+�5%   
   �� -   5        $��Cardpack_label��5%      $��52��6&   $��0��63   $��7��6C   $��Center=�6P   $��Center=�6m   $
��Bold"�6�   �� 5&   
   -   5        $��Cardpack_img��5&      $��/Resources/cardpack.png��6$   $��8��6E   $��0��6R   �� 5'   
   -   5         ��Name$ StartButton��5'      $��9��6&   $��0��63   $��
Start Game��6C   ���	#FF3A62A4
�������$��5,5,5,5q�6�    � 	Resources 5(       �� 
TargetType �� Property	 �� Value��%_   '��       �� 5)      " ��6   .�5*      ��  �@��CornerRadius$	 10��65   5+      5,      5-      �� 5.   
   -   5        $ NextStageButton��5.      $��9��6*   $��1��67   $��
Next Stage��6G   ���	#FF3A62A4
�������$��5,5,5,5q�6�   {�.   5/      ��%_   '��       �� 50      " ��6   .�51      ��  �@��CornerRadius$	 10��65   52      53      54      �� 55   
   -	   5        $ NextActionButton��55      $��9��6+   $��2��68   $��Next Action��6H   ���	#FF3A62A4
�������$��5,5,5,5q�6�   {�.   56      ��%_   '��       �� 57      " ��6   .�58      ��  �@��CornerRadius$	 10��65   59      5:      5;      �� 5<   
   -
   5        $ SetTimerButton��5<      $��9��6)   $��3��66   $
��Auto��6F   ���	#FF3A62A4
�������$��5,5,5,5q�6�   {�.   5=      ��%_   '��       �� 5>      " ��6   .�5?      ��  �@��CornerRadius$	 10��65   5@      5A      5B      �� 5C   
   -   5        $ KillTimerButton��5C      $��9��6*   $��4��67   $��	Stop Auto��6G   ���	#FF3A62A4
�������$��5,5,5,5q�6�   {�.   5D      ��%_   '��       �� 5E      " ��6   .�5F      ��  �@��CornerRadius$	 10��65   5G      5H      5I      �� 5J   
   -   5        $��cardTableFlop_img��5J      $��/Resources/backside.png��6)   $��4��6J   $��3��6W   $��2��6g   $y�Hidden=�6x   �� 5K   
   -   5        $��cardTableTern_img��5K      $��/Resources/backside.png��6)   $��4��6J   $��4��6W   $��2��6g   $y�Hidden=�6x   �� 5L   
   -   5        $��cardTableRiver_img��5L      $��/Resources/backside.png��6*   $��4��6K   $��5��6X   $��2��6h   $y�Hidden=�6y   5M      5N      