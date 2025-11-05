#!/bin/bash
# Line endings must be LF (Unix) not CRLF (Windows)

echo "🚀 Starting .NET Spark Application with JVM Bridge"

# Wait for Spark cluster
echo "Waiting for Spark cluster to be ready..."
sleep 45

# Check if worker is available
WORKER_DIR="/opt/microsoft-spark"
WORKER_BINARY="$WORKER_DIR/Microsoft.Spark.Worker"
PORT=${DOTNETBACKEND_PORT:-5567}

if [ -f "$WORKER_BINARY" ]; then
    echo "✅ Microsoft.Spark.Worker is available at: $WORKER_BINARY"
    
    # Make sure it's executable
    chmod +x "$WORKER_BINARY"
    
    echo "Starting JVM Bridge on port $PORT..."
    
    # Start JVM Bridge in background and capture PID
    "$WORKER_BINARY" --port "$PORT" > /tmp/jvm-bridge.log 2>&1 &
    JVM_PID=$!
    
    echo "JVM Bridge started with PID: $JVM_PID"
    
    # Wait for bridge to start
    echo "Waiting for JVM Bridge to start listening..."
    sleep 10
    
    # Check if port is listening
    if netstat -tuln 2>/dev/null | grep ":$PORT" > /dev/null || ss -tuln 2>/dev/null | grep ":$PORT" > /dev/null; then
        echo "✅ JVM Bridge is listening on port $PORT"
    else
        echo "❌ JVM Bridge is not listening on port $PORT"
        echo "JVM Bridge log:"
        cat /tmp/jvm-bridge.log
        kill $JVM_PID 2>/dev/null
        exit 1
    fi
    
    # Start the .NET application
    echo "Starting .NET application..."
    dotnet /app/DotNetSparkCSV.dll
    DOTNET_EXIT_CODE=$?
    
    # Cleanup
    echo "Cleaning up JVM Bridge..."
    kill $JVM_PID 2>/dev/null
    exit $DOTNET_EXIT_CODE
else
    echo "❌ Microsoft.Spark.Worker not found at: $WORKER_BINARY"
    ls -la "$WORKER_DIR"
    exit 1
fi