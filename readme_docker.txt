==== Docker (on wsl)
Examples here assume you are running WSL in windows environment
https://www.digitalocean.com/community/tutorials/how-to-install-and-use-docker-on-ubuntu-20-04
https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux

- to start docker (you may need to do this every startup)
sudo service docker start

- to see status
service docker status

- show active containers
docker ps -a

- show all images
docker images

- install docker compose
sudo apt-get update
sudo apt-get install docker-compose-plugin




==== Steps


# Creating webapi and running in docker

- in webapi folder -> create dockerfileDebug

- to build webapi into a docker image
docker build -t webapitest:v1 -f dockerfileDebug .
  -t webapitest:v1      --> tagging
  -f dockerfileDebug    --> tell docker to use this file
  .                     --> use current folder

- to run webapi as a docker container
//this will always create a new container from image
docker run -d --rm -p 8080:80 --name webber webapitest:v1 
  -d              --> detach, don't lock on current console
  --rm            --> auto remove container when exit
  -p 8080:80      --> bind local port 8080 to container's port 80
  --name webber   --> give this container a name
  webapitest:v1   --> name of image

- run webapi from an existing docker container
docker start webber

- stop webapi's docker container
docker stop webber




# Creating calculator api and running in docker compose

- in root folder -> create docker-compose.yml

- to build images (doesn't seem to auto detect changes in dockerfile)
docker compose build

- run containers defined in docker compose
docker compose up -d
  -d  --> detach mode

- to stop docker compose
docker compose down