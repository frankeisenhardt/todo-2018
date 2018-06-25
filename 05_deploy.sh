#!/bin/sh
# source env
source ./env.sample
cp deploy2kube.template deploy2kube.yaml
# replacements with sed using , syntax because we have slahes in url
sed -i '' "s,@REGISTRY@,${namespace},g" deploy2kube.yaml
