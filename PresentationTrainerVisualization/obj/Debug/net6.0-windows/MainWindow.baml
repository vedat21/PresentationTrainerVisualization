   M S B A M L   `   `   `  ����[  WPresentationTrainerVisualization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:PresentationTrainerVisualization PresentationTrainerVisualization  5   	   1    +PresentationTrainerVisualization.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.61.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aae aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35J 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:PresentationTrainerVisualization     5   	   _ [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   /ui&http://schemas.lepo.co/wpfui/2022/xaml  5   	     � Title$  
MainWindow��5   	   $	��800��5   	   $
��1300��5   	   
�������.��5      �  � RowDefinitions 5   
   �� 5      $
��0.4* �6   �� 5      $��2* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � -   5         �Name$ DashboardConfiguration��5      $��1��6/   $	��Top=�6?   .+�5      �� -   5         ��Name$ ComboSessions��5      $	��200��6W   $
��Left=�6c   $��10q�6~   ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6%   .+�5      �� $
��Bold"�6(   $��	Session: ��6:   �� 5      $
��Bold"�6(   ��6:   �� *TextForComboBox+�� 5       $�� ��6(   �� 5!      $
��Bold"�6(   ��6:   �� *StartForComboBox+ �� StringFormat$ F��5"      5#      5%      � 5&      -   5        $ SelectDateSpan��5&      $��10q�6+   $��Right=�67    5'      �� 5(      $
��Auto �6'   �� 5)      $
��Auto �6'   .+�5+      �� $��Center=�6   $��1��6:   $ 
Horizontal=�6G   .+�5,      ��  !  MaterialDesignButtonTextBlock#��[  6    $��Center=�6W   $
��Left=�6r   $��10q�6�   From6�   (  "System.Windows.Controls.DatePicker  5-      -   5          Name$ 	StartDate��5-   !   $��Center=�65   $
��Left=�6P   6�   5.      �� 5/      $��1��6   $��1��6*   $ 
Horizontal=�6:   $
��Left=�6S   .+�50      �� #��[  6    $��Center=�6W   $
��Left=�6r   $��10q�6�   To6�     51      -   5        $ EndDate��51   !   $��Center=�62   $
��Left=�6M   6�   52      53      � 54      -   5        $ SelectLastSessions��54      $y�	Collapsed=�6/   $��Right=�6F   
������� 55      �� 56      $
��Auto �6'   �� 57      $
��Auto �6'   .+�59      �� $��5q�6   $ 
Horizontal=�6(   .+�5:      �� -   5         ��Name$ 	NumberOfX��5:      $��Center=�60   	 �� TickFrequency$	 1Q�6K   
 �� IsSnapToTickEnabled$

 True��6]   $	��200��6x    � Minimum$ 1Q�6�   $��1��6�   6�   �� 5;        GoalDescriptionStyle#��[ 6v   $��Right=�6�   $��Center=�6�   $��Center=�6�    �� TextAlignment$ Right=�6�   $��50��6	  $��1��6  $��1��6!  ��6   ��  �� ElementName$ 	NumberOfX�� �� Path$ Value� �� UpdateSourceTrigger$ PropertyChanged=�6!  5<      �� 5>      $��5q�6   $��1��6(   $ 
Horizontal=�68   .+�5?      �� #��[  6    $��Center=�6W   $
��Left=�6r   	Last Days6�   d� 5@      -   5         d�Name$ DaysOrSessions��5@   #   $��Center=�6\   �� 5A      #��[  6    $��Center=�6W   $��Right=�6r   Last Sessions6�   5B      5C      5D      �� 5F   
   -	   5        $��Sidebar��5F      $��2��6!    �� CornerRadius$ 8 0 0 8��62   ._�5G      �  5H      �� 5I      $
��Auto �6'   �� 5J      $��* �6'   .+�5L      �� $��0��6   ���#1c4e80$ 8 0 0 8��6>   ._�5M      � -
   5        $ 
Navigation��5M      $��60��6/    5N      �� 5O      $��60 �6,   �� 5P      $��* �6,   �� 5Q      $��* �6,   �� 5R      $
��0.2* �6,   .+�5T      �� $��1��6%   $��
-1 40 0 20q�62   .+�5U      �� -   5         � ToolTip$ 	Dashboard��5U   %   $��10q�6X   
������ 
������   Default#���  6�   .��5V   "   �� $ 
Horizontal=�6-   .+�5W   &   '  !MaterialDesignThemes.Wpf.PackIcon  
�������$��30��6V   $��30��6b      Kind$ ViewDashboardOutline=�6m   6�   �� 5X   &   -   5         ��Name$ TextDashboard1��5X   0     SidebarText#��[ 6H   $y�Hidden=�6m   $��Center=�6�   	Dashboard6�   5Y   #   5Z      �� 5c      -   5        $ Goals��5c   %   $��10q�6O   
������ 
������ #���  6�   .��5d   "   �� $ 
Horizontal=�6-   .+�5e   &     
�������$��30��6W   $��30��6c   $ FlagOutline=�6n   6�   �� 5f   &   -   5        $ TextGoalsSetting��5f   0   #��[ 6J   $y�Hidden=�6o   $��Center=�6�   Goals Setting6�   5g   #   5h      5i      5r      5s      � 5u      $��0��6    5v      �� 5w      $��70 �6(   �� 5x      $��* �6(   .+�5z      d� -   5          HamburgerButtonStyle#��[ 5z   #   $��Center=�6�   
������ $��0q�6�   $��35��6�   $��30��6�    � Triggers 5{      :� 5|       :� RoutedEvent$ ToggleButton.Unchecked��6+   .[�5}   "   �� .��5~   &   �� -   5         ��Name$ HideStackPanel��5~   1   .��5   *   W�  �� 
TargetName$ 
Navigation��6:    �� TargetProperty$ Width�6]    g� 	BeginTime	 0:0:06    W� From 2106�   	 W� To 606�    g� Duration$ 0:0:0.7D�6�   5�   +   5�   '   5�   #   5�      :� 5�      $ ToggleButton.Checked��6+   .[�5�   "   �� .��5�   &   �� -   5        $ ShowStackPanel��5�   1   .��5�   *   W� $ 
Navigation��6:   $ Width�6]   	 0:0:06    606�    2106�   $ 0:0:0.7D�6�   5�   +   5�   '   5�   #   5�      5�      5�      5�      5�       � 5�   
   -   5          �Name$ MainContainer��5�      $��20q�6'     � NavigationUIVisibility$ Hidden=�63   $��1��6S   $��0��6c   $��2��6p   5�      5�      