#!/bin/sh

buildpkg.sh
nuget push HalideSharp.`cat version.txt`.nupkg
