   M S B A M L   `   `   `  ����   MediCloudDrive'    !MediCloudDrive.Views.MainListView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  R NMahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  .���� -     ��Name$  baseControl������ *	ListItems+ �� SelectedItem �� *SelectedItem+ � ContextMenu �� .@�m� $��파일��.@�m� $��파일 열기�� m� Command �� *CmdOpenFile+m� $��폴더 열기�� �� *CmdOpenFolder+m� $��항목��.@�m� $ ��해당 항목 강제정리�� �� *CmdClearItem+����  �� 
TargetType" m�   MetroMenuItem �� BasedOn# [   �� Triggers ��  �� Binding �� *SelectedItem+ �� Value null.a��� 	 �� Property	 � {�
 �� Value
 .  m� $ ��전체 완료항목 정리�� �� *CmdClearAll+��5  -System.Windows.Controls.ItemContainerTemplate�  .R�� $��45��$	��1,1q� � RowDefinitions �� �� �� $��1 � � ColumnDefinitions �� $��30 ��� �� $��30 ��� $��60 ��� $��70 �.+�.  (MahApps.Metro.IconPacks.PackIconMaterial  $��Center=�$��Right=�$��2������ "   ��  �� *	FileExt+ PDF.a��� E AControlzEx, Version=3.0.2.4, Culture=neutral, PublicKeyToken=null�  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null]]	 �@ Kind$
 FilePdf=��� 	 � ��
 �	#FFDD4848�� 	 � ��$
 20���� 	 � ��$
 20����  �� *	FileExt+ IMG.a��� 	 �@ Kind$
 Image=��� 	 � ��

 ��  ��� 	 � ��$
 18���� 	 � ��$
 18���� $��1��$��CharacterEllipsis=�$��Bottom=�$	��5,0q����� *
FileName+��  �� Orientation$ 
Horizontal=�$��1��$��1��$��10,0,0,0q�.+�     Kind$ 
FolderOpen=�$��Center=�$��Center=�$��15��$��15��
��������� $��11(�
�������$��Center=�$��3,0,0,0q�$��CharacterEllipsis=����� *
ForderPath+)  #MahApps.Metro.Controls.ProgressRing  $��25��$��25��$��2��$��2��   IsActive �� *	IsSending+ �� UpdateSourceTrigger$ PropertyChanged=��� $��0��$��3��$��Right=�$��Bottom=�$��0,0,10,0q�$��11(����� " �� ��  �� *SendStatusEnum+	 Ready.a��� 	 � ��$
 전송준비���� 	 � ��

 � � ���  �� *SendStatusEnum+ Fail.a��� 	 � ��$
 전송실패���� 	 � ��

 �  ����  �� *SendStatusEnum+ Complete.a��� 	 � ��$
 전송완료���� 	 � ��

 ��  ���  �� *SendStatusEnum+ Sending.a��� 	 � ��$
 	전송중���� 	 � ��

 � � ��� $��1��$��3��$��Right=�$��0,0,10,0q�
����������� *
FileSize+�� $��0��$��4��$��Bottom=�
����������� *ListAddTime+ �� StringFormat$ {0:yyyy/MM/dd}���� $��1��$��4��
����������� *ListAddTime+$ {0:hh:mm:ss}���� 
�������$	��5,0q�$��2��$��5��