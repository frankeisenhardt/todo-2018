#!/bin/sh
source ./env.local
echo sync static webfiles to IBM Cloud Object Storage
#s3cmd sync -v --host-bucket=$host_bucket --host=$host_base --access_key=$access_key_id --secret_key=$secret_access_key -v -P -r ./todo-wwwroot/ s3://$bucket_name
s3cmd sync -v --host-bucket=$host_bucket --host=$host_base --access_key=$access_key_id --secret_key=$secret_access_key -v --acl-private -r ./todo-wwwroot/ s3://$bucket_name
