#!/bin/sh
# source env
source ./env.local
rg=$(ibmcloud resource search "type:resource-group AND name:$cloudant_resource_group" | grep "Resource Group ID:" | awk '{print $4}')
svc=$(ibmcloud resource search "(name:$cloudant_svc_name) AND (doc.resource_group_id:$rg)" | grep $cloudant_svc_name)
echo $svc
if [ -z "$svc" ] ; then
    echo "Cloudant servive does not exist creating: " $cloudant_svc_name
    ibmcloud resource service-instance-create  $cloudant_svc_name cloudantnosqldb lite $cloudant_region -p '{"legacyCredentials": true}'
else
    echo "Cloudant service already there: " $cloudant_svc_name
fi
