Write-Host "========================================="
Write-Host "  Building User Details Application"
Write-Host "========================================="

# Navigate to script directory
Set-Location -Path $PSScriptRoot

Write-Host "`nStopping existing containers..."
docker compose down

Write-Host "`nRemoving old images..."
docker image prune -f

Write-Host "`nRebuilding containers..."
docker compose build --no-cache

Write-Host "`nStarting application..."
docker compose up -d

Write-Host "`nShowing container status..."
docker ps

Write-Host "`nApplication build complete!"
Write-Host "UI:  http://localhost:4200"