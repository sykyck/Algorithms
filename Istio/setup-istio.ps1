# ASP.NET Core + Istio Setup Script for Windows

Write-Host "=== ASP.NET Core + Istio Setup ===" -ForegroundColor Green

# Step 1: Create Kind cluster
Write-Host "1. Creating Kubernetes cluster..." -ForegroundColor Yellow
kind create cluster --name aspnet-istio

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to create Kind cluster" -ForegroundColor Red
    exit 1
}

# Step 2: Install Istio
Write-Host "2. Installing Istio..." -ForegroundColor Yellow
$istioVersion = "1.19.3"  # You can change this to the latest version
$istioUrl = "https://github.com/istio/istio/releases/download/$istioVersion/istio-$istioVersion-win.zip"

# Download Istio
Invoke-WebRequest -Uri $istioUrl -OutFile "istio.zip"
Expand-Archive -Path "istio.zip" -DestinationPath "." -Force
Remove-Item "istio.zip"

# Set Istio path and install
$istioDir = Get-ChildItem -Directory | Where-Object { $_.Name -like "istio-*" } | Select-Object -First 1
if ($istioDir) {
    $istioPath = Join-Path $istioDir.FullName "bin\istioctl.exe"
    & $istioPath install --set profile=demo -y
} else {
    Write-Host "Failed to find Istio directory" -ForegroundColor Red
    exit 1
}

# Step 3: Enable Istio sidecar injection
Write-Host "3. Enabling Istio sidecar injection..." -ForegroundColor Yellow
kubectl label namespace default istio-injection=enabled --overwrite

# Step 4: Build and deploy applications
Write-Host "4. Building and deploying ASP.NET Core applications..." -ForegroundColor Yellow

# Build Docker images
Write-Host "   Building Product API..." -ForegroundColor Cyan
docker build -t product-api:latest ./src/ProductApi

Write-Host "   Building Order API..." -ForegroundColor Cyan
docker build -t order-api:latest ./src/OrderApi

# Load images into Kind
Write-Host "   Loading images into Kind cluster..." -ForegroundColor Cyan
kind load docker-image product-api:latest --name aspnet-istio
kind load docker-image order-api:latest --name aspnet-istio

# Step 5: Deploy to Kubernetes
Write-Host "5. Deploying to Kubernetes..." -ForegroundColor Yellow
kubectl apply -f ./kubernetes/product-api.yaml
kubectl apply -f ./kubernetes/order-api.yaml
kubectl apply -f ./kubernetes/gateway.yaml

# Wait for pods to be ready
Write-Host "6. Waiting for pods to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Check pod status
Write-Host "`nPod Status:" -ForegroundColor Green
kubectl get pods

Write-Host "`nServices:" -ForegroundColor Green
kubectl get services

Write-Host "`nIstio Gateway:" -ForegroundColor Green
kubectl get gateway

# Step 6: Start port forwarding
Write-Host "`n7. Starting port forwarding..." -ForegroundColor Yellow
Write-Host "   Access your applications at:" -ForegroundColor White
Write-Host "   Products: http://localhost:8080/api/products" -ForegroundColor Cyan
Write-Host "   Orders:   http://localhost:8080/api/orders" -ForegroundColor Cyan
Write-Host "`nPress Ctrl+C to stop the port forwarding" -ForegroundColor Yellow

# Start port forwarding
kubectl port-forward -n istio-system svc/istio-ingressgateway 8080:80