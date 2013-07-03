add-pssnapin smcmdletsnapin
import-module smlets
xcopy /Y C:\software\SendEmail\SendEmail\bin\Debug\SendEmail.dll C:\Software\SendEmail\SendEmail
Stop-Process -Name Microsoft.EnterpriseManagement.ServiceManager.UI.Console
del .\sendemail.mp
.\FastSeal.exe SendEmail.xml /Company Microsoft /KeyFile testkeys.snk
.\New-MPBFile.ps1 sendemail.mp SendEmail
Get-SCManagementPack -Name SendEmail | Remove-SCManagementPack
del "C:\Users\Administrator\AppData\Local\Microsoft\System Center Service Manager 2010\SMDC\1.0.0.0\SendEmail.dll"
Import-SCSMManagementPack .\SendEmail.mpb
& 'C:\Program Files\Microsoft System Center\Service Manager 2010\Microsoft.EnterpriseManagement.ServiceManager.UI.Console.exe'