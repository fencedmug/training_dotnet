== Docker (on wsl)
https://www.digitalocean.com/community/tutorials/how-to-install-and-use-docker-on-ubuntu-20-04
https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux

- to start docker:
sudo service docker start

- to see status:
sudo service docker status

- show active containers
docker ps -a

- show all images
docker images



== webapi

docker build -t webapitest:v1 -f dockerfileDebug .
  -t webapitest:v1      --> tagging
  -f dockerfileDebug    --> tell docker to use this file
  .                     --> use current folder

//this will always create a new container from image
docker run -d -p 8080:80 --name webber webapitest:v1 
  -d              --> detach, don't lock on current console
  -p 8080:80      --> bind local port 8080 to container's port 80
  --name webber   --> give this container a name
  webapitest:v1   --> name of image

//this will always start an existing container
docker start webber

docker stop webber