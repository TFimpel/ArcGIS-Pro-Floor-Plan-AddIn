   M S B A M L   `   `   `  ����I  EFloorPlanAddIn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:FloorPlanAddInFloorPlanAddIn  5      a ]ArcGIS.Desktop.Extensions, Version=12.1.0.0, Culture=neutral, PublicKeyToken=8fc3cc631e44ad86hJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.ExtensionsArcGIS.Desktop.Extensions 5      "    FloorPlanAddIn.Dockpane2View   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +uiclr-namespace:FloorPlanAddIn     5      a
extensionsJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.Extensions     5        � 	Resources  5       �� MergedDictionaries<  6ArcGIS.Desktop.Extensions.DesignOnlyResourceDictionary   Source�� 5   
    5        5      $S Mpack://application:,,,/ArcGIS.Desktop.Framework;component\Themes\Default.xaml?�6:   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5       $��* �6   �� 5!      $��* �6   �� 5"      $��* �6   �� 5#      $��* �6   �� 5$      $��* �6   �� 5%      $��* �6    � ColumnDefinitions 5)   
   �� 5*      $
��Auto �6   .+�5-   
   �� $��0��6   $��0��6"   $/��)(1) Set Extent, Rotate Selected Map Frame��62    !  Esri_TextBlockDockPaneHeading#���   6c   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5.   
   $��1��6   $��5,0,0,0q�6#    �� Orientation$ 
Horizontal=�64   .+�5/      �� $��
Set Extent��6     Esri_Button#���  6]   $	��100��6�   ��6+   �� *CmdZoomAndCenterActiveLayout+6�   �� 50      -   5        $��Rotate 45°��50      #���  6>   $	��100��6e   51      �� 52   
   $��5,0,0,0q�6   $
��Left=�6%   $	��280��6@   $
��Wrap=�6L   $��2��6`   $��2��6n   $��0��6�   $z��tClick on the page layout's map frame, center the extent on the floor plan, then rotate it as needed to fit the page.��6�   $��11(�6    Esri_TextBlockRegular#���  6   �� TextAlignment$
 Left=�6L  6c  �� 56   
   $��3��6   $��0��6"   $��(2) Label Rooms��62   #���   6I   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 57   
   $��4��6   $��4��6'   $ Vertical=�64   $��5,5,5,5q�6K   $
��Left=�6]   .+�58      	� -   5        $
��None��58      
 � Tag$
 None��6F    d� 	IsChecked$
 TrueY�6Q   	� 59      -   5        $��Room number��59      $ $feature.RMNUMB��6M   $ FalseY�6c   	� 5:      -   5        $��	Room area��5:      $ $feature.RMAREA��6K   $ FalseY�6a   	� 5;      -   5        $��Room number and area��5;      $B <$feature.RMNUMB +  TextFormatting.NewLine  + $feature.RMAREA��6V   $ FalseY�6�   5<      �� 5>   
   $��6��6   $��0��6"   $��(3) Color Code Rooms��62   #���   6N   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5?   
   $��7��6   $��5,5,5,5q�6"   $��3��63   $ Vertical=�6D   $
��Left=�6[   .+�5@      	� -   5        $��All Transparent��5@      $
 None��6S   $ FalseY�6^   	� 5A      -   5        $��	All Beige��5A      $ Beige��6M   $ FalseY�6Y   	� 5B      -   5        $#��Color Code based on Use Group��5B      $ Multicolor1��6a   $ FalseY�6s   	� 5C      -	   5        $$��Color Code based Function Code��5C      $ Multicolor1��6b   $ FalseY�6t   	� 5D      -
   5        $(��"Color Code based on Group/Dept. ID��5D      $ Multicolor1��6f   $ FalseY�6x   5E      �� 5G   
   $��10��6   $��0��6#   $��(4) Change Line Color��63   #���   6P   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5H   
   $��5,5,5,5q�6   $��1��6'   $��11��68   $��0��6F   $��2��6V   $ 
Horizontal=�6j   $
��Left=�6�   .+�5I      	� -   5        $��5,0,10,0q�5I      $
��None��6,   $
 None��6Z   $ FalseY�6e   	� 5J      -   5        $��5,0,10,0q�5J      $
��Gray��6,   $
 Gray��6Z   $ FalseY�6e   	� 5K      -   5        $��5,0,10,0q�5K      $��Mutlicolor 1��6,   $ 
Multicolor��6b   $ FalseY�6s   5L      �� 5N   
   $��12��6   $��0��6#   $ ��(5) Adjust Reference Scale��63   #���   6U   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5O   
   $��5,0,0,0q�6   $��13��6'   $ 
Horizontal=�65   .+�5P      �� -   5        $��Decrease��5P      $
 down��6Q   {�. $��70��6m   $��29��6y   $��Center=�6�   ���#FFFFFF
���   �$	��0.8q�6�   6�   �� 5Q      -   5        $��Reset��5Q      $ reset��6J   {�. $��50��6h   $��29��6t   $��Center=�6�   ���#FFFFFF
���   �$	��0.8q�6�   6�   �� 5R      -   5        $��Increase��5R      $ up��6P   {�. $��70��6j   $��29��6v   ���#FFFFFF
���   �$	��0.8q�6�   6�   5S      �� 5T   
   $��5,0,0,0q�6   $
��Left=�6%   $	��280��6@   $��1��6L   $
��Wrap=�6]   $��14��6q   $��2��6�   $��0��6�   $v��pDecrease: lines appear thinner and labels appear smaller. Increase: lines appear wider and labels appear larger.��6�   $��11(�6  #���  6*  $
 Left=�6Z  6q  �� 5W   
   $��15��6   $��0��6#   $��(6) Export to .PDF File��63   #���   6R   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5X   
   -   5        $��5,5,5,0q�5X      $
��Left=�6"   $	��100��6=   $��16��6I   $��0��6W   $��Export to .pdf��6h   #���  6�   $��2��6�   �� 5Y   
   $��5,5,0,5q�6   $
��Left=�6%   $	��280��6@   $
��Wrap=�6L   $��17��6`   $��2��6o   $��0��6�   $B��<File will be saved in the current ArcGIS Pro Project folder.��6�   $��11(�6�   #���  6�   $
 Left=�6  6,  5[      5\      