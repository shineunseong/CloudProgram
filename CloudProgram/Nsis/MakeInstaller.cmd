@ECHO Off
Set BuildMode=Debug
::Release / Debug 공백없이 둘중에 하나로 설정
	
	RMDIR ..\bin\"%BuildMode%" /s /q
	
	RMDIR bin /s /q
	
	call build.cmd "%vs160comntools%\VsMSBuildCmd.bat" "..\MediCloudDrive.sln" "%BuildMode%"	

ECHO  -----------------------------------------------------------------
ECHO  Medi-Cloud Drive Packaging Start
ECHO  -----------------------------------------------------------------

	call package.cmd 
			
	::RMDIR ..\bin\"%BuildMode%" /s /q
		
	::RMDIR bin /s /q
		
ECHO  -----------------------------------------------------------------
ECHO  Medi-Cloud Drive Packaging Complete
ECHO  -----------------------------------------------------------------