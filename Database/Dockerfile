FROM mcr.microsoft.com/mssql/server:2017-latest

# Install Unzip
RUN apt-get update && apt-get install unzip -y

WORKDIR /tmp/migration

# Install SQLPackage for Linux and make it executable
RUN wget -q -O sqlpackage.zip https://go.microsoft.com/fwlink/?linkid=873926 \
    && unzip -qq sqlpackage.zip -d /opt/sqlpackage \
    && chmod +x /opt/sqlpackage/sqlpackage

WORKDIR /