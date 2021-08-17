#!/bin/bash

ROOT_DIR=/app

# Replace env vars in JavaScript files
echo "Replacing env variables in appsetting"
for file in $ROOT_DIR/appsettings.json;
do
  echo "Processing $file ...";

  sed -i 's|HIS_APP_ACCESS_TOKEN_ENCRYPTION_KEY|'${HIS_APP_ACCESS_TOKEN_ENCRYPTION_KEY}'|g' $file 
  sed -i 's|HIS_APP_MSSQL_DB_HOST|'${HIS_APP_MSSQL_DB_HOST}'|g' $file
  sed -i 's|HIS_APP_MSSQL_DB_NAME|'${HIS_APP_MSSQL_DB_NAME}'|g' $file
  sed -i 's|HIS_APP_MSSQL_DB_USER|'${HIS_APP_MSSQL_DB_USER}'|g' $file
  sed -i 's|HIS_APP_MSSQL_DB_PASSWORD|'${HIS_APP_MSSQL_DB_PASSWORD}'|g' $file
done

echo "Starting dotnet..."
dotnet Api.dll