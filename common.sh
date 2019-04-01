#!/bin/bash
# Copyright (c) 2018 Bill Adams. All Rights Reserved.
# Bill Adams licenses this file to you under the MIT license.
# See the license.txt file in the project root for more information.

# Debug output in the scripts?
DEBUG=true

# Some verbose output? Set to true if debug is true as well! (echo statements do not check both)
VERBOSE=true

# Debug or Release build.
BUILD_CONFIG="Release"

# Counter for if errors are not fatal.
ERROR_COUNT=0

# When true, the build script will exit on any error.
ERRORS_ARE_FATAL=true

#A bash array for the things that completed which should be reported at the end,
#  i.e. locally published packages.
EVENT_LOG=()

# Same for errors so if errors are not fatal we print out some useful information at the end.
ERROR_LOG=()

#What are temporary files that appear and should be removed on clean?
# *~ are files left behind by emacs/xemacs.
TEMP_FILES="*~ project.lock"

NUPKG_SUBDIR="bin/$BUILD_CONFIG"

for DIR in "$APPDATA" "$HOME/AppData/Roaming"; do
    if [ "$DIR" = "" ]; then continue; fi
    if [ -e "$DIR/NuGet/NuGet.config" ]; then
        NUGET_CONFIG="$DIR/NuGet/NuGet.config"
        break
    fi
done
if $VERBOSE; then echo "NuGet Config: $NUGET_CONFIG"; fi

if [ "$LOCAL_NUGET_DIR" != "" ]; then
    if $VERBOSE; then echo "Good! Have LOCAL_NUGET_DIR from the environment ($LOCAL_NUGET_DIR)"; fi
    elif [ -e  "$NUGET_CONFIG" ]; then
    # Rough way to figure out where the local NuGet repository is. Just searches NuGet.config for
    #   the first directory that exists, without regard as to the actual XML syntax.
    if $VERBOSE; then echo "Try to figure out a local NuGet directory from $NUGET_CONFIG"; fi
    for VALUE in $(grep -oE 'value *= *"[^"]+"' "$NUGET_CONFIG"); do
        # git-bash handles c:\ so we do not need to tr the slashes nor c:
        DIR=`echo $VALUE | awk -F \" '{print $2}'`
        if $DEBUG; then echo "LINE: $VALUE -> DIR $DIR"; fi
        if [ -d "$DIR" ]; then
            if $VERBOSE; then echo "Local NuGet Directory Being Used: $DIR"; fi
            LOCAL_NUGET_DIR="$DIR"
            break
        fi
    done
fi

if [ "$LOCAL_NUGET_DIR" = "" ]; then
    echo "Cannot figure out where to put NuGet packages Try setting LOCAL_NUGET_DIR"
    echo "  Set LOCAL_NUGET_DIR to NONE to suppress these messages."
fi

DOTNET_MODULE_DIRS=()
while read LANGUAGE TYPE DIR; do
    if [ "$DIR" = "" ]; then
        # Comment line, or mal-formed line.
        continue
    fi
    
    if $DEBUG; then echo "    [$LANGUAGE]  [$TYPE]  [$DIR]"; fi
    if [ "$LANGUAGE" = "dotnet" -o "$LANGUAGE" = "c#" -o "$LANGUAGE" = "C#" ]; then
        if $DEBUG; then echo "      Adding $DIR to dotnet modules."; fi
        DOTNET_MODULE_DIRS+=("$DIR")
    fi
done <<< $(cat modules.txt|awk -F '#' '{print $1}')

function join_by { local IFS="$1"; shift; echo "$*"; }