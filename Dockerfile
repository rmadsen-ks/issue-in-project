# Set the base image as the .NET 5.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true

# Copy everything and publish the release (publish implicitly restores and builds)
COPY . .
RUN dotnet publish -c Release -o /app --runtime linux-musl-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-amd64
WORKDIR /app
COPY --from=build /app .

# Label the container
LABEL maintainer="Asger Iversen <asger.iversen@gmail.com>"
LABEL repository="https://github.com/AsgerIversen/issue-in-project"

# Label as GitHub action
LABEL com.github.actions.name="issue-in-project"
# Limit to 160 characters
#LABEL com.github.actions.description=""
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="git-pull-request"
LABEL com.github.actions.color="orange"

ENTRYPOINT ["./issue-in-project"]
