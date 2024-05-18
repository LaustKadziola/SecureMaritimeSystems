#!/bin/bash

SRC_DIR=.
DST_DIR=.

#protoc -I=$SRC_DIR --python_out=$DST_DIR $SRC_DIR/messages.proto

protoc --proto_path=$SRC_DIR --csharp_out=$DST_DIR messages3.proto

#--csharp_opt=base_namespace=Mmtp 