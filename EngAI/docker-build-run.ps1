# Set variables
$imageName = "default/eng-ai"
$containerName = "eng-ai-container"
$port = 8081

# Optional: Clean previous containers and images
docker rm -f $containerName
docker rmi $imageName

# Build the Docker image
Write-Host "Building Docker image..."
docker build -t $imageName .
docker tag default/eng-ai:latest 149536470355.dkr.ecr.ap-southeast-1.amazonaws.com/default/eng-ai:latest

if ($LASTEXITCODE -ne 0) {
    Write-Error "Docker build failed. Exiting."
    exit 1
}

# Run the Docker container
Write-Host "Running Docker container..."
docker run -d -p $port:8080 --name $containerName $imageName

# Confirm
Write-Host "App is running at: http://localhost:$port"