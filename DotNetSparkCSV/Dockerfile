FROM mcr.microsoft.com/dotnet/runtime:3.1

# Update package sources to use archive repositories for Debian Buster
RUN sed -i 's/deb.debian.org/archive.debian.org/g' /etc/apt/sources.list && \
    sed -i 's|security.debian.org|archive.debian.org/debian-security|g' /etc/apt/sources.list && \
    sed -i '/buster-updates/d' /etc/apt/sources.list

# Install dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    openjdk-11-jre \
    net-tools \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Download and install Microsoft.Spark.Worker for .NET Core 3.1
RUN mkdir -p /opt/microsoft-spark && \
    cd /opt/microsoft-spark && \
    curl -L -o microsoft-spark.tar.gz \
    "https://github.com/dotnet/spark/releases/download/v2.1.1/Microsoft.Spark.Worker.netcoreapp3.1.linux-x64-2.1.1.tar.gz" && \
    tar -xzf microsoft-spark.tar.gz --strip-components=1 && \
    rm microsoft-spark.tar.gz && \
    chmod +x Microsoft.Spark.Worker

# Set environment variables
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV DOTNET_WORKER_DIR=/opt/microsoft-spark
ENV DOTNETBACKEND_PORT=5567
ENV SPARK_MASTER=spark://spark-master:7077
ENV JAVA_HOME=/usr/lib/jvm/java-11-openjdk-amd64

WORKDIR /app
COPY publish/ .

# Health check using bash built-in TCP (since netcat is not available)
HEALTHCHECK --interval=10s --timeout=3s --start-period=30s --retries=3 \
  CMD bash -c 'echo > /dev/tcp/spark-master/7077' 2>/dev/null || exit 1

# Startup script using bash built-in TCP check
CMD /bin/bash -c "\
  echo '🚀 Starting .NET Spark Application with JVM Bridge' && \
  echo 'Waiting for Spark master to be ready...' && \
  while ! bash -c 'echo > /dev/tcp/spark-master/7077' 2>/dev/null; do \
    echo 'Waiting for Spark master...'; \
    sleep 5; \
  done && \
  echo '✅ Spark master is ready!' && \
  \
  echo 'Starting JVM Bridge on port 5567...' && \
  /opt/microsoft-spark/Microsoft.Spark.Worker --port 5567 > /tmp/jvm-bridge.log 2>&1 & \
  JVM_PID=\$! && \
  sleep 10 && \
  \
  if netstat -tuln 2>/dev/null | grep ':5567' > /dev/null; then \
    echo '✅ JVM Bridge is listening on port 5567' && \
    echo 'Starting .NET application...' && \
    dotnet /app/DotNetSparkCSV.dll; \
    kill \$JVM_PID 2>/dev/null; \
    exit \$?; \
  else \
    echo '❌ JVM Bridge failed to start' && \
    cat /tmp/jvm-bridge.log && \
    kill \$JVM_PID 2>/dev/null; \
    exit 1; \
  fi"