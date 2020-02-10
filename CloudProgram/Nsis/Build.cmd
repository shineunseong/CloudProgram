@ECHO Off

ECHO  -----------------------------------------------------------------
ECHO  Visual Studio Variables Setting(%3)
ECHO  -----------------------------------------------------------------

	CALL %1

ECHO  -----------------------------------------------------------------
ECHO  Medi-Cloud Drive Solution CLEAN(%3)
ECHO  -----------------------------------------------------------------

	DEVENV %2 /CLEAN %3

ECHO  -----------------------------------------------------------------
ECHO  Medi-Cloud Drive Solution BUILD(%3)
ECHO  -----------------------------------------------------------------

	DEVENV %2 /BUILD %3

ECHO  -----------------------------------------------------------------
ECHO  Medi-Cloud Drive Solution BUILD Complete %2(%3)
ECHO  -----------------------------------------------------------------

ECHO  -----------------------------------------------------------------
ECHO  Medi-Cloud Drive Project File Copy(%3)
ECHO  -----------------------------------------------------------------

	XCOPY "%cd%\..\Bin\%3\*.*" "%cd%\bin\*.*" /Y /S /E /Q
						
	
	DEL /F /Q "%cd%\bin\*.pdb"
	DEL /F /Q "%cd%\bin\*.xml"		
	DEL /F /Q "%cd%\bin\*.vshost.*"	
		
	
ECHO  -----------------------------------------------------------------
ECHO  Medi-Cloud Drive Project File Copy Complete(%3)
ECHO  -----------------------------------------------------------------