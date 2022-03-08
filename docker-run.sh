#!/bin/sh

docker run -it -v C:$(pwd):C:/source \
--user ContainerAdministrator \
--entrypoint "powershell" \
mcr.microsoft.com/dotnet/framework/sdk:4.8 "& ""C:\\source\\publish-app.ps1"""