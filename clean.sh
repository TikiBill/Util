#!/bin/bash
# Copyright (c) 2018 Bill Adams. All Rights Reserved.
# Bill Adams licenses this file to you under the MIT license.
# See the license.txt file in the project root for more information.

#Make sure we are working in our source directory
cd $(dirname $BASH_SOURCE)

#Source the variables.
. common.sh


#
#
#------------------------------------------------------------------------------
function clean_bin {
    for SRC_DIR in "src" "test" "benchmark"; do
        for BASE_MODULE in "${DOTNET_MODULE_DIRS[@]}"; do
            for EXTRA in '' '.Test' '.Benchmark'; do
                MODULE="$BASE_MODULE$EXTRA"
                if [ -e "$SRC_DIR/$MODULE" ]; then
                    echo "Cleaning $SRC_DIR/$MODULE"
                    for SUBDIR in bin obj BenchmarkDotNet.Artifacts; do
                        OBJ_DIR="$SRC_DIR/$MODULE/$SUBDIR";
                        if [ -e "$OBJ_DIR" ]; then
                            echo
                            echo "Cleaning $OBJ_DIR"
                            echo "rm -rfv $OBJ_DIR"
                            rm -rfv "$OBJ_DIR"
                            #elif $DEBUG; then
                            #    echo "NOTE: $OBJ_DIR does not exist"
                        fi
                    done
                fi
            done
        done
    done
}


#
#
#------------------------------------------------------------------------------
function clean_temp_files {
    for FILE in $TEMP_FILES; do
        find . -name "$FILE" -print -delete
    done
}

#
#
#==============================================================================

clean_bin;
clean_temp_files;


