docker-compose up --build
Docker Compose will:
Force a rebuild of all images (based on your Dockerfiles).
Then start the containers with the freshly built images.

docker-compose run --rm dotnetkafka
This means:
Run the service named dotnetkafka (from your docker-compose.yml).
When the container exits, Docker will automatically delete it — including its filesystem layer, logs, etc.
