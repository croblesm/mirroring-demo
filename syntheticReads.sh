#!/bin/bash

# Define the command to run
command="dotnet run 10 10000"

# Change directory to the application directory
cd /workspaces/mirroring-demo/getDataApp

# Run the command 6 times in parallel
for i in {1..6}
do
    echo "Starting thread $i"
    $command & # The & operator runs the command in the background
done

# Wait for all background processes to finish
wait