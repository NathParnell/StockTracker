# StockTracker

OVERVIEW:

Stock Tracker consists of two applications, a server application and a client application.

The server application is a console application built in .NET 6.0 which can run on Linux, Mac and Windows
The client application is a .Net MAUI Blazor Hybrid application built in .NET 7.0 and can also run on Linux, Mac and Windows. The client application can also run on Android and iOS, however I have not built the application with these platforms in mind and it therefore is not recommended to run on these platforms.


RUNNING THE PROJECT:

To run the application, open either Visual Studio or Rider on your computer and clone the StockTracker Repository (Or open the solution from your files)
Following this, the solution should load within your IDE.
Open a powershell/terminal within your IDE and run the command "dotnet restore" - This will restore all of the nuget packages used within the solution.
I would then recommend that you run the command "dotnet build" which will build the solution.

Following this, you need to configure the IP addresses of some of the services within the client to point to correctly point to the server:
If you are running both the server and the client applications on the same machine, your IP address will be "127.0.0.1".
If you are running the server and client applications on different machines, you will to find out your PRIVATE IP address - on this website you can find out how to do this : "https://www.wikihow.com/Find-Out-Your-IP-Address"

Once you have your private Ip address - if you goto the file "NetmqClientTransportService" (StockTrackerApp/Services/NetmqClientTransportService) and change the value of the variable "SERVER_IP_ADDRESS" to be your private IP address
Then goto the file "BroadcastListenerService" (StockTrackerApp/Services/BroadcastListenerService) and change the value of the variable "PUBLISHER_ENDPOINT" to include your private IP address - "{yourIPAddress:5557}".

Following changing the IP Addresses in these locations, you can now run the application.
To do this, right click on the project StockTrackerServer and press the debug option, and then press "Start New Instance" (or the equivelant on your IDE). This will run the server application.
Then right click on the project StockTrackerApp and do the same to start the client application. (NOTE - you can only run the MAUI application on a mac - you cannot debug it)

NOTE: To build on a mac, you must have iOS installed, which you can do using XCode


DATABASE:

This project is NOT using a conventional database server.
Due to the requirements from the university, We need to mock a database and to do this, I am taking advantage of Entity Framework's "In Memory Database", which creates a new instance of a database every time that the server application is ran.
Entity framework uses ORM which allows you to create data tables based on your objects, to read more about EF, read this article - "https://learn.microsoft.com/en-us/ef/core/"


NETMQ:
This project uses NetMq which is an implementation of Zero Mq build for .NET.
I use the Request/Response model in this solution to handle requests and responses to and from the server. The Request/Response model is also used within the implementation of the private messaging feature.
To handle broadcasting to many users, I use the Publisher/Subscriber model, where clients subscribe to topics and receive broadcast objects from the topics they subscribe to
To learn more about NetMq, here is some of the NetMq documentation - "https://netmq.readthedocs.io/en/latest/introduction/"


ENCRYPTION:
I have implemented symmetric encryption into this project using Microsofts "System.Security.Cryptography" library, which makes the application less vulnerable to attacks like a "man in the middle" attacks, which is necessary for when we are sending sensitive data using TCP and UDP.


HASHING:
I have created a "HashingHelper" class which hashes data. This has not been used within the code as I didnt find it necessary with me storing using very simple passwords within the mock database, but If i were to implement a real database into the project, I would implement this class to increase the security of the program.
To learn more about hashing, read this article - "https://www.techtarget.com/searchdatamanagement/definition/hashing"

UNIT TESTING (and mocking):
I have implemented unit tests into this application using the XUnit unite testing framework. I have implemented unit tests within the applciation using XUnit and I have mocked services using Moq, a library which allows you to mock services. All of the unit tests pass.


