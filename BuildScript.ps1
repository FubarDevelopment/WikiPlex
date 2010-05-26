properties {
    # setable properties
    $configuration = if ($env:TEAMCITY_PROJECT_NAME -ne $NULL) { 'Release' } else { 'Debug' }
    $buildNumber = if ($env:build_number -ne $NULL) { $env:build_number } else { '1.0.0.0' }
    
    # paths
    $baseDir = resolve-path .
    $archiveDir = "$baseDir\_zip"
    $helpDir = "$baseDir\_help"
    $sampleDir = "$baseDir\Sample"
    $slnPath = "$baseDir\WikiPlex.sln"
    $docSlnPath = "$baseDir\WikiPlex.Documentation\WikiPlex.Documentation.shfbproj"
}

task default -depends run-clean, run-build, run-tests, run-perf-tests
task ci -depends run-clean, set-version, run-build, run-tests, build-package
task doc -depends prepare-documentation
task cleandoc -depends clean-documentation-files
task builddoc -depends prepare-documentation, build-documentation, clean-documentation-files

task run-clean {
    clean $archiveDir
    clean $helpDir
    clean $sampleDir
    exec { msbuild $slnPath /t:Clean /p:Configuration=$configuration /v:quiet }
}

task run-build {
    exec { msbuild $slnPath /t:Build /p:Configuration=$configuration /v:quiet }
    exec { aspnet_compiler -p "$baseDir\WikiPlex.Web.Sample" -v "/WikiPlex.Web.Sample" }
}

task run-tests {
    execute-tests "$baseDir\WikiPlex.Tests\bin\$configuration\WikiPlex.Tests.dll" "Tests"
    execute-tests "$baseDir\WikiPlex.IntegrationTests\bin\$configuration\WikiPlex.IntegrationTests.dll" "Integration Tests"
}

task run-perf-tests {
    execute-tests "$baseDir\WikiPlex.PerformanceTests\bin\$configuration\WikiPlex.PerformanceTests.dll" "Performance Tests"
}

task set-version {
    $assemInfo = "$baseDir\GlobalAssemblyInfo.cs"
    exec { attrib -r $assemInfo }
    regex-replace $assemInfo 'AssemblyVersion\("\d+\.\d+\.\d+\.\d+"\)' "AssemblyVersion(`"$buildNumber`")"
    regex-replace $assemInfo 'AssemblyFileVersion\("\d+\.\d+\.\d+\.\d+"\)' "AssemblyFileVersion(`"$buildNumber`")"
}

task build-package -depends prepare-sample {
    create $archiveDir
    
    exec { .\3rdParty\zip.exe -9 -A -j `
                              "$archiveDir\WikiPlex.zip" `
                              "$baseDir\WikiPlex\bin\$configuration\*.dll" `
                              "$baseDir\WikiPlex\bin\$configuration\*.pdb" `
                              "$baseDir\License.txt"
    }
    
    exec { .\3rdParty\zip.exe -9 -A -r `
                              "$archiveDir\WikiPlex-Sample.zip" `
                              "Sample" `
                              "Sample-Readme.txt" `
                              "License.txt"
    }
    
    clean $sampleDir
}

task prepare-sample {   
    copy-item "$baseDir\WikiPlex.Web.Sample" -destination $sampleDir -recurse -container
    copy-item "$baseDir\GlobalAssemblyInfo.cs" -destination "$sampleDir\Properties"
    
    $csproj = "$sampleDir\WikiPlex.Web.Sample.csproj"
    
    regex-replace $csproj '(?m)Include="\.\.\\GlobalAssemblyInfo.cs"' 'Include="Properties\GlobalAssemblyInfo.cs"'
    regex-replace $csproj '(?m)<Link>Properties\\GlobalAssemblyInfo.cs</Link>' ''
    regex-replace $csproj '(?ms)<ProjectReference Include="\.\.\\WikiPlex\\WikiPlex\.csproj">.+?</ProjectReference>' '<Reference Include="WikiPlex" />'
}

task prepare-documentation -depends run-build {
    clean $helpDir
    create $helpDir
    
    copy-item "$baseDir\WikiPlex\bin\$configuration\*.dll", "$baseDir\WikiPlex\bin\$configuration\*.xml" -destination "$baseDir\WikiPlex.Documentation"
    copy-item "$baseDir\WikiPlex\bin\$configuration\*.dll", "$baseDir\3rdParty\WikiMaml\*.*" -destination $helpDir
    
    exec { .\_help\WikiMaml.Console.exe "$baseDir\WikiPlex.Documentation\Source" "$baseDir\WikiPlex.Documentation" }
}

task build-documentation -depends prepare-documentation {
    exec { msbuild $docSlnPath /p:"Configuration=$configuration;Platform=AnyCpu;OutDir=$helpDir" }
}

task clean-documentation-files {
    $docPath = "$baseDir\WikiPlex.Documentation"
    foreach ($path in get-childitem $docPath -include *.dll, *.xml, *.aml -recurse) { remove-item $path }
    foreach ($path in get-childitem $docPath -exclude Source | where-object { $_.PSIsContainer }) { remove-item $path }
}

function global:execute-tests($assembly, $message) {
    exec { .\3rdParty\xUnit\xunit.console.x86.exe $assembly } "Failure running $message"
}

function global:clean($path) {
    remove-item -force -recurse $path -ErrorAction SilentlyContinue
}

function global:create($path) {
    new-item -path $path -type directory | out-null
}

function global:regex-replace($filePath, $find, $replacement) {
    $regex = [regex] $find
    $content = [System.IO.File]::ReadAllText($filePath)
    
    Assert $regex.IsMatch($content) "Unable to find the regex '$find' to update the file '$filePath'"
    
    [System.IO.File]::WriteAllText($filePath, $regex.Replace($content, $replacement))
}