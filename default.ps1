properties {
    $baseDir = resolve-path .
    $archiveDir = "$baseDir\_zip"
    $helpDir = "$baseDir\_help"
    $sampleDir = "$baseDir\Sample"
    $slnPath = "$baseDir\WikiPlex.sln"
    $configuration = "Release"
}

task default -depends run-clean, run-build, run-tests, run-perf-tests, build-package

task run-clean {
    remove-item -force -recurse $archiveDir -ErrorAction SilentlyContinue
    remove-item -force -recurse $helpDir -ErrorAction SilentlyContinue
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

task build-package {
    new-item -path "$archiveDir" -type directory | out-null
    
    exec { .\3rdParty\zip.exe -9 -A -j `
                              "$archiveDir\test.zip" `
                              "$baseDir\WikiPlex\bin\$configuration\*.dll" `
                              "$baseDir\WikiPlex\bin\$configuration\*.pdb" `
                              "$baseDir\License.txt"
    }
}