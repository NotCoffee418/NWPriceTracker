FROM postgres:alpine
ADD NWPriceTracker.Database/scripts/0001_init.sql /docker-entrypoint-initdb.d
RUN chmod a+r /docker-entrypoint-initdb.d/*
EXPOSE 5431