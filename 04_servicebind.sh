#!/bin/sh
# source env
source ./env.local
# check if binding exists
bind=$(ibmcloud ks cluster-services $cluster_name | grep $cloudant_svc_name)
echo $bind
if [ -z "$bind" ] ; then
    echo "Cloudant servive not bound to cluster" $cloudant_svc_name
    echo "binding..."
    ibmcloud ks cluster-service-bind --cluster $cluster_name --namespace $cluster_namespace --service $cloudant_svc_name --role Manager
else
    echo "Cloudant service already bound to cluster " $cloudant_svc_name
fi
