$images=$(yq e '.services.[].image' docker-compose.yml)

foreach ($image in $images)
{    
    docker manifest create --amend $image `
      "$($image)-linux-arm64" `
      "$($image)-linux-amd64"
    
    docker manifest push $image
}
