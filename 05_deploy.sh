#!/bin/sh
# source env
source ./env.local
cp deploy2kube.template deploy2kube.yaml
# replacements with sed using , syntax because we have slahes in url
sed -i '' "s,@REGISTRY@,${namespace},g" deploy2kube.yaml
sed -i '' "s,@SVCNAME@,${cloudant_svc_name},g" deploy2kube.yaml
sed -i '' "s,@S3_ACCESS_KEY@,${access_key_id},g" deploy2kube.yaml
sed -i '' "s,@S3_SECRET_KEY@,${secret_access_key},g" deploy2kube.yaml
sed -i '' "s,@S3_BUCKET@,${bucket_name},g" deploy2kube.yaml
sed -i '' "s,@COS_ENDPOINT@,${host_base},g" deploy2kube.yaml
sed -i '' "s,@COS_URI@,${bucket_url},g" deploy2kube.yaml
sed -i '' "s,@NGINX_LOCATION@,"/",g" deploy2kube.yaml
sed -i '' "s,@INGRESS_SUBDOMAIN@,${ingress_subdomain},g" deploy2kube.yaml
sed -i '' "s,@INGRESS_SECRET@,${ingress_secret},g" deploy2kube.yaml
