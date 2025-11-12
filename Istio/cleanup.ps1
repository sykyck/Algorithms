# Cleanup Script for ASP.NET Core + Istio Demo

Write-Host "=== Cleaning up ASP.NET Core + Istio Demo ===" -ForegroundColor Green

Write-Host "1. Deleting Kind cluster..." -ForegroundColor Yellow
kind delete cluster --name aspnet-istio

Write-Host "2. Removing local Docker images..." -ForegroundColor Yellow
docker rmi product-api:latest -f
docker rmi order-api:latest -f

Write-Host "3. Cleaning up Istio files..." -ForegroundColor Yellow
Remove-Item -Recurse -Force -ErrorAction SilentlyContinue istio-*

Write-Host "4. Pruning Docker system..." -ForegroundColor Yellow
docker system prune -f

Write-Host "Cleanup completed!" -ForegroundColor Green
Write-Host "All resources have been removed from your system." -ForegroundColor White