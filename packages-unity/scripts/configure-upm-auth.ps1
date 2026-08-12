# Script to configure Unity Package Manager (UPM) authentication for TrickleCharge Registry
# Usage: Run this script from a PowerShell terminal to automatically configure authentication.

$Token = Read-Host "Enter your TrickleCharge Registry Token"

# Gracefully exit if the user presses enter without pasting a token
if ([string]::IsNullOrWhiteSpace($Token))
{
    Write-Error "Token cannot be empty. Authentication setup aborted."
    exit
}

$ConfigPath = Join-Path $Home ".upmconfig.toml"

$ConfigLines = @(
    '[npmAuth."https://npm.tricklecharge.dev/"]'
    "_authToken = `"$Token`""
    "alwaysAuth = true"
)

# If the file exists, check if it ends with a newline character to prevent bunching or double spacing
if (Test-Path $ConfigPath)
{
    $Content = [System.IO.File]::ReadAllText($ConfigPath)

    if (![string]::IsNullOrEmpty($Content) -and !$Content.EndsWith("`n") -and !$Content.EndsWith("`r"))
    {
        Add-Content -Path $ConfigPath -Value "`n"
    }
}

Add-Content -Path $ConfigPath -Value $ConfigLines

Write-Host "Setup complete! Configured $ConfigPath" -ForegroundColor Green