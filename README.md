# Manect
Task-сайт и телеграм-бот, созданные, для контроля над проектами которые ведут сотрудники в кампании.
# Demo


# Usage
В файле appsetings.json поменять строку подкючения к вашей базе данных.
Для создания таблиц в базе данных в окне терминала, запустим команды.

`dotnet ef migrations add createMigration --project src/Manect --context IdentityDbContext --output-dir Migrations/IdentityDbContext`  
`dotnet ef migrations add createMigration --project src/Manect --context ProjectDbContext --output-dir Migrations/ProjectDbContext`  
`dotnet ef database update --project src/Manect --context IdentityDbContext`     
`dotnet ef database update --project src/Manect --context ProjectDbContext`