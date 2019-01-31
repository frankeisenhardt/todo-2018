#!/bin/sh
# source env
source ./env.local
ibmcloud ks cluster-service-unbind --cluster $cluster_name --namespace $cluster_namespace --service $cloudant_svc_name
ibmcloud resource service-instance-delete $cloudant_svc_name
kubectl delete -f deploy2kube.yaml
