   M S B A M L   `   `   `  ����F  BFlyleafLib, Version=3.7.20.0, Culture=neutral, PublicKeyToken=nullU9clr-namespace:FlyleafLib.Controls.WPF;assembly=FlyleafLibFlyleafLib.Controls.WPF  5   	   [ WPresentationTrainerVisualization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:PresentationTrainerVisualization PresentationTrainerVisualization 5   	   8   2PresentationTrainerVisualization.VideoPlayerWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.61.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aae aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35J 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Hfl9clr-namespace:FlyleafLib.Controls.WPF;assembly=FlyleafLib     5   	   _	 [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5   	   :local.clr-namespace:PresentationTrainerVisualization 	 5   	   -   5          �Name$  VideoPlayerWindowElemennt��5
   &    � Title$ NewVideoPlayerWindow��6	   $	��450��6I   $	��800��6V    � WindowState$ 	Maximized=�5   	   .��5      � 
���   � � RowDefinitions 5   
   �� 5      $��7* �6   �� 5      $
��1.5* �6   �� 5      $��1* �6   .+�5   
   )   #FlyleafLib.Controls.WPF.FlyleafHost     AttachedDragMove$
 Both=�68      KeyBindings$
 Both=�6P   $
��Auto��6c   $
��Auto��6p   $
��1200��6~      Player 6   �� *VideoPlayer+.��5      � .+�5      � $��Right=�6   $	��Top=�63   .+�5      �� -   5         ��Name$ ListBoxActions��5       ,  (MaterialDesignNavigationSecondaryListBox#��[  66   $	��200��6�   $	��800��6�   
�������$y�Hidden=�6�   ��5      �� 5      .R�5   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5   &   �� 
���  ��$��16��6>   $��16��6I   $��0,2,5,2q�6U   y�6g   �� 	 �� Path$	 Mistake�  BoolToVisibility
 �� 	Converter#
 [ �� 5   &   
��� � �$��16��6=   $��16��6H   $��0,2,5,2q�6T   y�6f   �� $	 Mistake�  InverseBoolToVisConverter#
 [ �� 5   &   $
��Bold"�61   ��6C   �� *LogActionDisplay+5   #   5       5"      �� 5$      -   5        $ ListBoxSentences��5$      #��[  68   $	��200��6�   $	��800��6�   
�������$y�Hidden=�6�    � 	Resources 5%       �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value��%  '��       �� 5&      " ��6$   .�5'   "   ��  � ��
 �����6?    5)   "   P� 5*   &    �@��
IsSelected$
 True��6D   . �5+   *   ��  � ��
 ��ح�6G   5-   '   P� 5.   &    � {�$ False��6C   . �5/   *   ��  � ��
 ��ح�6G   51   '   53      54      ��55      �� 56      .R�57   "   �� $ 
Horizontal=�6-   .+�58   &   �� 
���  ��$��16��6>   $��16��6I   $��0,2,5,2q�6U   y�6g   �� $	 WasIdentified�#
 [ �� 59   &   
��� � �$��16��6=   $��16��6H   $��0,2,5,2q�6T   y�6f   �� $	 WasIdentified�#
 [ �� 5:   &   $
��Bold"�60   ��6B   �� *SentenceText+ ^� ToolTip 6a   �� *SentenceText+5;   #   5<      5>      5?      �� 5A      -   5         ��Name$ subtitleContainer��5A      $��Center=�65   $��Bottom=�6R   ���	#80000000 �� Padding$ 10q�6�   ._�5B      �� .+�5C      �� -   5         ��Name$ VideoSubTitle��5C   $   $��18(�6;   
�������$��Bottom=�6\   $��Center=�6w   �� 5D      -   5        $ VideoSubTime��5D   $   $��18(�6:   
�������$��Bottom=�6[   $��Center=�6v   5E      5F      5H      5I      �� 5K   
   $��1��6   .+�5L      �� $��10q�6   $
��Auto��6!    �� IsMoveToPointEnabled$
 True��6}    � Maximum 6.   �� *VideoPlayer.Duration+ � Value 6W   �� *VideoPlayer.CurTime+ � SmallChange 6�   �� *+)VideoPlayer.Config.VideoPlayer.SeekOffset+ � LargeChange 6�   �� *,*VideoPlayer.Config.VideoPlayer.SeekOffset2+�� 5M      $ 
Horizontal=�6   $��Center=�62   .+�5N      �� -   5        $ CurrentTime��5N      
�������00:006M   �� 5O      
�������/63   �� 5P      -   5        $ Duration��5P      
�������6/   5Q      5R      �� 5T   
   $��Center=�6   $
��Auto��62   $ 
Horizontal=�6?   $��Center=�6X   $��2��6s   .+�5U      �� -	   5        $��5q�5U   5   .��5V      ' 	 !MaterialDesignThemes.Wpf.PackIcon  -
   5          Name$ playButtonIcon��5V   *      Kind$ Pause=�6B   6Q   5W      �� 5X      -   5         � ToolTip$ Jump to previous Action��5X      $��5q�6V   .��5Y        $ ArrowLeftBold=�6*   6A   5Z      �� 5[      -   5        $ Jump to next Action��5[      $��5q�6T   .��5\        $ ArrowRightBold=�6*   6B   5]      �� 5^      -   5         ��Name$ 
LoopButton��5^   4   $ Loop selected Action��6   $��5q�6b   .��5_        $
 Loop=�6*   68   5`      �� 5a      $ 
Speed Down��6   $��	100,5,5,5q�6^   ��6+   �� * VideoPlayer.Commands.SpeedDown+.��5b        $ 
FastRewind=�6*   6>   5c      �� 5d      $ Speed Up��6   $��5q�6Z   ��6)   �� *VideoPlayer.Commands.SpeedUp+.��5e        $ FastForward=�6*   6?   5f      5g      5h      5i      