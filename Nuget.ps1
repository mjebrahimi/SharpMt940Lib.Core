$path = pwd
$path = $path.Path


Function BuildSolution ([string]$configuration, [string]$platform) {

    $buildlog = ".artifacts\msbuild_$platform.txt".
        ToLower(). 
        Replace(" ","")

    Write-Host -NoNewLine "Building ($platform | $configuration) using log: $buildlog."

    $result = Start-Process `
        -FilePath msbuild `
        -PassThru `
        -Wait `
        -RedirectStandardOutput $buildlog `
        -NoNewWindow `
        -ArgumentList `
        @("/m", 
        "/target:`"Raptorious_SharpMt940Lib:Rebuild`"",
        "/property:Platform=`"$platform`"",
        "/property:Configuration=`"$configuration`"",
        "/property:OutputPath=`"..\.artifacts\$platform\lib\net40-client`"",
        "/property:msbuildemitsolution=1"
        "/nologo",
        "Raptorious.SharpMT940Lib.sln")

    if ($result.ExitCode -ne 0) {
        Write-Host -ForegroundColor Red " Failed, errors"
        Get-Content $buildlog 
    }

    else {
        Write-Host -ForegroundColor Green " Done, no errors"
    }

    Write-Host

    return $result
}

Function GetMsBuildPath([string]$version) {
    return (Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\$version"  MSBuildToolsPath).MSBuildToolsPath
}

Function CreateXmlWriter($path) {
    # get an XMLTextWriter to create the XML
    $XmlWriter = New-Object System.XMl.XmlTextWriter($path,$Null)
 
    # choose a pretty formatting:
    $xmlWriter.Formatting = 'Indented'
    $xmlWriter.Indentation = 1
    $XmlWriter.IndentChar = "`t"

    $xmlWriter.WriteStartDocument()

    return $xmlWriter
}

Function GetNuspecInfo([string]$platform) {
    Write-Host -NoNewline "Colllecting nuget information..."

    $versionInfo = (dir .\.artifacts\$platform\lib\net40-client\Raptorious.SharpMt940Lib.dll).VersionInfo

	$changlog = Get-Content "$path\Raptorious.SharpMT940Lib\CHANGELOG.md"
	
    $nuget = @{
        "id" = "Raptorious.Finance.Swift.Mt940"
        "title" = "SharpMt940Lib"
        "authors" = "Jaco Adriaansen"
        "licenseUrl" = "http://www.raptorious.com/mt940lib/"
        "projectUrl" = "http://www.raptorious.com/mt940lib/"
        "requireLicenseAcceptance" = "false"
        "copyright" = "Copyright 2012-2014"
        "version" = $versionInfo.ProductVersion
        "description" = $versionInfo.Comments
        "tags" = "finance mt940 bank"
		"releaseNotes" = $changelog
    }

    Write-Host -ForegroundColor Yellow " Found" $nuget.Title.ToString() "version" $nuget.Version.ToString()
    return $nuget
}

Function GenerateNuspec($nuget, [string]$output) {    
    Write-Host -NoNewline "Creating nuspec: $output"

    $writer = CreateXmlWriter($output)
    
    $writer.WriteStartElement("package")
    $writer.WriteStartElement("metadata")
    
    foreach($item in $nuget.Keys) {
        $writer.WriteElementString($item, $nuget[$item])
    }   

	#$writer.WriteStartElement("files")
	#$writer.WriteElementString("file", "README.md")
    #$writer.WriteEndElement()
	
    $writer.WriteStartElement("references")
    
    $writer.WriteStartElement("reference")
    $writer.WriteAttributeString("file", "Raptorious.SharpMt940Lib.dll")
    $writer.WriteEndElement()
    
    $writer.WriteEndElement()
    $writer.WriteEndElement()
    $writer.WriteEndElement()

    $writer.WriteEndDocument()
    $writer.Flush()
    $writer.Close()

    Write-Host -ForegroundColor Yellow  " Done."
}

Function PreparePackage([string]$platform) {
    
}

$msbuild_path = GetMsBuildPath("4.0")
$env:Path += ";" + $msbuild_path

$default = "Any CPU"
$platforms = @("Any CPU", "x86", "x64")
$configuration = "Release"

$results = @()

$push  = Read-Host 'Push to nuget.org? [y/N]'    
	
foreach($platform in $platforms) {
    $results += BuildSolution $configuration $platform
    
    $nuget = GetNuspecInfo $platform

    $output = $path + "\.artifacts\$platform\Raptorious.SharpMt940LibSwift"
    
    if($platform -ne "Any CPU") {
        $output += ".$platform"

        $nuget.Set_Item("id", $nuget.Id + ".$platform")
        $nuget.Set_Item("title", $nuget.Title.ToString() + "($platform)")
    }
    
    $output += ".nuspec"

    GenerateNuspec $nuget $output

    $nuget_exec = $path + "\.nuget\nuget.exe"

    cd "$path\.artifacts\$platform"

    
    $result = Invoke-Expression "$nuget_exec pack -Symbols `"$output`""
    

	if($push.ToLower() -eq 'y') {
		foreach($i in 1..2) {
			$nuget_package = $result[$i].Substring("Successfully created package ".Length).Trim(".")
			Write-Host $nuget_package
			Invoke-Expression "$nuget_exec push $nuget_package"
		}
	}
    
    cd $path

    Write-Host
    Write-Host -ForegroundColor Green "Done."
    Write-Host
}

pause