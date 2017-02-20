# SuperLogger
The idea for this project come from a requirement to provide an asynchronous, high availability, high performance and load balance logging mechanism for .Net solution and Dynamic CRM (Plugins and Workflow Activities). The goal is to provide a logging solution that can handle thousands of logging requests simultaneously and impacting as less as possible the client applications.

**I really hope this project will help someone out there :P**

The solution is devided into the following components:

1. C# client API;
2. Message bus (Rabbit MQ);
3. Windows Logging Service;
4. Database (SQL server);

## C# Client API
It is the front-end of this project and offer static methods to log (INFO, WARNING and ERROR).
For each log request a message is published to the message bus.
The source parameter is required and each source will be created the first time is used.

Examples:
```cs
// Simple INFO log
SuperLoggerClient.Info("MySource", "Logging Message");

// Using a Correlation ID
SuperLoggerClient.CorrelationID = Guid.NewGuid().ToString(); // After this point all log requests will have the same Correlation ID
SuperLoggerClient.Info("MySource", "Logging Message");

// Providing parameters from a IDictionary
IDictionary<string, string> data = new Dictionary<string, string>();
data.Add(new KeyValuePair<string, string>("Param 1", "Value 1"));
data.Add(new KeyValuePair<string, string>("Param 2", "Value 2"));
data.Add(new KeyValuePair<string, string>("Param 3", "Value 3"));
SuperLoggerClient.Info("MySource", "My logging message", data);

// Indicating to capture exceptions
try {
   SuperLoggerClient.Info("MySource", "Logging Message", null, true);
} catch (Exception e) {
   Console.WriteLine("Error logging");
}

```

## Message bus (Rabbit MQ)
The message bus is responsible to collect all logging requests and store them until they are fully processed by the integration service and stored into the database.

Example of a message payload:

```json
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
```

## Windows Logging Service
It is a windows service that subscribes to the message bus and process all valid messages storing them into the database.
Every message processing is a transaction and valid messages are only removed from the queue after been stored into the database.
The message payload is represented by a JSON object and invalid messages will be removed from the queue.

## Database
As expected the database is where all the logging request will land and there are only three tables: LogSource, Log and LogData.
The SQL operation are implemented as stored procedures aiming for max performance.
The tables and fields are listed below:
```
LogSource: 
    ID
    Name

Log: 
    ID
    CreatedOn
    Source (LogSource FK)
    Type ('I' = Info; 'W' = Warning; 'E' = Error)
    Message
    Correlation ID
    StackTrace

LogData: 
    ID
    Name
    Value
    Log (Log FK)
```

# How to use

1. Create a SQL Server Database and execute the SQL statement from the DatabaseScripts.sql file.
   - That will create the tables and stored procedures.
2. Build the solution to generate the projects DLLs
3. Install RabbitMQ
   - Instructions here: https://www.rabbitmq.com/download.html
4. Setup the RabbitMQ 
   - Instructions about how to setup the RMQ Management plugin here: https://www.rabbitmq.com/management.html
   - Create a fanout exchange. e.g. SuperLogger.fanout
   - Create a queue. e.g. SuperLogger
   - Bind the fanout to the queue. 
   - Remember to take note of the routing key. e.g. super_logger
5. Update the Windows Service app.config file
   - Add the correct information on the Rabbit MQ key
   - Add the correct information on the SQL Server connection string key
6. Install the windows Service
   - Instructions here: https://msdn.microsoft.com/en-us/library/sd8zc8ha(v=vs.110).aspx
7. User the client API
   - Add the following key to you client application configuration file
```xml
<add key="SuperLogger.RMQ.Settings"
  value='{ "HostName"    : "localhost",
           "VirtualHost" : "/",
           "Port"        : 5672,
           "Exchange"    : "SuperLogger.fanout",
           "UserName"    : "guest",
           "Password"    : "Myrmq",
           "RoutingKey"  : "super_logger",
           "Queue"       : "SuperLogger"}'/>
```
# TODO list

1. Review all the TODOs from the source code;
2. Add the option to call the log method asynchronous;
3. Finish the unit testing;
4. Prepare a stress test;
