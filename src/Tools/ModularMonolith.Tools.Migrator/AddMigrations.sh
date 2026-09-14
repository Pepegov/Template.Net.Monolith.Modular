#!/usr/bin/env bash
# Создание начальных миграций по модулям в правильной последовательности.
# Запускать из корня репозитория (там же, где ModularMonolith.sln).
set -euo pipefail

# 1) Модуль Template — создаёт таблицу Templates.
dotnet ef migrations add Initial \
  --context TemplateDbContext \
  --project ModularMonolith.Module.Template.DataAccess.Sqlite \
  --startup-project ModularMonolith.Web

# 2) Модуль Builder — зависит от Template.Templates для FK по TemplateId.
#    ВАЖНО: после генерации миграции Builder вручную добавьте в её Up() raw-SQL FK:
#      migrationBuilder.Sql("ALTER TABLE \"Builders\" ADD CONSTRAINT ..." +
#      " FOREIGN KEY (\"TemplateId\") REFERENCES \"Templates\" (\"Id\") ON DELETE RESTRICT;");
#    (SQLite игнорирует схему EF, поэтому таблица называется \"Templates\", а не \"Template\".\"Templates\".)
dotnet ef migrations add Initial \
  --context BuilderDbContext \
  --project ModularMonolith.Module.Builder.DataAccess.Sqlite \
  --startup-project ModularMonolith.Web
