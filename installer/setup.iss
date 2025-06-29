; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Plc Soft Analyzer"
#define MyAppVersion "1.0"
#define MyAppExeName "PlcSoftAnalyzer.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{2142A05D-8A26-4AA3-89B9-613B24679B91}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
DefaultDirName={autopf}\{#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
DefaultGroupName={#MyAppName}
InfoAfterFile=C:\VisualStudio\repos\PlcSoftAnalyzer\artifacts\readme.txt.txt
; Uncomment the following line to run in non administrative install mode (install for current user only).
;PrivilegesRequired=lowest
OutputDir=PlcSoftAnalyzerInstaller
OutputBaseFilename=PlcSoftAnalyzerInstaller
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\src\PlcSoftAnalyzer\bin\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\readme.txt.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\src\PlcSoftAnalyzer\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

