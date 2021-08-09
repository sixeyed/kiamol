#!/bin/sh

# set up access to Kube API
kubectl config set-cluster default --server=https://kubernetes.default.svc.cluster.local --certificate-authority=/var/run/secrets/kubernetes.io/serviceaccount/ca.crt
kubectl config set-context default --cluster=default
kubectl config set-credentials user --token=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
kubectl config set-context default --user=user
kubectl config use-context default

echo ----------------
echo "Generating user cert - username: $USER_NAME; group: $GROUP"
echo ----------------

openssl genrsa -out user.key 2048
openssl req -new -key user.key -out user.csr -subj "/C=UK/ST=LONDON/L=London/O=${GROUP}/CN=${USER_NAME}"

csr=$(cat user.csr | base64 | tr -d "\n")
sed -i "s/{CSR}/$csr/g" csr.yaml
sed -i "s/{USER_NAME}/$USER_NAME/g" csr.yaml

echo '** CSR generated.'

kubectl apply -f csr.yaml
kubectl certificate approve $USER_NAME

echo '** Cert approved.'

kubectl get csr/$USER_NAME -o json | jq '.status.certificate' -r | base64 -d > user.crt

echo ----------------
echo "Cert generated: /certs/user.key and /certs/user.crt"
echo ----------------

if [ -n "$SET_CONTEXT" ]; then
    kubectl config set-credentials $USER_NAME --client-key=./user.key --client-certificate=./user.crt --embed-certs=true
    kubectl config set-context $USER_NAME --user=$USER_NAME --cluster default
    kubectl config use-context $USER_NAME

    echo "** Using context for user: $USER_NAME; group: $GROUP"
fi

if [ -n "$PRINT_CERTS" ]; then
    echo "user.key"
    cat /certs/user.key
    echo "user.crt"
    cat /certs/user.crt
else 
    while true; do sleep 1000; done
fi