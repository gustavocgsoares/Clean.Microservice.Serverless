@echo off

dotnet sonarscanner begin /k:"Clean.Microservice.Serverless" /d:sonar.host.url=http://sonar.url /d:sonar.verbose=true
dotnet build
dotnet sonarscanner end