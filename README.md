# DynamicModelReflectorLinq

Welcome to the DynamicModelReflectorLinq project, a robust DataAccess Layer designed to streamline interactions between your applications and databases. Inspired by Entity Framework, DynamicModelReflectorLinq leverages ADO.NET to offer a flexible and powerful approach to data management while strictly adhering to SOLID design principles.

## Features

- **Entity-like Framework**: Provides an API that mimics Entity Framework, making it easy for developers familiar with Entity Framework to use and integrate.
- **ADO.NET Backend**: Utilizes ADO.NET for efficient, reliable database connectivity and operations, ensuring high performance and scalability.
- **SOLID Principles**: Developed with a strong focus on SOLID principles to ensure that the architecture is easy to maintain, extend, and adapt.
- **Easy Configuration**: Simplifies configuration processes, allowing developers to set up and start interacting with the database quickly.
- **High Performance**: Optimized for performance, making it suitable for high-load environments where database interaction is a critical factor.
- **Extensible**: Designed to be flexible and extensible, allowing developers to customize and extend functionalities to fit their specific requirements.
- **Dependency Injection**: Developed with dependency injection in mind, promoting a modular and testable architecture.

## Example Usage

Here is how you can use DynamicModelReflectorLinq to interact with databases. This example demonstrates both the simplicity and power provided by the framework, akin to Entity Framework but utilizing ADO.NET.

```csharp
// Initialize the SqlModelReflector with required dependencies
// Dependency injection is enabled throughout the application by designing all constructors to request dependencies via interfaces. This approach ensures flexibility and testability of components.
IModelReflector reflector = new SqlModelReflector(
    new SqlDataOperations(), 
    new SqlQueryBuilder(new SqlDataOperationHelper())
);

// Example of loading a single category
Category category = new();
reflector
    .Load(category)
    .Execute();

// Example of loading multiple users named "John" into a list
List<UserInformation> usersNamedJohn = new();
reflector
    .Load(usersNamedJohn)
    .Where(user => user.UserName == "John")
    .Execute();

// Example of creating a new user in the database
public void CreateNewUser(UserInformation userInfo)
{
    reflector
        .Create(userInfo)
        .Execute();
}

// Example of updating user information in the database
public void UpdateUserInformation(UserInformation userInfo, string userEmail)
{
    reflector
        .Update(userInfo)
        .Where(user => user.Email == userEmail)
        .Execute();
}
