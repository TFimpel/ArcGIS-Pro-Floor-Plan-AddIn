   M S B A M L   `   `   `  ����I  EFloorPlanAddIn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:FloorPlanAddInFloorPlanAddIn  5      a ]ArcGIS.Desktop.Extensions, Version=12.1.0.0, Culture=neutral, PublicKeyToken=8fc3cc631e44ad86hJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.ExtensionsArcGIS.Desktop.Extensions 5      "    FloorPlanAddIn.Dockpane1View   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +uiclr-namespace:FloorPlanAddIn     5      a
extensionsJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.Extensions     5        � 	Resources  5       �� MergedDictionaries<  6ArcGIS.Desktop.Extensions.DesignOnlyResourceDictionary   Source�� 5   
    5        5      $S Mpack://application:,,,/ArcGIS.Desktop.Framework;component\Themes\Default.xaml?�6:   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5       $��* �6   �� 5!      $��* �6   �� 5"      $��* �6   �� 5#      $��* �6   �� 5$      $��* �6   �� 5%      $��* �6   �� 5&      $��* �6    � ColumnDefinitions 5)   
   �� 5*      $	��150 �6   �� 5+      $��* �6   .+�5.   
   �� $��0��6   $��0��6"   $��(1) Filter by Site/Campus��62    !  Esri_TextBlockDockPaneHeading#���   6S   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5/   
   -   5        $��SiteListBox��5/   /   $��1��6   $��2��6    �� SelectionMode$ Multiple=�6a   $��5,5,0,0q�6z    %� IsVirtualizing .  $��2��6�   $��2��6�   ��6B   �� *Sites+��50      �� 51       �� 
TargetType" ��6   .�52      ��  �� Property �@��
IsSelected	 �� Value	 63   �� 
 �� Mode$
 TwoWay=� �� Path$ 
IsSelected� �� NotifyOnTargetUpdated . 53      ��55      �� 56      .R�57      �� ��6    �� *Site+58      5:      �� 5;   
   $��1��6   $��0��6   $��Load Options��6/     Esri_Button#���  6n   $��1��6�   ��6F   �� *CmdRefreshSiteData+6�   �� 5<   
   $��2��6   $��0��6   $��Select all Options��6.   #���  6q   $��1��6�   ��6K   �� *CmdSelectAllSites+6�   �� 5>   
   $��3��6   $��0��6#   $��(2) Filter by Building��63   #���   6Q   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5?   
   -   5        $��BuildingListBox��5?   /   $��4��6   $��1��6    �� SelectedValuePath$ 
IsSelected��6h   $ Multiple=�6�   $��5,5,0,0q�6�    .  $��3��6�   $��2��6�   ��6F   �� *	Buildings+��5@      �� 5A      " ��6   .�5B      ��  �@��
IsSelected	 63   �� $
 TwoWay=�$ 
IsSelected�5C      ��5E      �� 5F      .R�5G      �� ��6    �� *
Building+5H      5J      �� 5L   
   $��4��6   $��0��6    $��5,5,5,5q�60   $��Bottom=�6c   $��Bottom=�6�   $
��Left=�6�   $
��Left=�6�    d� 	IsChecked 6A   �� *BuildingChb+.��5M      �� $-��'Load filtered on options selected above��6   $
��Wrap=�6G     Esri_TextBlockRegular#���  6]   $��11(�6�   5N      �� 5P   
   $��5��6   $��0��6   $��Load Options��6.   #���  6q   $��1��6�   ��6E   �� *CmdRefreshBuildingData+6�   �� 5Q   
   $��6��6   $��0��6   $��Select all Options��6.   #���  6u   $��1��6�   ��6K   �� *CmdSelectAllBuildings+6�   �� 5S   
   $��7��6   $��0��6#   $��(3) Filter by Floor��63   #���   6N   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5T   
   -   5        $��FloorListBox��5T   /   $��8��6   $��1��6   $ Multiple=�6c   $��5,5,0,0q�6|    .  $��3��6�   $��2��6�   ��6C   �� *Floors+��5U      �� 5V      " ��6   .�5W      ��  �@��
IsSelected	 63   �� $
 TwoWay=�$ 
IsSelected�5Y      ��5[      �� 5\      .R�5]      �� ��6    �� *Floor+5^      5`      �� 5a   
   $��10��6   $��0��6   $��Select all Options��6/   #���  6s   $��1��6�   ��6L   �� *CmdSelectAllFloors+6�   �� 5b   
   $��8��6   $��0��6    $��5,5,5,5q�60   $��Bottom=�6`   $��Bottom=�6�   $
��Left=�6�   $
��Left=�6�    6A   �� *
FloorChb+.��5c      �� $-��'Load filtered on options selected above��6   $
��Wrap=�6G   #���  6]   $��11(�6�   5d      �� 5e   
   $��9��6   $��0��6   $��Load Options��6.   #���  6m   $��1��6�   ��6E   �� *CmdRefreshFloorData+6�   �� 5g   
   $��11��6   $��0��6$   $��(4) Filter by Group ID��64   #���   6R   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5h   
   -   5        $��GroupIDListBox��5h   0   $��12��6   $��1��6    $ Multiple=�6h   $��5,5,0,0q�6�    .  $��3��6�   $��2��6�   ��6F   �� *
GroupIDs+��5i      �� 5j      " ��6   .�5k      ��  �@��
IsSelected	 67   �� $
 TwoWay=�$ 
IsSelected�5l      ��5n      �� 5o      .R�5p      �� ��6$   �� *	GroupID+5q      5s      �� 5t   
   $��14��6   $��0��6   $��Select all Options��6/   #���  6u   $��1��6�   ��6L   �� *CmdSelectAllGroupIDs+6�   �� 5u   
   $��12��6   $��0��6"   $��5,5,5,5q�62   $��Bottom=�6e   $��Bottom=�6�   $
��Left=�6�   $
��Left=�6�    6D   �� *
GroupIDChb+.��5v      �� $-��'Load filtered on options selected above��6   $
��Wrap=�6G   #���  6]   $��11(�6�   5w      �� 5x   
   $��13��6   $��0��6   $��Load Options��6/   #���  6p   $��1��6�   ��6F   �� *CmdRefreshGroupIDData+6�   �� 5z   
   $��15��6   $��0��6$   $+��%(5) Validate Query and Create Layouts��64   #���   6a   $��Bottom=�6�   $��5,5,0,0q�6�   $��2��6�   6�   �� 5}   
    �� Orientation$ 
Horizontal=�6   $��16��6.   $��0��6<   $��2��6L   .+�5~      �� $��Validate Query��6   #���  6O   $	��100��6v   ��6/   �� *CmdValidate+6v   �� 5      $��Create Layout��6   #���  6W   $	��100��6~   ��6-   �� *CmdCreateMapsLiveData+6~   5�      �� 5�   
   $��5,0,0,0q�6   $
��Left=�6%   $
��Wrap=�6@   $��17��6T   $��2��6c   $��0��6w   $7��1Repeat steps 1-5 to query additional floor plans.��6�   $��11(�6�   #���  6�    �� TextAlignment$
 Left=�6�   6  �� 5�   
   $��5,0,25,0q�6   $��Center=�6&   $��Right=�6A   $��18��6]   $��2��6l   $��0��6�   $%��Style Layout and Export to PDF ��6�   $��14(�6�   #���  6�   $
 Left=�6�   6  �� 5�   
   -   5        $��5,5,5,5q�5�      $��Center=�6"   $��Right=�6>   $��18��6Z   $��2��6h   $��Open styler��6y     Esri_ButtonForwardSmall#���  6�   $��2��6�   5�      5�      