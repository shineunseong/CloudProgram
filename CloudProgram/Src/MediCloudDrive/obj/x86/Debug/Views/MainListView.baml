   M S B A M L   `   `   `  ����  MediCloudDrive'    !MediCloudDrive.Views.MainListView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    61   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6<   @d2http://schemas.microsoft.com/expression/blend/2008     5      H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  6B   R NMahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5      .��5      �� -   5          ��Name$  baseControl��5      ��6$   �� *	ListItems+ �� SelectedItem 6F   �� *SelectedItem+ � ContextMenu 5   
   �� 5	      .@�5
      m� $��파일��6   .@�5      m� $��파일 열기��6    m� Command 6.   �� *CmdOpenFile+m� 5      $��폴더 열기��6    6.   �� *CmdOpenFolder+5      m� 5      $��항목��6   .@�5      m� $ ��해당 항목 강제정리��6    63   �� *CmdClearItem+��5      �� 5       �� 
TargetType" m�6$      MetroMenuItem �� BasedOn# [  6:    �� Triggers 5   "   �� 5   &    �� Binding 62   �� *SelectedItem+ �� Value null6S   .a�5   *   �� 	 �� Property	 � {�
 �� Value
 .  6F   5   '   5      5      m� 5      $ ��전체 완료항목 정리��6    63   �� *CmdClearAll+5      5      ��5   
   5  -System.Windows.Controls.ItemContainerTemplate�  5      .R�5       � $��45��6   $	��1,1q�6#    � RowDefinitions 5!      �� 5"      �� 5#      �� 5$      $��1 �6(    � ColumnDefinitions 5&      �� 5'      $��30 �6+   �� 5(      �� 5)      $��30 �6+   �� 5*      $��60 �6+   �� 5+      $��70 �6+   .+�50      .  (MahApps.Metro.IconPacks.PackIconMaterial  $��Center=�51   !   $��Right=�6<   $��2��6X   ��52      �� 53      "  6$    54   "   �� 55   &    62   �� *	FileExt+ PDF6N   .a�56   *   �� E AControlzEx, Version=3.0.2.4, Culture=neutral, PublicKeyToken=null�  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null]]	 �@ Kind$
 FilePdf=�6A   �� 57   *   	 � ��
 �	#FFDD48486G   �� 58   *   	 � ��$
 20��6C   �� 59   *   	 � ��$
 20��6B   5:   '   �� 5;   &    62   �� *	FileExt+ IMG6N   .a�5<   *   �� 	 �@ Kind$
 Image=�6A   �� 5=   *   	 � ��

 ��  �6G   �� 5>   *   	 � ��$
 18��6C   �� 5?   *   	 � ��$
 18��6B   5@   '   5B      5D      �� 5H      $��1��6:   $��CharacterEllipsis=�6J   $��Bottom=�6k   $	��5,0q�6�   ��6    �� *
FileName+6�   �� 5I       �� Orientation$ 
Horizontal=�6!   $��1��6:   $��1��6J   $��10,0,0,0q�6W   .+�5J           Kind$ 
FolderOpen=�5K   %   $��Center=�67   $��Center=�6R   $��15��6o   $��15��6{   
�������6�   �� 5L      $��11(�6@   
�������$��Center=�6a   $��3,0,0,0q�6|   $��CharacterEllipsis=�6�   ��6$   �� *
ForderPath+6�   5M      )  #MahApps.Metro.Controls.ProgressRing  5P      $��25��6p   $��25��6{   $��2��6�   $��2��6�      IsActive 6,   �� *	IsSending+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5S      $��0��6    $��3��6-   $��Right=�6=   $��Bottom=�6Y   $��0,0,10,0q�6t   $��11(�6�   ��5T      �� 5U      " ��6$    5V   "   �� 5W   &    62   �� *SendStatusEnum+	 Ready6U   .a�5X   *   �� 	 � ��$
 전송준비��6A   �� 5Y   *   	 � ��

 � � �6G   5Z   '   �� 5[   &    62   �� *SendStatusEnum+ Fail6U   .a�5\   *   �� 	 � ��$
 전송실패��6A   �� 5]   *   	 � ��

 �  ��6G   5^   '   �� 5_   &    62   �� *SendStatusEnum+ Complete6U   .a�5`   *   �� 	 � ��$
 전송완료��6A   �� 5a   *   	 � ��

 ��  �6G   5b   '   �� 5c   &    62   �� *SendStatusEnum+ Sending6U   .a�5d   *   �� 	 � ��$
 	전송중��6A   �� 5e   *   	 � ��

 � � �6G   5f   '   5h      5j      �� 5k      $��1��6:   $��3��6G   $��Right=�6W   $��0,0,10,0q�6s   
���������6    �� *
FileSize+6�   �� 5o      $��0��6]   $��4��6j   $��Bottom=�6z   
���������6    �� *ListAddTime+ �� StringFormat$ {0:yyyy/MM/dd}��6�   �� 5p      $��1��6[   $��4��6h   
���������6    �� *ListAddTime+$ {0:hh:mm:ss}��6x   �� 5r      
�������$	��5,0q�61   $��2��6>   $��5��6K   5s      5t      5v      5w      