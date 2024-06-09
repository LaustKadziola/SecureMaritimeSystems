#################
##   Mininet   ##
#################
The versions of mininet can be found via this link:
https://github.com/mininet/mininet/releases/
The implementation requires python 3.85 and dotnet 8.0300 therefor the 
minimum required version of ubunto is 20.04


To start Mininet run the following command in the terminal.

sudo python3 Topo.py

This will a CLI to interact with the Mininet. 
A terminal can be opened for a specific host in the mininet by entering the command.

xterm [host name]

For Example "xterm r1" will open the Edge Router and "xterm h1" will open host 1.

##############################
##   Building the project   ##
##############################

The projects in the solution can be build with the command:

dotnet build

The project should be built outside Mininet because there are problems with NuGet packages in the Mininet.

############################
##   Running EdgeRouter   ##
############################

To start the Edge Router run the command in the Mininet host r1:

dotnet run --project MMSEdgeRouter/MMSEdgeRouter.csproj --no-build

The Ip of the Edge Router is currently hard coded, therefore it can only run on host r1.
The ip can be changed by setting the fields HOST and PORT in MMSAgent/Agent.cs and The input to EdgeRouter(host, port) in MMSEdgeRouter/EdgeRouter.cs

#######################
##   Running Agent   ##
#######################

The agent is a class library but there are a selection programs and tests that implement the library

Interactive app:
dotnet run --project AgentApp/AgentApp.csproj --no-build [mrn]

Tests:
dotnet test --no-build

Performance tests:
dotnet run --project PerformanceTest/PerformanceTest.csproj --no-build

Stress hosts:
dotnet run --project StressTest/StressTest.csproj --no-build



