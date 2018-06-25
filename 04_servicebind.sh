#!/bin/sh
# source env
source ./env.local
# check if binding exists
bind=$(bx cs cluster-services $cluster_name | grep $cloudant_svc_name)
echo $bind
if [ -z "$bind" ] ; then
    echo "Cloudant servive not bound to cluster" $cloudant_svc_name
    echo "binding..."
    bx cs cluster-service-bind $cluster_name $cluster_namespace $cloudant_svc_name
else
    echo "Cloudant service already bound to cluster " $cloudant_svc_name
fi
echo "Binding"
bx cs cluster-services $cluster_name | grep $cloudant_svc_name
echo "Secret"
kubectl get secret binding-$cloudant_svc_name --namespace=$cluster_namespace -o json
