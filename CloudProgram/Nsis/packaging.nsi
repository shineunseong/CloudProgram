Var isAllUser
Unicode True

# Text Settings
!define COMPANY_NAME                    "MediAge"
!define PRODUCT_NAME                    "ReportCloudDrive"                          # Name
!define MAJOR_VERSION                   "1"                              # Version
!define MINOR_VERSION                   "0"                              # Version
!define APPLICATION_NAME				"${PRODUCT_NAME}"
!define PRODUCT_VOLUME                  "${PRODUCT_NAME} ${MAJOR_VERSION}.${MINOR_VERSION}"
!define APP_EXENAME                     "MediAge - Report Cloud Drive"
!define EXE_NAME                     	"MediCloudDrive"
!define REG_STARTUP_NAME				"mediclouddrv"
!define INSTALL_MODE                    "Bin"

!define PRODUCT_REG_KEY                 "Software\${COMPANY_NAME}\${PRODUCT_NAME}"
!define ALREADY_START_INSTALL_ERR_MSG   "${PRODUCT_NAME} is running.$\r$\nPlease close ${PRODUCT_NAME} and install again."
!define ALREADY_START_UNINSTALL_ERR_MSG "${PRODUCT_NAME} is running.$\r$\nPlease close ${PRODUCT_NAME} and uninstall again."

!define /date MyTIMESTAMP 				"%Y-%m-%d-%H-%M-%S"
!define TXT_SECTION_LAUNCHWHENSYSTEMRUN "Setting Startup"

BrandingText                            "Copyright 2011-2019 ${COMPANY_NAME}. All Rights reserved."
Name                                    "${APP_EXENAME}"
OutFile                                 "..\Bin\${PRODUCT_NAME}_${MyTIMESTAMP}.exe"
InstallDir                              "$PROGRAMFILES\${COMPANY_NAME}\${PRODUCT_NAME}"

SetCompress                             auto
SetCompressor                           lzma
ShowInstDetails                         show
ShowUnInstDetails                       show

!include "LogicLib.nsh"
!include "MUI.nsh"
!include "MUI2.nsh"

# MUI Settings

!define MUI_ABORTWARNING
!define MUI_ICON 						"Bin\Mediage.ico"
!define MUI_UNICON                      "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall-colorful.ico"

!define SEC_STARTUP

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
; !insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE               "Korean"

Var GetInstalledSize.total

Function GetInstalledSize
	Push $0
	Push $1
	StrCpy $GetInstalledSize.total 0
	${ForEach} $1 0 256 + 1
		${if} ${SectionIsSelected} $1
			SectionGetSize $1 $0
			IntOp $GetInstalledSize.total $GetInstalledSize.total + $0
		${Endif}
 
		; Error flag is set when an out-of-bound section is referenced
		${if} ${errors}
			${break}
		${Endif}
	${Next}
 
	ClearErrors
	Pop $1
	Pop $0
	IntFmt $GetInstalledSize.total "0x%08X" $GetInstalledSize.total
	Push $GetInstalledSize.total
FunctionEnd

Function CheckAndDownloadDotNet45
# Let's see if the user has the .NET Framework 4.5 installed on their system or not
# Remember: you need Vista SP2 or 7 SP1.  It is built in to Windows 8, and not needed
# In case you're wondering, running this code on Windows 8 will correctly return is_equal
# or is_greater (maybe Microsoft releases .NET 4.5 SP1 for example)

# Set up our Variables
Var /GLOBAL dotNET45IsThere
Var /GLOBAL dotNET_CMD_LINE
Var /GLOBAL EXIT_CODE

ReadRegDWORD $dotNET45IsThere HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
IntCmp $dotNET45IsThere 378389 is_equal is_less is_greater

is_equal:
    Goto done_compare_not_needed
is_greater:
    # Useful if, for example, Microsoft releases .NET 4.5 SP1
    # We want to be able to simply skip install since it's not
    # needed on this system
    Goto done_compare_not_needed
is_less:
    Goto done_compare_needed

done_compare_needed:
    #.NET Framework 4.5 install is *NEEDED*

    # Microsoft Download Center EXE:
    # Web Bootstrapper: http://go.microsoft.com/fwlink/?LinkId=225704
    # Full Download: http://go.microsoft.com/fwlink/?LinkId=225702

    # Setup looks for components\dotNET45Full.exe relative to the install EXE location
    # This allows the installer to be placed on a USB stick (for computers without internet connections)
    # If the .NET Framework 4.5 installer is *NOT* found, Setup will connect to Microsoft's website
    # and download it for you

    # Reboot Required with these Exit Codes:
    # 1641 or 3010

    # Command Line Switches:
    # /showrmui /passive /norestart

    # Silent Command Line Switches:
    # /q /norestart


    # Let's see if the user is doing a Silent install or not
    IfSilent is_quiet is_not_quiet

    is_quiet:
        StrCpy $dotNET_CMD_LINE "/q /norestart"
        Goto LookForLocalFile
    is_not_quiet:
        StrCpy $dotNET_CMD_LINE "/showrmui /passive /norestart"
        Goto LookForLocalFile

    LookForLocalFile:
        # Let's see if the user stored the Full Installer
        ;IfFileExists "$EXEPATH\components\dotNET45Full.exe" do_local_install do_network_install

        ;do_local_install:
            # .NET Framework found on the local disk.  Use this copy

         ;   ExecWait '"$EXEPATH\components\dotNET45Full.exe" $dotNET_CMD_LINE' $EXIT_CODE
          ;  Goto is_reboot_requested

        # Now, let's Download the .NET
        ;do_network_install:

            Var /GLOBAL dotNetDidDownload
            NSISdl::download "http://go.microsoft.com/fwlink/?LinkId=225704" "$TEMP\dotNET45Web.exe" $dotNetDidDownload

            ;StrCmp $dotNetDidDownload success fail
			StrCmp $dotNetDidDownload "success" success fail
            success:
                ExecWait '"$TEMP\dotNET45Web.exe" $dotNET_CMD_LINE' $EXIT_CODE
                Goto is_reboot_requested

            fail:
                MessageBox MB_OK|MB_ICONEXCLAMATION "Unable to download .NET Framework.  ${PRODUCT_NAME} will be installed, but will not function without the Framework!"
                Goto done_dotNET_function

            # $EXIT_CODE contains the return codes.  1641 and 3010 means a Reboot has been requested
            is_reboot_requested:
                ${If} $EXIT_CODE = 1641
                ${OrIf} $EXIT_CODE = 3010
                    SetRebootFlag true
                ${EndIf}

