   M S B A M L   `   `   `  ����E  ASimulation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Simulation
Simulation  5   	   ;"clr-namespace:Simulation.ViewmodelSimulation.Viewmodel  5   	   3clr-namespace:Simulation.ViewsSimulation.Views  5   	       Simulation.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Simulation     5   	   8	viewmodel"clr-namespace:Simulation.Viewmodel     5   	   0viewsclr-namespace:Simulation.Views     5   	     � Title$  
MainWindow��5
   	   $	��720��6   $
��1280��6)   $
��1280��66   $	��720��6F    � 	Resources 5       �� 
TargetType �� Property �� Value �� DataType'  !Simulation.Viewmodel.SimViewModel   Simulation.Views.SimView � DataContext,  &Simulation.Viewmodel.SettingsViewModel#   Simulation.Views.SettingsView(  "Simulation.Viewmodel.StatViewModel   Simulation.Views.StatView��%�  '��       (�� g     5   ,   *, +)(�� �     5   1   *, +)(��     5   -   *, +)�� 5   
   " ��6   .�5      ��  �@��BitmapScalingMode$ HighQuality=�5      5      �� 5   
   SimTemplate��"  6,   .R�5         6   �� 5      �� 5   
   SettingsTemplate��"  61   .R�5         6!   �� 5      �� 5   
   StatTemplate��"  6-   .R�5         6   �� 5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      �� 5       �� 5!       � RowDefinitions 5$   
   �� 5%      �� 5&      �� 5'      �� 5(      �� 5)      .+�5,   
   � ���	#FFE0C59F$��0��6&   $��0��63   $��5��6C   .+�5-      �� .+�5.      �� -   5        $��80��5.      $��
Simulation��6%   
�������$��30(�6M   $
��Bold"�6[   ���	#FF3A62A46�   �� 5/      -   5        $��80��5/      $��Settings��6%   
�������$��30(�6K   $
��Bold"�6Y   ���	#FF3A62A46�   50      �� 51      $��Bottom=�6   $��/Resources/intro.png��6/   52      �� 54   
   $��0��6-   $��1��6:   $��4��6J   $��5��6^   ��6   �� 6^   55      56      