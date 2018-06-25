#!/bin/sh
source ./env.local
echo "Build todo ASP.NET Core 2.1 API Server"
echo ${namespace}
image1=${namespace}/todo-api
docker build -t $image1 ./todo-api
docker push $image1

echo "Build nginx"
echo ${namespace}
image2=${namespace}/todo-nginx
docker build -t $image2 ./openresty-cos/docker
docker push $image2