done_compare_not_needed:
    # Done dotNET Install
    Goto done_dotNET_function

#exit the function
done_dotNET_function:

FunctionEnd

Function .onInit
	ExecWait "TaskKill /F /IM ${EXE_NAME}.exe"    
				    
    ;call setupdotnetsectionifneeded
  
FunctionEnd

Section "${PRODUCT_NAME}" SecProduct

    ; StrCmp $isAllUser 1 0 +2
    ;SetShellVarContext all
	
	;call CheckAndDownloadDotNet45

	StrCpy $0 ""    
    SectionIn RO
	
    SetOutPath "$INSTDIR\"    	
	
	File /r "bin\*.*"
    
    WriteRegStr HKLM "${PRODUCT_REG_KEY}" "Install_Dir" "$INSTDIR"
    WriteUninstaller "$INSTDIR\Uninstall.exe"	
    
    CreateDirectory "$SMPROGRAMS\${COMPANY_NAME}"                      ;MediAge
    CreateDirectory "$SMPROGRAMS\${COMPANY_NAME}\${PRODUCT_NAME}"      ;MediAge\MediCloudDrive
    CreateShortCut "$SMPROGRAMS\${COMPANY_NAME}\${PRODUCT_NAME}\${PRODUCT_VOLUME}.lnk" "$INSTDIR\${EXE_NAME}.exe"    
    ;CreateShortCut "$DESKTOP\${PRODUCT_VOLUME}.lnk" "$INSTDIR\${EXE_NAME}.exe"
	CreateShortCut "$DESKTOP\Cloud Drive ${MAJOR_VERSION}.${MINOR_VERSION}.lnk" "$INSTDIR\${EXE_NAME}.exe"
	
	; calc Total Setup Size
	call GetInstalledSize
	    
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "DisplayName" "Medi Cloud Drive 1.0"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "DisplayIcon" "$INSTDIR\Mediage.ico"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "UninstallString" '"$INSTDIR\Uninstall.exe"'
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "URLInfoAbout" "http://www.mediage.co.kr/"			
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "Publisher" "${COMPANY_NAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "DisplayVersion" "2019.12.24"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "EstimatedSize" "$GetInstalledSize.total"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "NoModify" 1
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}" "NoRepair" 1
	
SectionEnd

Section "${TXT_SECTION_LAUNCHWHENSYSTEMRUN}" Section_LaunchWhenSystemRun
	SectionIn 1
	
	WriteRegStr	HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "${REG_STARTUP_NAME}" "$INSTDIR\${EXE_NAME}.exe"	
	
SectionEnd

Function un.onInit			
	ExecWait "TaskKill /F /IM ${EXE_NAME}.exe"
	
    ;FindProcDLL::FindProc "${EXE_NAME}.exe"	
    ;Pop $R0
	;MessageBox MB_OK "$R0"
    
    ;${If} $R0 == "1"
    ;    MessageBox MB_OK "${ALREADY_START_UNINSTALL_ERR_MSG}"
    ;    Abort
    ;${EndIf}    
FunctionEnd

Section Uninstall

	KillProcDLL::KillProc "${EXE_NAME}.exe"	

    ReadRegStr $isAllUser HKLM "Software\${COMPANY_NAME}\${PRODUCT_NAME}" "isAllUser"	

    StrCmp $isAllUser 1 0 +2
    SetShellVarContext all
		
    RMDir /r "$INSTDIR"
	
	DeleteRegKey HKLM "${PRODUCT_REG_KEY}"    	
    DeleteRegKey /ifempty HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPLICATION_NAME}"
	
	
	; Reg Start Delete	
	DeleteRegValue HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Run" "${REG_STARTUP_NAME}"
	DeleteRegValue HKLM "SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run" "${REG_STARTUP_NAME}"
	
    
    RMDir /r "$SMPROGRAMS\${COMPANY_NAME}\${PRODUCT_NAME}"
    Delete "$DESKTOP\Cloud Drive ${MAJOR_VERSION}.${MINOR_VERSION}.lnk"
  
SectionEnd

LangString DESC_PRODUCT ${LANG_KOREAN} "Install MediAge Report Cloud Drive"
LangString DESC_STARTUP	${LANG_KOREAN} "Start up to Windows"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${SecProduct} $(DESC_PRODUCT)
!insertmacro MUI_DESCRIPTION_TEXT ${Section_LaunchWhenSystemRun} $(DESC_STARTUP)

!insertmacro MUI_FUNCTION_DESCRIPTION_END