cd C:/source 
# nuget restore
# dotnet msbuild JoggingTracker.sln /t:Build /p:SelfContained=True /p:PublishProtocol=FileSystem /p:Configuration=Release /p:PublishDir=publish /p:PublishReadyToRun=False /p:PublishTrimmed=False
msbuild JoggingTracker/JoggingTracker.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile
# msbuild JoggingTracker.sln /p:DeployOnBuild=true /p:PublishProfile=FolderProfile
# dotnet publish JoggingTracker.sln /p:RestorePackagesConfig=true /p:PublishProfile=FolderProfile