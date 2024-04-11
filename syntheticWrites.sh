#!/bin/bash

# Define the command to run
command="dotnet run 2 2"

# Change directory to the application directory
cd /workspaces/mirroring-demo/setDataApp

# Run the command 2 times in parallel
for i in {1..2}
do
    echo "Starting thread $i"
    $command & # The & operator runs the command in the background
done

# Wait for all background processes to finish
wait