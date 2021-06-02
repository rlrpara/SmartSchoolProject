dotnet sonarscanner begin /k:"SmartSchoolProject" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="61dce71a2fc3e179fb6e7a887b55e5347bfbc6be"
dotnet build
dotnet sonarscanner end /d:sonar.login="61dce71a2fc3e179fb6e7a887b55e5347bfbc6be"