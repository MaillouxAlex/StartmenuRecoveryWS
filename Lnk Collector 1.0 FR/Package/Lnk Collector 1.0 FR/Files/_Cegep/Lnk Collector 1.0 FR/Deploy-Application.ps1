<#
.SYNOPSIS
	This script performs the installation or uninstallation of an application(s).
	# LICENSE #
	PowerShell App Deployment Toolkit - Provides a set of functions to perform common application deployment tasks on Windows.
	Copyright (C) 2017 - Sean Lillis, Dan Cunningham, Muhammad Mashwani, Aman Motazedian.
	This program is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation, either version 3 of the License, or any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
	You should have received a copy of the GNU Lesser General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.
.DESCRIPTION
	The script is provided as a template to perform an install or uninstall of an application(s).
	The script either performs an "Install" deployment type or an "Uninstall" deployment type.
	The install deployment type is broken down into 3 main sections/phases: Pre-Install, Install, and Post-Install.
	The script dot-sources the AppDeployToolkitMain.ps1 script which contains the logic and functions required to install or uninstall an application.
.PARAMETER DeploymentType
	The type of deployment to perform. Default is: Install.
.PARAMETER DeployMode
	Specifies whether the installation should be run in Interactive, Silent, or NonInteractive mode. Default is: Interactive. Options: Interactive = Shows dialogs, Silent = No dialogs, NonInteractive = Very silent, i.e. no blocking apps. NonInteractive mode is automatically set if it is detected that the process is not user interactive.
.PARAMETER AllowRebootPassThru
	Allows the 3010 return code (requires restart) to be passed back to the parent process (e.g. SCCM) if detected from an installation. If 3010 is passed back to SCCM, a reboot prompt will be triggered.
.PARAMETER TerminalServerMode
	Changes to "user install mode" and back to "user execute mode" for installing/uninstalling applications for Remote Destkop Session Hosts/Citrix servers.
.PARAMETER DisableLogging
	Disables logging to file for the script. Default is: $false.
.EXAMPLE
    powershell.exe -Command "& { & '.\Deploy-Application.ps1' -DeployMode 'Silent'; Exit $LastExitCode }"
.EXAMPLE
    powershell.exe -Command "& { & '.\Deploy-Application.ps1' -AllowRebootPassThru; Exit $LastExitCode }"
.EXAMPLE
    powershell.exe -Command "& { & '.\Deploy-Application.ps1' -DeploymentType 'Uninstall'; Exit $LastExitCode }"
.EXAMPLE
    Deploy-Application.exe -DeploymentType "Install" -DeployMode "Silent"
.NOTES
	Toolkit Exit Code Ranges:
	60000 - 68999: Reserved for built-in exit codes in Deploy-Application.ps1, Deploy-Application.exe, and AppDeployToolkitMain.ps1
	69000 - 69999: Recommended for user customized exit codes in Deploy-Application.ps1
	70000 - 79999: Recommended for user customized exit codes in AppDeployToolkitExtensions.ps1
.LINK
	http://psappdeploytoolkit.com
