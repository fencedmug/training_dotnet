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