#!/bin/sh
# source env
source ./env.local
bx target --cf -o $cf_org -s $cf_space
# check if service already exists
svc=$(bx cf services | grep $cloudant_svc_name)
if [ -z "$svc" ] ; then
    echo "Cloudant servive does not exist creating: " $cloudant_svc_name
    bx service create cloudantNoSQLDB Lite $cloudant_svc_name
else
    echo "Cloudant servive already there: " $cloudant_svc_name
fi
