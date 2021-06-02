dotnet sonarscanner begin /k:"SmartSchoolProject" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="b6853302ff552538c5ecc8335cde29d22d69b3f1"
dotnet build
dotnet sonarscanner end /d:sonar.login="b6853302ff552538c5ecc8335cde29d22d69b3f1"