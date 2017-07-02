# postgres1
Sample .Net Core app with EntityFrameworkCore

Простой пример кроссплатформенного .Net Core приложения, использующего Entity Framework Core.
В качестве базы используется Postgres. Клиент - Npgsql.EntityFrameworkCore.
Строка соединения хранится в файле "connection". По понятным причинам файл в репозиторий не помещён.
Вам нужно создать файл "connection" валидной строкой соединения вида:
Server={0};Database={1};User Id={2};Password={3};Port={4}
{0} - адрес Postgres сервера
{1} - название БД
{2} - логин пользователя
{3} - пароль пользователя
{4} - порт адреса
