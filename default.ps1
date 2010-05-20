properties {
    $baseDir = resolve-path .
    $archiveDir = "$baseDir\_zip"
    $helpDir = "$baseDir\_help"
    $sampleDir = "$baseDir\Sample"
    $slnPath = "$baseDir\WikiPlex.sln"
    $configuration = "Release"
}

#task default -depends tst
task default -depends run-clean, run-build, build-package
#run-tests, run-perf-tests, build-package
task tst {
    $csproj = "$sampleDir\WikiPlex.Web.Sample.csproj"
    regex-replace $csproj '(?ms)<ProjectReference Include="\.\.\\WikiPlex\\WikiPlex\.csproj">.+?</ProjectReference>' '<Reference Include="WikiPlex" />'
}

task run-clean {
    remove-item -force -recurse $archiveDir -ErrorAction SilentlyContinue
    remove-item -force -recurse $helpDir -ErrorAction SilentlyContinue
    remove-item -force -recurse $sampleDir -ErrorAction SilentlyContinue
    exec { msbuild $slnPath /t:Clean /p:Configuration=$configuration /v:quiet }
}

task run-build {
    exec { msbuild $slnPath /t:Build /p:Configuration=$configuration /v:quiet }
    exec { aspnet_compiler -p "$baseDir\WikiPlex.Web.Sample" -v "/WikiPlex.Web.Sample" }
}

task run-tests {
    exec { .\3rdParty\xUnit\xunit.console.x86.exe "$baseDir\WikiPlex.Tests\bin\$configuration\WikiPlex.Tests.dll" }
    exec { .\3rdParty\xUnit\xunit.console.x86.exe "$baseDir\WikiPlex.IntegrationTests\bin\$configuration\WikiPlex.IntegrationTests.dll" }
}

task run-perf-tests {
    exec { .\3rdParty\xUnit\xunit.console.x86.exe "$baseDir\WikiPlex.PerformanceTests\bin\$configuration\WikiPlex.PerformanceTests.dll" }
}

task build-package -depends prepare-sample {
    new-item -path $archiveDir -type directory | out-null
    
    exec { .\3rdParty\zip.exe -9 -A -j `
                              "$archiveDir\test.zip" `
                              "$baseDir\WikiPlex\bin\$configuration\*.dll" `
                              "$baseDir\WikiPlex\bin\$configuration\*.pdb" `
                              "$baseDir\License.txt"
    }
}

task prepare-sample {   
    copy-item "$baseDir\WikiPlex.Web.Sample" -destination $sampleDir -recurse -container
    copy-item "$baseDir\GlobalAssemblyInfo.cs" -destination "$sampleDir\Properties"
    
    $csproj = "$sampleDir\WikiPlex.Web.Sample.csproj"
    
    regex-replace $csproj '(?m)Include="\.\.\\GlobalAssemblyInfo.cs"' 'Include="Properties\GlobalAssemblyInfo.cs"'
    regex-replace $csproj '(?m)<Link>Properties\\GlobalAssemblyInfo.cs</Link>' ''
    regex-replace $csproj '(?ms)<ProjectReference Include="\.\.\\WikiPlex\\WikiPlex\.csproj">.+?</ProjectReference>' '<Reference Include="WikiPlex" />'
}

function regex-replace {
    param (
        [parameter(position=0, mandatory=1)][string]$filePath, 
        [parameter(position=1, mandatory=1)][string]$find, 
        [parameter(position=2)][string]$replacement
    )
    
    $regex = [regex] $find
    $content = get-content $filePath
    
    Assert $regex.IsMatch($content) "Unable to find the regex '$find' to update the file '$filePath'"
    
    set-content $filePath $regex.Replace($content, $replacement)
}