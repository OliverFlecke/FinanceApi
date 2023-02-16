#!/usr/bin/env sh

docker cp data.sql $1:data.sql
docker exec -it $1 psql -U postgres -f data.sql
