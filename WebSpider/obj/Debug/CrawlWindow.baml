   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   
   D @WebSpider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:WebSpider	WebSpider 5   	      WebSpider.CrawlWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   
   )localclr-namespace:WebSpider     5   	   -   5          � Title$  CrawlWindow��5	   	   $��370.747��6   $��416.326��6.   $	��416��6>   $	��370��6M   
����=H�$��1q�5
   %   .��5      � -   5         �Name$ MainGrid��5      .+�5   
   �� -   5         ��Name$ StartLinkTextBox��5      $��34��6,   $��130,10,10,0q�68    �� TextWrapping$
 Wrap=�6M   $	��Top=�6a   $	��268��6y   $��Right=�6�   �� 5   
   -   5         ��Name$ 	textBlock��5      $
��Left=�6'   $��	10,17,0,0q�6B   $%��Начальная ссылка��6U   $	��Top=�6m   $��19��6�   $	��105��6�   �� 5   
   -   5         ��Name$ StartCrawlbutton��5      $��Crawl��6+   $��Right=�6;   $��
0,49,186,0q�6W   $	��Top=�6k   $��92��6�   $��25��6�   6�   �� 5   
   -   5        $ textBlock_Copy��5      $
��Left=�6,   $��	10,53,0,0q�6G   $��Глубина��6Z   $	��Top=�6i   $��18��6�   $��49��6�   *   $MahApps.Metro.Controls.NumericUpDown  5   
   -   5          Name$ DepthTextBox��5   !      Interval$ 1Q�67      Minimum$ 0Q�6D   	   Maximum$	 10Q�6P   $
��Left=�6_   $��25��6z   $��	64,48,0,0q�6�   $	��Top=�6�   $��61��6�   �� 5   
   -   5        $ StartCrawlbutton_Copy��5      $��Crawl from last��60   $��Right=�6J   $��	0,49,10,0q�6f   $	��Top=�6y   $	��171��6�   $��25��6�   6�   �� 5   
   -	   5        
 ��Name$
 ThreadslistBox��5      $��15,79,10,48q�6B   ��6*   �� 6B   5      5      