   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   D @WebSpider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:WebSpider	WebSpider 5   	      WebSpider.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   )localclr-namespace:WebSpider     5   	   -   5          � Title$  
MainWindow��5	   	   $	��557��6   $	��722��6)   
����=H�$��1q�5
   %   (   "MahApps.Metro.Controls.MetroWindow   SaveWindowPosition .    RightWindowCommands 5      +   %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5         ��Name$ openCrawlWindowButton��5      $��Crawling settings��64   5      5       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Resources/Icons.xaml?�6%   %    5      .��5      � .+�5   
   �� -   5         ��Name$ SearchQuerytextBox��5      $��Stretch=�6.   $��23��6L   $��
90,10,81,0q�6X    �� TextWrapping$
 Wrap=�6l   $4��.Введите поисковый запрос��6�   $	��Top=�6�   6�   �� 5   
   -   5        	 ��Name$	 SearchResultslistBox��5      $��10,68,10,10q�61   
 K� ZIndex$
 1��6^   
������ ��6F   �� ��5      �� 5        �� DataType$ String��6   .R�5!      �� .+�5"      �� ��6$   ��  �� Path$ 	PageTitle�5#      �� 5$      �� 5%      -   5        ��5%      �� .�6V   .�5&      �� ��6    �� 5'      5(      5)      5*      5,      )   #MahApps.Metro.Controls.ProgressRing  5-   
   -   5          Name$ SearchProgressRing��5-          IsActive .  J� Opacity$ 1Q�6L   $y�Visible=�6X   $
 2��6m   �� 5.   
   -   5        $ SearchButton��5.      $��Искать��6'   $��Right=�68   $��	0,10,10,0q�6T   $	��Top=�6g   $��66��6   $��26��6�   � 5/   
   $��
10,38,10,0q�6   $	��Top=�6#   $��25��6;   .+�50      	� -   5         	�Name$  FrequencySearchradioButton��50      $��По частоте��6>   $
��Left=�6S   	� 51      -	   5        $ SimpleSearchradioButton��51      $��По вхождению��6<   $
��Left=�6S   $��95,0,0,0q�6n   	� 52      -
   5        $ LocationSearchradioButton��52      $#��По расположению��6>    d� 	IsChecked$
 TrueY�6X   $
��Left=�6i   $��	207,0,0,0q�6�   �� 53      -   5         ��Name$ ExecutionTimetextBlock��53      $��0,0,0,0q�68   $
��Wrap=�6I   $�� ��6]   
�������$��Bottom=�6w   $��Right=�6�   $��20��6�   54      �� 56   
   -   5        $ 
Undobutton��56      $
��Left=�6%   $��0,2,0,0q�6@   $	��Top=�6Q   $��40��6i   $��40��6u      MetroCircleButtonStyle#���   6�   .��57      �� $��20��6   $��20��6$   
���   � J� OpacityMask 58      !� 59        appbar_arrow_left !� Visual# [ 6"    j� Stretch$
 Fill=�6N   5;      5<      �� 5>   
   -   5        $ 
Redobutton��5>      $
��Left=�6%   $��40,2,0,0q�6@   $	��Top=�6R   $��40��6j   $��40��6v   #���   6�   .��5?      �� $��20��6   $��20��6$   
���   � 5@      !� 5A        appbar_arrow_right# [ 6"   $
 Fill=�6O   5C      5D      5E      5F      