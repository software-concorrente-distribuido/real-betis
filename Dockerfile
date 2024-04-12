FROM ubuntu

RUN apt update
RUN apt --yes install sudo
RUN apt --yes install git
# Setup Dotnet SDK
# Set preferences for Microsoft .NET packages installation.
RUN cd /etc/apt/preferences.d && \
    touch 99microsoft-dotnet.pref && \
    echo 'Package: *' >> 99microsoft-dotnet.pref && \
    echo 'Pin: origin "packages.microsoft.com"' >> 99microsoft-dotnet.pref && \
    echo 'Pin-Priority: 1001' >> 99microsoft-dotnet.pref
# Update packages
RUN apt update
# Install .NET SDK 8.0
RUN apt --yes install dotnet-sdk-8.0
# Setup GCC
RUN apt-get update
RUN apt-get install -y gcc

RUN mkdir /./.projects
