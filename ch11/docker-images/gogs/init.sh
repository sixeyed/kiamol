# /bin/sh

# start Gogs and give it time to spin up
/app/gogs/docker/start.sh & sleep 5

# finsh installation
curl -d @gogs-install.txt http://localhost:3000/install

# create user auth token
curl -q -X POST -H 'Content-Type: application/json' -d '{"name": "api"}' --user kiamol:kiamol http://localhost:3000/api/v1/users/kiamol/tokens > response.json
token=$(cat response.json | jq '.sha1' -r)
rm -f token.json

# create repo
curl -q -X POST -H 'Content-Type: application/json' -d '{
  "name": "kiamol",
  "description": "kiamol source code",
  "private": false
}' "http://localhost:3000/api/v1/user/repos?token=$token"

# move the data from the volume to a directory in the image
cp -r /data /init-data