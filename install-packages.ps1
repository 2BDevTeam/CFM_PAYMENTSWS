$packages = Get-Content "packages.txt" | Select-Object -Skip 1

foreach ($package in $packages) {
    $parts = $package -split '\s{2,}'
    $id = $parts[0]
    $version = $parts[1] -replace '[{}]', ''
    Write-Host "Installing $id version $version"
    dotnet add package $id --version $version
}
