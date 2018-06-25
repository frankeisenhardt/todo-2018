# todo-2018
Example Todo application on Kubernetes using ASP Core .NET 2.1, Cloudant NoSQL DB  and IBM Cloud Object Storage 

## Description
This is a small todo application with the following components:
* Cloudant NoSQL Database => storage of todo items
* ASP Core .NET 2.1 REST Service => microservice for CRUD of todo items
* IBM Cloud Object Storage  => S3 compatible storage of static webfile content with private HMAC bucket credentials
* OpenResty/NGINX Caching Webserver => caching static webcontent proxy for api  

## Prerequisites:
* Paid IBM Cloud Kubernetes Custer
* local git, docker and kubectl CLIs installed
* IBM Cloud Object Storage Instance with bucket and HMAC credentials https://console.bluemix.net/docs/services/cloud-object-storage/hmac/credentials.html#using-hmac-credentials
* s3cmd installed, separate configuration not needed, everything needed will be passed by 01_s3sync.sh script http://s3tools.org/s3cmd

## Installation on IBM Cloud Kuberntes Service
1. clone this repo
```shell
git clone 
```
2. create environment source file
```shell
cp env.local.sample env.local
```
4. adjust settings in env.local to match your COS and Kubernetes Settings

4. Sync static webcontent to your bucket
```shell
./01_s3sync.sh
```
execute 02_buildimages.sh this will build the Docker image and push to IBM Cloud Container Registry Service
execute 03_deploy.sh this will generate a deploy2kube.yaml
kubect create -f deploy2kube.yaml
curl -I http://YOURWORKERNODEIP:31200/index.html output will contain X-Cache: MISS
run again curl -I http://YOURWORKERNODEIP:31200/index.html output will contain X-Cache: HIT


TODO:

Build Helmchart
Document example

