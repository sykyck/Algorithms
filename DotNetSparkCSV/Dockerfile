# Use Ubuntu 22.04 stable
FROM ubuntu:22.04

# Install system dependencies & Java 11
RUN apt-get update && \
    apt-get install -y wget apt-transport-https software-properties-common curl openjdk-11-jre-headless && \
    rm -rf /var/lib/apt/lists/*

# Install .NET 8 SDK
RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0 && \
    rm -rf /var/lib/apt/lists/*

# Download Apache Spark 2.4.8 (Hadoop 2.7 version for compatibility)
RUN curl -L https://archive.apache.org/dist/spark/spark-2.4.8/spark-2.4.8-bin-hadoop2.7.tgz \
    | tar -xzC /usr/local/

ENV SPARK_HOME=/usr/local/spark-2.4.8-bin-hadoop2.7
ENV PATH=$SPARK_HOME/bin:$PATH

# Set working directory
WORKDIR /app

# Copy only csproj first for caching
COPY DotNetSparkCSV.csproj .
RUN dotnet restore

# Copy remaining files
COPY . .

# Build the project
RUN dotnet build -c Release

# Entry point
ENTRYPOINT ["dotnet", "run", "--configuration", "Release"]