#>
[CmdletBinding()]
Param (
	[Parameter(Mandatory=$false)]
	[ValidateSet('Install','Uninstall','Repair')]
	[string]$DeploymentType = 'Install',
	[Parameter(Mandatory=$false)]
	[ValidateSet('Interactive','Silent','NonInteractive')]
	[string]$DeployMode = 'Interactive',
	[Parameter(Mandatory=$false)]
	[switch]$AllowRebootPassThru = $false,
	[Parameter(Mandatory=$false)]
	[switch]$TerminalServerMode = $false,
	[Parameter(Mandatory=$false)]
	[switch]$DisableLogging = $false,
	[Parameter(Mandatory=$false)]
	[string] $Url = "" #ex: http://servername.com/api/shortcuts
)
function APIRestored(){
	Param (
		[string] $LnkNameRestored,
		[string] $LnkPathRestored
	)
	$ObjectProperties = @{
		LnkName = $LnkNameRestored
		LnkPath = $LnkPathRestored
	}

	$JSON = ConvertTo-Json -InputObject $(New-Object PSObject -Property $ObjectProperties)

	$restoreURI = "http://smrws.cegep-chicoutimi.qc.ca/api/SorcutRestored"
	#$restoreURI = "http://localhost:5167/api/SorcutRestored"

	try {
		Invoke-RestMethod -Uri $restoreURI -Method 'Post' -Body $JSON -ContentType "application/json; charset=utf-8"
	}
	catch {
		$streamReader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
		$ErrResp = $streamReader.ReadToEnd()
		$streamReader.Close()
		$ErrResp
	}
}
Try {
	## Set the script execution policy for this process
	Try { Set-ExecutionPolicy -ExecutionPolicy 'ByPass' -Scope 'Process' -Force -ErrorAction 'Stop' } Catch {}

	##*===============================================
	##* VARIABLE DECLARATION
	##*===============================================
	## Variables: Application
	[string]$appVendor = 'Cegep de Chicoutimi'
	[string]$appName = 'Lnk Creator LocalInstance'
	[string]$appVersion = '1.0'
	[string]$appArch = 'x64'
	[string]$appLang = 'EN'
	[string]$appRevision = '01'
	[string]$appScriptVersion = '1.0.0'
	[string]$appScriptDate = '14/01/2023'
	[string]$appScriptAuthor = 'RAGA HULA ALMA'
	##*===============================================
	## Variables: Install Titles (Only set here to override defaults set by the toolkit)
	[string]$installName = ''
	[string]$installTitle = ''

	##* Do not modify section below
	#region DoNotModify

	## Variables: Exit Code
	[int32]$mainExitCode = 0

	## Variables: Script
	[string]$deployAppScriptFriendlyName = 'Deploy Application'
	[version]$deployAppScriptVersion = [version]'3.8.4'
	[string]$deployAppScriptDate = '26/01/2021'
	[hashtable]$deployAppScriptParameters = $psBoundParameters

	## Variables: Environment
	If (Test-Path -LiteralPath 'variable:HostInvocation') { $InvocationInfo = $HostInvocation } Else { $InvocationInfo = $MyInvocation }
	[string]$scriptDirectory = Split-Path -Path $InvocationInfo.MyCommand.Definition -Parent

	## Dot source the required App Deploy Toolkit Functions
	Try {
		[string]$moduleAppDeployToolkitMain = "$scriptDirectory\AppDeployToolkit\AppDeployToolkitMain.ps1"
		If (-not (Test-Path -LiteralPath $moduleAppDeployToolkitMain -PathType 'Leaf')) { Throw "Module does not exist at the specified location [$moduleAppDeployToolkitMain]." }
		If ($DisableLogging) { . $moduleAppDeployToolkitMain -DisableLogging } Else { . $moduleAppDeployToolkitMain }
	}
	Catch {
		If ($mainExitCode -eq 0){ [int32]$mainExitCode = 60008 }
		Write-Error -Message "Module [$moduleAppDeployToolkitMain] failed to load: `n$($_.Exception.Message)`n `n$($_.InvocationInfo.PositionMessage)" -ErrorAction 'Continue'
		## Exit the script, returning the exit code to SCCM
		If (Test-Path -LiteralPath 'variable:HostInvocation') { $script:ExitCode = $mainExitCode; Exit } Else { Exit $mainExitCode }
	}

	
	#endregion
	##* Do not modify section above
	##*===============================================
	##* END VARIABLE DECLARATION
	##*===============================================

	If ($deploymentType -ine 'Uninstall' -and $deploymentType -ine 'Repair') {
		##*===============================================
		##* PRE-INSTALLATION
		##*===============================================
		[string]$installPhase = 'Pre-Installation'

		## <Perform Pre-Installation tasks here>


		##*===============================================
		##* INSTALLATION
		##*===============================================
		[string]$installPhase = 'Installation'

		## <Perform Installation tasks here>

        #SQL Server URL
        $Url = "http://smrws.cegep-chicoutimi.qc.ca/api/shortcuts"
		#$Url = "http://localhost:5167/api/shortcuts"

        #Local log file to list all created shortcuts since LNK Creator exists
        $CreatorLogFile = "$env:SYSTEMROOT\_Cegep\Lnk Creator 1.0 FR\LnkCreator.log"
        if (!(Test-Path -Path $CreatorLogFile)) {
            New-Item -ItemType File -Path $CreatorLogFile -Force
        }

        $date = Get-Date -Format "yyyy-mm-dd HH:mm:ss"


        #JSON Get Method to get all approved shortcuts from remote SQL database
        try {
            $LnkList = Invoke-RestMethod -Uri $Url -Method 'Get' #-ContentType "application/json"
        }
        catch {
            $streamReader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
            $ErrResp = $streamReader.ReadToEnd()
            $streamReader.Close()
            $ErrResp
            $LnkList = ""
        }


        if ($LnkList) {

            #Get All users profile paths except for the Public one
            $UsersProfilePaths = Get-ChildItem "C:\Users\*" | Where-Object {$_.Name -notmatch "Public"}

            Foreach ($shortcut in $LnkList)
            {
	            $LnkPath = $shortcut.LnkPath
	            $AppPath = $shortcut.AppPath
	            $AppArgs = $shortcut.AppArgs
	            $LnkName = $shortcut.LnkName


                switch -Regex ($LnkPath)
                {
                    '^\$EACHUSERPROFILE\$' 
                    {

                        ForEach($folder in $UsersProfilePaths) 
                        {
                            $ConvertedLnkPath = $LnkPath -replace ('^\$EACHUSERPROFILE\$',$folder)
                            $shortcutPath = $ConvertedLnkPath+$LnkName

                            if(!(Test-Path -Path $shortcutPath)){
                                if (Test-Path -Path $AppPath) {
				                    if ($shortcut.AppArgs){
					                    New-Shortcut -path $shortcutPath -TargetPath $AppPath -Arguments $AppArgs -description $LnkName
				                    }
				                    else {
					                    New-Shortcut -path $shortcutPath -TargetPath $AppPath -description $LnkName
				                    }
                                    $data = $date + ";" + $shortcutPath
                                    Add-Content -Path $CreatorLogFile -Value $data
									APIRestored -LnkNameRestored $LnkName -LnkPathRestored $LnkPath
                                }
			                }
                        }

                    }
                    Default 
                    {

                        $shortcutPath = $LnkPath+$LnkName
                        if(!(Test-Path -Path $shortcutPath)){
                            if (Test-Path -Path $AppPath) {
				                if ($shortcut.AppArgs){
					                New-Shortcut -path $shortcutPath -TargetPath $AppPath -Arguments $AppArgs -description $LnkName
				                }
				                else {
					                New-Shortcut -path $shortcutPath -TargetPath $AppPath -description $LnkName
                                
				                }
                                $data = $date + ";" + $shortcutPath
                                Add-Content -Path $CreatorLogFile -Value $data
								APIRestored -LnkNameRestored $LnkName -LnkPathRestored $LnkPath
                            } 
			            }

                    }

                } #End of Switch

            } #End of ForEach

        } #End of If


		##*===============================================
		##* POST-INSTALLATION
		##*===============================================
		[string]$installPhase = 'Post-Installation'

		## <Perform Post-Installation tasks here>

		## Copy configuration file to every users

	}
	ElseIf ($deploymentType -ieq 'Uninstall')
	{
		##*===============================================
		##* PRE-UNINSTALLATION
		##*===============================================
		[string]$installPhase = 'Pre-Uninstallation'

		## <Perform Pre-Uninstallation tasks here>


		##*===============================================
		##* UNINSTALLATION
		##*===============================================
		[string]$installPhase = 'Uninstallation'

		## <Perform Uninstallation tasks here>


		##*===============================================
		##* POST-UNINSTALLATION
		##*===============================================
		[string]$installPhase = 'Post-Uninstallation'

		## <Perform Post-Uninstallation tasks here>


	}
	ElseIf ($deploymentType -ieq 'Repair')
	{
		##*===============================================
		##* PRE-REPAIR
		##*===============================================
		[string]$installPhase = 'Pre-Repair'

		## Show Progress Message (with the default message)
		Show-InstallationProgress

		## <Perform Pre-Repair tasks here>

		##*===============================================
		##* REPAIR
		##*===============================================
		[string]$installPhase = 'Repair'

		## Handle Zero-Config MSI Repairs
		If ($useDefaultMsi) {
			[hashtable]$ExecuteDefaultMSISplat =  @{ Action = 'Repair'; Path = $defaultMsiFile; }; If ($defaultMstFile) { $ExecuteDefaultMSISplat.Add('Transform', $defaultMstFile) }
			Execute-MSI @ExecuteDefaultMSISplat
		}
		# <Perform Repair tasks here>

		##*===============================================
		##* POST-REPAIR
		##*===============================================
		[string]$installPhase = 'Post-Repair'

		## <Perform Post-Repair tasks here>


    }
	##*===============================================
	##* END SCRIPT BODY
	##*===============================================

	## Call the Exit-Script function to perform final cleanup operations
	Exit-Script -ExitCode $mainExitCode
}
Catch {
	[int32]$mainExitCode = 60001
	[string]$mainErrorMessage = "$(Resolve-Error)"
	Write-Log -Message $mainErrorMessage -Severity 3 -Source $deployAppScriptFriendlyName
	Show-DialogBox -Text $mainErrorMessage -Icon 'Stop'
	Exit-Script -ExitCode $mainExitCode
}
