   M S B A M L   `   `   `  ����[  WPresentationTrainerVisualization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullc6clr-namespace:PresentationTrainerVisualization.windows(PresentationTrainerVisualization.windows  5   	   C    =PresentationTrainerVisualization.windows.VideoPlayerWindowOld   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.61.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aae aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35J 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   Blocal6clr-namespace:PresentationTrainerVisualization.windows  5   	   -   5          �Name$  VideoPlayerWindowElement��5	   =    � Title$ VideoPlayerWindow��6	   $	��800��6#   $
��1200��60   .��5      � 
���   � � RowDefinitions 5   
   �� 5      $��5* �6   �� 5      $��1* �6   .+�5   
   r� -   5         r�Name$ VideoPlayer��5       r� Source$d ^C:\\Users\\vedat\\OneDrive\\Desktop\\BachelorNeu\\testdata\\neueDaten\\Downloads\\5344641c.mp4?�6   $
��Auto��5      $
��Auto��6*   $
��1200��68    r� LoadedBehavior$ Manual=�6H    r� UnloadedBehavior$ Manual=�6`    r� Stretch$
 Fill=�6z   6�   � 5   
   $��Right=�6   $	��Top=�6+    5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��0.2* �6    .+�5      �� -   5         ��Name$ ListBoxActions��5      $	��200��6O   $	��800��6[   
�������y�6   �� 	 �� Path$	 IsActionVideo�
 �� ElementName$
 VideoPlayerWindowElement��   BoolToVisibility �� 	Converter# [  ��5      �� 5      .R�5       ��  �� Orientation$ 
Horizontal=�6%   .+�5!      �� 
���  ��$��16��63   $��16��6>   $��0,2,5,2q�6J   y�6\   �� $	 Mistake�# [  �� 5"      
��� � �$��16��65   $��16��6@   $��0,2,5,2q�6L   y�6^   �� $	 Mistake�  InverseBoolToVisConverter# [ �� 5#      $
��Bold"�6)   ��6;   �� *LogActionDisplay+5$      5%      5'      �� 5)      -   5        $ ListBoxSentences��5)      $	��200��6Q   $	��800��6]   
�������y�6�   �� $	 IsActionVideo�$
 VideoPlayerWindowElement��# [ ��5*      �� 5+      .R�5,      �� $ 
Horizontal=�6%   .+�5-      �� 
���  ��$��16��63   $��16��6>   $��0,2,5,2q�6J   y�6\   �� $	 WasIdentified�# [ �� 5.      
��� � �$��16��65   $��16��6@   $��0,2,5,2q�6L   y�6^   �� $	 WasIdentified�# [  �� 5/      $
��Bold"�6)   ��6;   �� *SentenceText+50      51      53      54      �� 57   
   $��Center=�6   $	��450��62   $ 
Horizontal=�6>   $��1��6W   .+�58      �� -   5        $��5q�58   5   .��59      '  !MaterialDesignThemes.Wpf.PackIcon  -   5          Name$ playButtonIcon��59   *      Kind$ Pause=�6B   6Q   5:      �� 5;      -   5        $��5q�5;   /   .��5<        $ ArrowLeftBold=�6*   6A   5=      �� 5>      -   5        $��5q�5>   1   .��5?        $ ArrowRightBold=�6*   6B   5@      �� 5A      -	   5         ��Name$ 
LoopButton��5A      $��5q�6C   .��5B        $
 Loop=�6*   68   5C      �� 5E      
�������$��Center=�6+   $��5q�6F   Volume6[   �� 5F      -
   5        $��VolumeSlider��5F      $��Center=�6)    � Minimum$ 0Q�6e    � Maximum$ 1Q�6q    � Value$	 0.5Q�6}   $��70��6�   �� 5G      
�������$��5q�6+   $��Center=�67   Speed6Y   �� 5H      -   5        $��speedRatioSlider��5H      $��Center=�6-   $ 1Q�6m   $��70��6w   5I      5J      5K      