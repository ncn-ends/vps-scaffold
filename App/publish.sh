#!/bin/bash

dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishReadyToRun=true -o ~/code/CliWrapper/out