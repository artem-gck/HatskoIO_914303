# Document management system

A software tool for document management in the commercial environment of an enterprise

## Running

### Requirements:
* [Docker](https://www.docker.com/)
* [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### Running
Create a self-signed certificate by [dotnet command](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide)
Create ServiceBus resource and save connection string.
In the root of project open the .env file and replace the values:
⋅⋅*ServiceBus - primary connection string
⋅⋅*CertPath - path to self-signed certificate
⋅⋅*CertPassword - password to self-signed certificate

Open PowerShell in the root directory and enter the 
`docker-compose build`
after finished, enter the command 
`docker-compose up`


## Structure of project
![image](https://user-images.githubusercontent.com/72604580/210352476-54145a58-7aaa-402a-a8b5-fc9799d20053.png)

## Structure of relational databases
![image](https://user-images.githubusercontent.com/72604580/192766072-5c7f9522-6f1c-48a7-b3c5-357503427cbc.png)

## Microservices structure:

### User info CRUD service
    N-Layer architecture
   
![image](https://user-images.githubusercontent.com/72604580/194308168-6f18c6be-748e-467f-8774-82e22b8db632.png)

### Task CRUD service
    Hexagonal architecture
   
![image](https://user-images.githubusercontent.com/72604580/194329851-3cb14e46-135e-4e99-bbf3-edea32327796.png)

### Structure CRUD service
    Onion architecture
   
![image](https://user-images.githubusercontent.com/72604580/194351055-4fd9b4c8-264a-4fc9-b8c8-d4f2e733c97a.png)

### Document CRUD service
   CQRS pattern
   
![image](https://user-images.githubusercontent.com/72604580/194356313-6676d325-5356-435c-aed0-a14eb4f91159.png)

### Company management service
   N-Layer architecture

![image](https://user-images.githubusercontent.com/72604580/196154263-7ec59b12-4bcb-495a-b725-049b7b5fe42e.png)

### Notification service
   N-Layer architecture
   
![image](https://user-images.githubusercontent.com/72604580/196687373-41b89ac2-74f2-4e68-821b-0ef36d2fc6e7.png)
