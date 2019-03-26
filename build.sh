#!/bin/bash
# Copyright (c) 2018 Bill Adams. All Rights Reserved.
# Bill Adams licenses this file to you under the MIT license.
# See the license.txt file in the project root for more information.

#Make sure we are working in our source directory
cd $(dirname $BASH_SOURCE)

#Source the variables.
. common.sh

#
function build_dotnet {
    ROOT_DIR="$PWD"
    echo "Build dotnet, starting in $ROOT_DIR"
    for SRC_DIR in src test; do
        for EXTRA in '' '.Test' '.Benchmark'; do
            #Build all of the non .Test modules first.
            for BASE_MODULE in ${DOTNET_MODULE_DIRS}; do
                MODULE="$BASE_MODULE$EXTRA"
                OBJ_DIR="$SRC_DIR/$MODULE";
                if ! cd $ROOT_DIR; then
                    echo "ERROR: Could not cd $ROOT_DIR ??"
                    exit 1  #Always fatal, we came from this directory.
                fi
                
                if [ -e "$OBJ_DIR" ]; then
                    if ! cd $OBJ_DIR; then
                        echo "ERROR Could not cd $OBJ_DIR"
                        ERROR_LOG+=("Could not cd $OBJ_DIR");
                        ERROR_COUNT=$(($ERROR_COUNT+1))
                        if $ERRORS_ARE_FATAL; then exit 1; fi
                    fi
                    
                    echo
                    echo "******************** $SRC_DIR/$MODULE *****************"
                    
                    if [ "$NUPKG_SUBDIR" != "" -a -d "$NUPKG_SUBDIR" ]; then
                        rm -fv $NUPKG_SUBDIR/*.nupkg
                    fi
                    
                    if ! dotnet restore; then
                        #Technically modern versions of dotnet will, by default do the restore
                        #  along with the build. However I want finer grained failure
                        #  messages.
                        ERROR_COUNT=$(($ERROR_COUNT+1))
                        ERROR_LOG+=("dotnet restore failed (in $PWD)")
                        echo
                        echo "*************************************************";
                        echo "ERROR dotnet restore failed."
                        echo "Try:"
                        echo "cd $PWD"
                        echo "dotnet restore"
                        echo "*************************************************";
                        echo
                        if $ERRORS_ARE_FATAL; then exit 1; fi
                    fi
                    
                    if ! dotnet build -c $BUILD_CONFIG; then
                        ERROR_COUNT=$(($ERROR_COUNT+1))
                        ERROR_LOG+=("dotnet build failed (in $PWD)")
                        echo
                        echo "*************************************************";
                        echo "ERROR dotnet build failed."
                        echo "Try:"
                        echo "cd $PWD"
                        echo "dotnet build"
                        echo "*************************************************";
                        echo
                        if $ERRORS_ARE_FATAL; then exit 1; fi
                    fi
                    
                    if [ "$SRC_DIR" = "test" -o "$SRC_DIR" = "benchmark" ]; then
                        echo "NOTE: Not packing $SRC_DIR code."
                        elif ! dotnet pack -c $BUILD_CONFIG; then
                        ERROR_COUNT=$(($ERROR_COUNT+1))
                        ERROR_LOG+=("dotnet pack failed (in $PWD)")
                        echo
                        echo "*************************************************";
                        echo "ERROR dotnet pack failed."
                        echo "Try:"
                        echo "cd $PWD"
                        echo "dotnet pack"
                        echo "*************************************************";
                        echo
                        if $ERRORS_ARE_FATAL; then exit 1; fi
                        elif [ "$LOCAL_NUGET_DIR" = "" ]; then
                        echo "No LOCAL_NUGET_DIR -- Not pushing packages."
                        elif [ "$LOCAL_NUGET_DIR" = "NONE" ]; then
                        echo "LOCAL_NUGET_DIR = NONE -- Not pushing packages"
                        elif [ -d "$LOCAL_NUGET_DIR" ]; then
                        for NUPKG in $NUPKG_SUBDIR/*.nupkg; do
                            if [ -e "$NUPKG" ]; then
                                if dotnet nuget push -s $LOCAL_NUGET_DIR $NUPKG; then
                                    EVENT_LOG+=("$OBJ_DIR/$NUPKG -> $LOCAL_NUGET_DIR")
                                else
                                    echo "FAILED to dotnet nuget push -s $LOCAL_NUGET_DIR $NUPKG"
                                    ERROR_COUNT=$(($ERROR_COUNT+1))
                                    ERROR_LOG+=("dotnet nuget push -s $LOCAL_NUGET_DIR $NUPKG failed (in $PWD)")
                                    if $ERRORS_ARE_FATAL; then exit 1; fi
                                fi
                            fi
                        done
                    else
                        echo "WARNING: The local NuGet directory $LOCAL_NUGET_DIR does not exist!"
                        if $ERRORS_ARE_FATAL; then exit 1; fi
                    fi
                fi
            done
        done
    done
    cd $ROOT_DIR
}


build_dotnet;
echo
for EVENT in "${EVENT_LOG}"; do
    echo "    $EVENT"
done

if [ ${#ERROR_LOG[@]} -gt 0 ]; then
    echo
    for ERROR in "${ERROR_LOG}"; do
        echo "  ERROR    $ERROR";
    done
    echo
    echo "ERROR: There were ${#ERROR_LOG[@]} errors."
    exit 1
fi
