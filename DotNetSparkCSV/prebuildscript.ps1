param([switch]$NoDocker = $false, [switch]$Quick = $false)

Write-Host "🚀 .NET Spark Pre-build Automation" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

# Check if Docker is running
Write-Host "`n🔍 Checking Docker..." -ForegroundColor Yellow
try {
    $dockerInfo = docker version 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Docker is not running! Please start Docker Desktop first." -ForegroundColor Red
        Write-Host "💡 Start Docker Desktop from your Start Menu, then run this script again." -ForegroundColor Yellow
        exit 1
    }
    Write-Host "✅ Docker is running" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker is not running! Please start Docker Desktop first." -ForegroundColor Red
    exit 1
}

# Cleanup phase
Write-Host "`n🧹 Cleaning up previous builds..." -ForegroundColor Yellow
docker-compose down --remove-orphans 2>$null

if ($Quick) {
    Write-Host "⚡ Quick mode - skipping full clean" -ForegroundColor Magenta
} else {
    Remove-Item -Recurse -Force bin, obj, publish -ErrorAction SilentlyContinue
}

# Build phase
Write-Host "`n📦 Building .NET application..." -ForegroundColor Yellow

if ($Quick -and (Test-Path "publish/DotNetSparkCSV.dll")) {
    Write-Host "⚡ Quick mode - using existing publish" -ForegroundColor Magenta
} else {
    Write-Host "  - Restoring packages..." -ForegroundColor Gray
    dotnet restore

    Write-Host "  - Building project..." -ForegroundColor Gray
    dotnet build -c Release --nologo

    Write-Host "  - Publishing application..." -ForegroundColor Gray
    dotnet publish -c Release -o publish --nologo
}

# Verify build
Write-Host "`n✅ Verifying build..." -ForegroundColor Yellow
if (Test-Path "publish/DotNetSparkCSV.dll") {
    Write-Host "  ✅ DotNetSparkCSV.dll found" -ForegroundColor Green
} else {
    Write-Host "❌ DotNetSparkCSV.dll not found!" -ForegroundColor Red
    exit 1
}

# Create test data if it doesn't exist
if (-not (Test-Path "data/sample.csv")) {
    Write-Host "`n📁 Creating sample data..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Force -Path data | Out-Null
    @"
id,name,age,department,salary
1,John Doe,30,Engineering,75000
2,Jane Smith,25,Marketing,65000
3,Bob Johnson,35,Sales,70000
4,Alice Brown,28,Engineering,80000
5,Charlie Wilson,32,Marketing,60000
"@ | Out-File -FilePath "data/sample.csv" -Encoding utf8
    Write-Host "  ✅ Sample data created" -ForegroundColor Green
}

if (-not $NoDocker) {
    Write-Host "`n🐳 Starting Docker containers..." -ForegroundColor Cyan
    Write-Host "   Spark UI: http://localhost:8080" -ForegroundColor Magenta
    Write-Host "   Worker UI: http://localhost:8081" -ForegroundColor Magenta
    
    # Build and start containers
    Write-Host "`n🔨 Building Docker image..." -ForegroundColor Gray
    docker-compose up --build
} else {
    Write-Host "`n✅ Pre-build completed successfully!" -ForegroundColor Green
    Write-Host "   Run 'docker-compose up --build' to start Docker containers" -ForegroundColor Yellow
}

Write-Host "`n💡 Usage Tips:" -ForegroundColor Cyan
Write-Host "   ./prebuildscript.ps1 -Quick              # Quick rebuild" -ForegroundColor Gray
Write-Host "   ./prebuildscript.ps1 -NoDocker           # Build only, don't start Docker" -ForegroundColor Gray