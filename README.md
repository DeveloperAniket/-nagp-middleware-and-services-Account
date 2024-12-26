# MiddlewareServices_GPRC_RabbitMQ
This Project demonstrates a microservices architecture with services for Product, Order, and Notification using RabbitMq and gRPC for communication.

# **Pre-requisites and assumptions**
Following softwares should be installed
- .NET 8
- Docker
- RabbitMq server running 

 
Note ::   Start RabbitMQ On local
 

# Steps to run the solution:
1. Clone the project.
2. Start RabbitMQ on Local.
3. On the root clone folder there is bat file . Run that inside command line.
`.\start.bat` .This will open all 4 application in 4 different cmd window. All the window will be set with respective app title.
4. Open Account Web Api app window. copy port-number of the ProductApi swagger. 
 ``` example :: 'http://localhost:5001/swagger/index.html' ```
 

# Product API: 
| API Method        | Method Type           | Details  |  
| :------------- |:-------------| :------------- | 
| Accounts/statement/{AccountNumber}     | HTTP GET | Get Accunt Statement Details | 
| Accounts/statement/PDF/{AccountNumber}      |  HTTP Get      |  Create a request for Statement Pdf Generate |
| Accounts/Create | HTTP POST      |   Add a new Account |

[API POSTMAN collection Link](AccountService.postman_collection.json)
