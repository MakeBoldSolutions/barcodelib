<#
.SYNOPSIS
    Builds and runs Barcode.Web locally.
.DESCRIPTION
    Frees the dev server ports (5182/7182) if a previous instance is still
    holding them, then starts Barcode.Web with `dotnet run`.
#>

$ErrorActionPreference = 'Stop'

$repoRoot = $PSScriptRoot
$webProject = Join-Path $repoRoot 'Barcode.Web\Barcode.Web.csproj'

$ports = 5182, 7182
foreach ($port in $ports) {
    $conns = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue
    foreach ($conn in $conns) {
        $processId = $conn.OwningProcess
        $proc = Get-Process -Id $processId -ErrorAction SilentlyContinue
        if ($proc) {
            Write-Host "Stopping process $($proc.ProcessName) (PID $processId) holding port $port..."
            Stop-Process -Id $processId -Force
        }
    }
}

Write-Host "Starting Barcode.Web..."
dotnet run --project $webProject
