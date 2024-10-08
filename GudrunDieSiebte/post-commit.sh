#!/bin/bash

# Get current version
version=$(grep "<Version>" GudrunDieSiebte.csproj | sed 's/[^0-9]*//g')
# Increment version
new_version=$((version+1))
# Update version in .csproj file
sed -i "s/<Version>$version<\/Version>/<Version>$new_version<\/Version>/g" GudrunDieSiebte.csproj
