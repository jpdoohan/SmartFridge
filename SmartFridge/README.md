# DotNet 9 Base MVC Solution

## Data Project

The Data project encapsulates all data related concerns and provides data entity (database) and implementations of following services:

1. An implementation of ```SmartFridge.Data.Services.IUserService``` using EntityFramework to handle data storage/retrieval is provided via ```SmartFridge.Data.Services.UserServiceDb```.
2. An implementation of ```SmartFridge.Data.Services.IMailService``` using the .NET Smtp Mail provider to handle email sending is provided via ```SmartFridge.Data.Services.SmtpMailService```.

Password hashing functionality is added via the ```SmartFridge.Data.Security.Hasher``` class. This is used in the Data project ```UserServiceDb``` to hash the user password before storing in database.

Data pagination is supported via the ```Paged<T>``` data-type. To create paged data from a query we can use the ```ToPaged(...)``` extension method. See ```UserService.GetUsers(....)``` method for a usage example. 

## Test Project

The Test project references the Core and Data projects and should implement unit tests to test any service implementations created in the Data project. A sample test file is provided for implementation of IUserService. You should provide your own tests to exercise the functionality of any services you create.

Test project also supports integration tests to verify operation of controllers. See `HomeControllerIntegrationTests` for example.


## Web Project

The Web project uses the MVC pattern to implement a web application. It references the Data project and uses the exposed services and models to access data management functionality. This allows the Web project to be completely independent of the service implementation details defined in the Data project.

### Dependency Injection

The DI container is used to manage creation of services that are consumed in the project and are configured in ```Program.cs```.

The EntityFramework ```DbContext``` can be configured to use either ```Sqlite``` (default), ```MySql```, ```Postgres``` or ```SqlServer``` databases. Connection strings for each database should be configured in ```appsettings.json```. Default configuration example is shown below.

```c#
builder.Services.AddDbContext<DatabaseContext>( options => {
    // Configure connection string for selected database in appsettings.json
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));   
});
```

Implementations of ```IUserService``` and ```IMailService``` defined in the data project, ```UserServiceDb``` and ```SmtpMailService``` are also added to the DI container as shown below.

```c#
// Add Application Services to DI   
builder.Services.AddTransient<IUserService,UserServiceDb>();
builder.Services.AddTransient<IMailService,SmtpMailService>();
```

* Database ConnectionString and MailSettings should be configured as required in ```appsettings.json```*

### Identity

The project provides extension methods to enable:

1. User Identity using cookie authentication is enabled without using the boilerplate smartfridge used in the standard web projects (mvc,web). This allows the developer to gain a better appreciation of how Identity is implemented. The core project implements a User model and the data project UserService implementation provides user management functionality such as Authenticate, Register, Change Password, Update Profile etc.

The Web project implements a UserController with actions for Login/Register/NotAuthorized/NotAuthenticated etc. These are implemented using the ```IUserService``` outlined above. The ```AuthBuilder``` helper class defined in ```SmartFridge.Web.Helpers``` provides a ```BuildClaimsPrinciple``` method to build a set of user claims for User Login action when using cookie authentication and this can be modified to amend the claims added to the cookie.

To enable cookie Authentication the following statement is included in Program.cs.

```c#
builder.Services.AddCookieAuthentication();
```

Then Authentication/Authorisation are then turned on in the Application via the following statements in Program.cs

```c#
app.UseAuthentication();
app.UseAuthorization();
```

### Additional Functionality

1. Any Controller that inherits from the Web project BaseController, can utilise:

    a. The Alert functionality. Alerts can be used to display alert messages following controller actions. Review the UserController for an example using alerts.

    ```Alert("The User Was Registered Successfully", AlertType.info);```

2. A ClaimsPrincipal authentication extension method
    * ```User.GetSignedInUserId()``` - returns Id of current logged in user or 0 if not logged in

3. Custom TagHelpers are included that provide

    a. Conditional Display Tag

    * ```<p asp-condtion="@some_boolean_expression">Only displayed if the condition is true</p>```

    Note: this can be used with claims principal extension method above to conditionally hide/display UI elements depending on whether a user has a specific role as shown below:

    ```
    <div asp-condtion="@User.HasOneOfRoles("rolea, roleb")"> ... </div>
    ```

4. A Breadcrumbs partial view is contained in ```Views/Shared/_Breadcrumbs.cshtml``` and can be added to a View as shown in example below. The the model parameter is an array of tuples containing the route and breadcrumb.

    ```c#
    <partial name="_BreadCrumbs" model=@(new [] {
        ("/","Home"),
        ("/student","Students"),
        ($"/student/details/{Model.Id}",$"{Model.Id}"),
        ("","Details")
    }) />
    ```

5. View components are provided for ordering and paging of tabular data sets.  
 
	* The paginator component can be used to display a UI element to navigate through pages of a model containing a ```Paged<T>``` dataset. The paginator has a single required ```pages``` parameter which is provided via ```@Model.Pages```. The component also accepts a second optional ```links``` parameter which can be used to configure the number of page links (default is 15)

	```c#
	<vc:paginator pages=@Model.Pages links="10" />
    ```

    * A sort link component can be used to provide ordering to table columns. In example below the component provides an anchor tag that will sort the data by "id" column.

    ```c#
    <vc:sort-link column="id" />
    ``` 

    An example of usage for both can be found in the ```UserController.Index``` action and view ```User/Index.cshtml```.



    ```dotnet new termonbase -n SolutionName```

4. To uninstall a smartfridge (no longer can be used with dotnet new ).

    ```dotnet new uninstall /path/DotNetSmartFridgeBase```
