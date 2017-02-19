# SuperLogger

SuperLogger is a .Net C# logging solution aiming high avaiability, performance and load balance for log information.

The project idea come a requirement to provide an assynchronous logging mechanisn for .Net solution and Dynamic CRM Plugins and Workflow Activities.
The idea came from the limitation where CRM Online does not allow C# code to access the file system making really hard to use logging APIs lke Log4Net.
The Log4Net API is flexible enough to allow the creation of appenders however I decided to create a specific logging API to provide some functionality required for the projects I am working on.
I hope this project will help someone out there :P

# The solution is devided into the following components:
1. C# client API;
2. Message bus (Rabbit MQ);
3. Windows Logging Service;
4. Database (SQL server);

## C# Client API
It is the front-end of this project and offer static methods to log (INFO, WARNING and ERROR).
For each log request a message is published to the message bus.
The source parameter is required and each source will be created the first time is used.

Examples:
// Simple INFO log
SuperLoggerClient.Info("MySource", "Loggin Message");

// Using a Correlation ID
SuperLoggerClient.CorrelationID = Guid.NewGuid().ToString(); // After this point all log request will have the same Correlation ID
SuperLoggerClient.Info("MySource", "Loggin Message");

// Providing a parameters from a IDictionary
IDictionary<string, string> data = new Dictionary<string, string>();
data.Add(new KeyValuePair<string, string>("Param 1", "Value 1"));
data.Add(new KeyValuePair<string, string>("Param 2", "Value 2"));
data.Add(new KeyValuePair<string, string>("Param 3", "Value 3"));
SuperLoggerClient.Info("MySource", "My logging message", data);

## Message bus (Rabbit MQ)
The message bus is responsible to collect all logging requests and store them until they are fully processed by the integration service and stored into the database.

Example of a message payload:
{
	"LogType": "I",
	"CorrelationID": "b376e829-13ee-4411-81b8-14a689789072",
	"CreatedOn": "\"2017-02-19T00:03:09.4268727Z\"",
	"Source": "MySource",
	"Message": "Info message: 2017-02-18 4:03:09 PM",
	"StackTrace": null,
	"Data": {
		"Param 1": "Value 1",
		"Param 2": "Value 2",
		"Param 3": "Value 3"
	}
}

## Windows Logging Service
It is a windows service that subscribes to the message bus and process all valid messages storing them into the database.
Every message processing is a transaction and valid messages are only removed from the queue after been stored into the database.
The message payload is represented by a JSON object and invalid messages will be removed from the queue.

## Database
As expected the database is where all the logging request will land and there are only three tables: LogSource, Log and LogData.
The tables and fields are listed below:
LogSource: ID and Name
Log: ID, CreatedOn, Source (LogSource FK), Type ('I' = Info; 'W' = Warning; 'E' = Error), Message, Correlation ID, StackTrace
LogData: ID, Name, Value and Log (Log FK)

# Installation instructions

