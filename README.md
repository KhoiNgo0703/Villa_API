THIS IS THE INSTRUCTION/STEPS OF THE SOLUTION
READ IT AS CODE LINES
/////////////////////////////////////////////////////////////////////////
RESTFUL API .NET 8 CORE: VILLA PROJECT
1.Create Project asp.net core web api
2.Add project to source control (GIT)
3.Add API controller
+Show all the files if controller folder is hidden
+Add controller using API controller/add class called nameController.cs
+For a regular class controller, need to add ControllerBase
+ in the controller method, need to return a list in the store
4.Create model
+create a folder called model in the solution
+create a model using class
5.create an api endpoint in controller
6.define route for controller
+use [Route("api/controllerName")]
+use [ApiController]
+need to define http verb for the action: add [HTTPGet]
7.use DTO
+create a new folder in model called Dto
+create a class called VillaDTO.cs
8.create VillaStore (in the future we do not need to do it)
+add a new folder called Data
+add a class called VillaStore
++make it a static class
9.Get individual item from the data
+create another endpoint in the controller with parameter=id
+use FirstOrDefault to get the first item
+need to set id as the parameter for HTTPget to avoid confusion
10.Status code in Endpoints
+Add ActionResult(DataType) and Ok object
11.Validation
+Add validation by checking id or checking a var = individual data retrived
12.Response Type
+Add [ProducesResponseType(number)]
---HTTPPOST-------------------------------
13.Add new data to the store
+In controller, use HTTPPOST and actionResult(typeofData)
+need to used [FromBody] for parameter inside the actionResult Method
+check validation
+increase the number of ID
+add the data to the list
+return data and add responsetype
14.Create at route
+give an url for the new data created
+use return CreatedAtRoute("Resourcewhereitcreated",id,Data)
+create a name for the action of Resourcewhereitcreated in http verb
15.ModelState Validation
+use modelState.IsValid to validate if dont want to use [ApiController]
+use validation in model 
16.Custom ModelState Validation
+use ModelState.AddModelError("name error","message")
------httpDelete----------------------------
17.HttpDelete in Action
+Put response type before httpverb
+use .remove()
+use return NoContent()
+use IActionResult not ActionResult
-----httpPut-----Update data--------------------
18.HttpPut in Action
+use IActionResult
+use [fromBody] DataSource
+return NoContent
-----httpPatch----Update 1 prop only----------
+install package (.aspnetcore.jsonpatch + newtonsofJson)
+import in program.cs builder.Services.AddControllers().AddNewtonsoftJson();
+use JsonPatchDocument<> in paramater of the action method
+use patch.ApplyTo(data,ModelState)
-------Postman------------------------
19.setting for application format
+in program.cs : builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); 
-------------------DEPENDENCY INJECTION and DBContext------------------------------------------
20. Logger Dependency Injection
+set up logger for a controller
++implmentation in the beginning inside class using dependency injection
++log info in each http action method using _logger.LogInformation("message")/_logger.LogError("message")
21.Serilog to get multiple log for checking
+install package serilog.aspnetcore +serilog.sinks.file
+implement the package in program.cs
22.custom log
------DATABASE-------------------------------------
23.Entity framework
+entityframeworkcore.sqlserver
+entityframeworkcore.tools
24.DBContext
++in model class Villa.cs, bind the id as the key and use [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
+create class called ApplicationDbContext in Data folder
+include DbContext to inherit it from Entity Framework
+create DbSet
25.database connection
+in appsetings.json,     "ConnectionStrings": { "DefaultSQLConnection": "Server=LAPTOP-83RRLTU3\\SQLEXPRESS;Database=Villa_API;user=sa;password=123456;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true" }
26.database dependency injection in program.cs
+builder.Services.AddDbContext<ApplicationDbContext>(option =>
{    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
+implement in ApplicationDbContext by using constructor
27.Migration
+PM console to create datatable:PM> add-migration AddVillaTable
+update migration : PM> update-database
28.Seed table with records in ApplicationDbContext
+use protected override
+seed in PM console >add-migration SeedVillaTable
+if want to add new value and not mess up migration, use PM>add-migration SeedVillaTableWithDataName
29.Replace VillaStore with database
+use dependency injection to get _db 
private readonly ApplicationDbContext _db;
public VillaAPIController(ApplicationDbContext db)
{
    _db = db;
}
+for patch, need as no tracking since it is tracking twice
------how to add a new column to an existing database--------
+Create a new migration in PM>add-migration AddCustomNoteColumn
+in the migration file, modify the code:
    public partial class test1 : Migration
{
    /// <inheritdoc />
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("ALTER TABLE Students ADD CustomNote NVARCHAR(MAX) DEFAULT '123123'; ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("ALTER TABLE Students DROP COLUMN CustomNote;");
    }
}


public partial class Test2 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<string>(
        name: "PhoneNumber",
        table: "Students",
        nullable: false,
        defaultValue: new string("123"));

}

/// <inheritdoc />
protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
      name: "PhoneNumber",
      table: "Students");

}
}
-------DTO AND AUTO MAPPER-----
30.seperate DTO for create and update
+create different DTO for different purposese : Create, Update
+Based on the DTO class, update APIcontroller 
31.Async Methods
+recommend using async await for action, checking and database action
32. Setup AutoMapper and Mapping Config
+Used to avoid long code when inserting props for obj
+install AutoMapper and AutoMapperDependency 
+create a config file into project by creating a class called MappingConfig
+register in program.cs : builder.Services.AddAutoMapper(typeof(filename))
+use dependency injection to integrate automapper in controller
+modify the code in controller to integrate automapping
--------------------Respository-----------------
33.Add Villa Repository Interface
+In real world, we do not deal with API directly
+create a folder called Respository
+create a folder inside called IRespository for the interface
+add an interface called IVillaRepository
++in the interface, add tasks including things like GetAll, Get, Create, Remove, Save, Update
34.Villa Repository Implementation
+Create a class called VillaRepository in Repository folder
+Inherit from IvillaRepository 
+since we work with DBContext, need to import it in Repository using dependency injection
+use async await for Task
+use IQueryable to retrieve the db 
35.Async Repository implementation in Project
+register in program.cs
+builder.Services.AddScoped<Interfacerepositoryname,repositoryname>();
+implement repository in controller by using dependency injection and can remove dbcontext implementation in controller
+replace dbcontext code with repository code in controller
36. Repository clean up
+issue when having many repositories with same name functionalities like Get, GetAll
+create an interface called IRepository in IRepository folder
+copy and paste everything from IVillaRepository except Update action since they will not be the same
+create a generic class called T 
+create a class called Repository in Repository folder
+implement IRepository in Repository.cs and copy paste things from VillaRepository.cs to modify 
+use DBset for _db.Villas to clean up the code
+need to implement Repository inside VillaRepository
+pass DBcontext from VillaRepository to Repository by adding base(db)
37.API Response
+Modify the code to make the API have 1 standard response with different props
+Create another model for API response called APIResponse
+implement the model in the controller with protected
+modify the return type in action to be APIResponse type
+modify return in each action and make action become ActionResult if we want to return APIResponse
+use try catch block to get error in every action method
+modify badrequest with response
---------SECOND API ENDPOINT VILLANUMBER-----
----------------------------------------------------------
==================================
38.Villa Number Models
+create model for villa number
+for VillaNumber prop, we do not want it increase the number automatically since number of villa can be 101, 102
++disable DatabaseGeneratedOption
39.Create VillaNumberDTO
+create VillaNumberDTO,VillaNumberUpdateDTO, VillaNumberCreateDTO
--------VILLANUMBER database----
40.Create DBase for VillaNumber
+in Data/ApplicationDbContext, create another dbSet for VillaNumber and seed data for VillaNumber
+create data table by PM console :PM> add-migration AddVillaNumberTable
+update migration : PM> update-database
+seed in PM console >add-migration SeedVillaNumberTable
-------VILLANUMBER repository---------------------------
41.Create IVillaNumberRepository and VillaNumberRepository
42.Add Repository to program.cs
-------VILLANUMBER automapper-------------------------
43. Create mapping path for models of VillaNumber in mappingconfig.cs
-------VILLANUMBER controller-----------------------------
44.Create VillaNumberAPIController
45.Implementation: apiresponse, repository, automapper
46. Httpverb +ProducesResponseType +ActionMethod
----RELATION BETWEEN VILLA AND VILLANUMBER TABLE----
47.Add foreign key reference
+add a foreign key for villanumber database in villa database
++create a prop called VillaID  and set it as foreign key in villanumber database
++add navigation for the foreign key
+add new migration after: PM> add-migration AddForeignKeyToVillaTable
+pm>update -database
+if there is conflict, need to delete VillaNumber and update database again
48. Add relation between VillaNumberDTO and VillaNumber
+add [Required] VillaID to all VillaNumberDTO
+in VillaNumberController, add validation to villaID in action methods
++implement villa repository in villaNumberControl
++add validation in update and create method
-------------------------CONSUMING API--------------------
======================================
+Use MVC to consume the API
49.Setup MVC Web Project
+Create another project in the solution using ASP.Net Core Web App(M-V-C)
+change port number in both projects /Properties/launchSettings to differentiate projects
+set up to run multiple projects
40.Web Project DTO's and API Models
+In real word, need to have another folders for DTO and Models since we do not want to share the DTO and models from the API Project
+copy  from Api project and paste models in Web Project
+In models, remove data entity model, keep API response model and DTO.
41.Constants for the solution
+In real world, should keep it seperately but in this solution, keep everything in 1 solution
+create a project of class library called Villa_Utility
+inside the project, create a class called SD (static details). this class should be static
+inside class, create a enum with http verb inside
42.API request
+for web project, need to create a model for API request called APIRequest
+create obs for ApiType, Url, Data which needs to access to Utility
+Add reference of Utility Project to get access to
43.AutoMapper for DTOs
+Need to map all the DTOs in WebProject like API project
+install AutoMapper and AutoMapper.DependencyInjection
+Config AutoMapper(MappingConfig.cs + register in Program.cs) and map it between DTO
44.Add API Project URL in Web Project AppSettings
+to avoid adding ConnectionString many times, add ServicesURL of API Project in Appsettings.json:
"ServiceUrls":{"VillaAPI":https://localhost:7002}
45.Base Service for API
+Need to implement a service to make the API request and fetch the response
+avoid hard coding and put things in many places
+in Web project, create a folder called Services
+inside Services folder, create a folder for interface called IServices
+inside IServices folder, create an interface called IBaseService
+create var for responseModel and a method to send API and call API.
+implement the IService model by creating BaseService class in Services folder
+responseModel is for sending API and HttpClient for calling API (dependency Injection for HttpClient)
+in the action method Send, create a client
++ in action method, try to use all the try catch to catch error
++ after sending the request, need to define the http type
++create a response after sending
46.Adding Villa Service
+need to implement the explicit service for villa/villa number
+create another interface in IServices for Villa API called IVillaService
+implement IVillaService, IHttpClientFactory(using dependency injection) and BaseService in VillaService.cs 
+remember to add path to  base class in IHttpClientFactory
+when implement Ihttpclientfactory, it needs url of the api project (VillaURL/VillaNumberURL)
+implement the url using dependency injection through IConfiguration
+modify the action methods in VillaService
+register VillaService and HttpClient in program.cs
47.Calling API in Web Project Controller
+In web project, create a controller called VillaController
+in VillaController, called IvillaService + automapper through dependency injection
+recommend to use async await for action methods
++get response of GetAll 
++check the validation of response to deserialize it
++return list of villaDTO 
+debug to check if it works
48.Display all the villa list 
+create a view called IndexVilla in the controller
+need to import the model 
+front-end design 
----front-end design---
+@section scripts{} to include all the scripts inside
+@{ViewBag.Title=""} to name the title of the website
+put all script sources in layout.cshtml and scripts func in scripts folder
---------------------------
49.create villa UI
+in controller:
++for a view, create get method to get view and post method to do the function
++use async await for action methods
++check modelState
+in view:
++ include asp-action
++all the field is required even though it is not required in the model -> to disable it, add ? in the props in the model
50.Diasble NULLABLE-IMPORTANT
+fix issue when data is not updated after creating
+disable Nullable in project edit 
+alsoe in PM>add-migration ChangeNullableToFalse
51.Update Villa Action Methods
+create get and post for UpdateVilla 
+UpdateVilla action need id as the parameter -> get the asp-route-villaId="@item.Id" in Index view
52.Update Villa UI
53.Delete Villa action methods + UI 
-------VILLANUMBER FOR WEB PROJ------
###############################
54.Add VillaNumberService
+IVillaNumberService + VillaNumberService
+add service in program.cs
+add controller for VillaNumber
+implement Villa when calling Name in VillaNumberIndex
++create var called VillaDTO Villa in VillaNumberDTO model
55.Include Villa when retrieving VillaNumber (Using Include())
+Need to show VillaAPI when GET VillaNumerAPI
+Go back API Project to update it 
++in VillaNumberDTO, add VillaDTO Villa
++We already added a foreign key in VillaNumber
++In IRepository and Repository, set string? includeProperties = null for 2 action methods GetAllAsync and GetAsync
++To make it work in Entity framwork, in class Repository, include 
_db.VillaNumbers.Include(u=>u.Villa).ToList() (this is not neccessary, just for syntax)
++this will retrieve Villa when calling VillaNumber
++split prop in includeProps and put it in a query
++test the api for VillaNumber
++in VillaNumbercontroller, update the action methods with the includeProps
---------Bootswatch theme----
56.Bootswatch Theme
57.Villa Home Page
+import all the Villa in Home Page
+In controller, implement automapper and villaservice thr dependency injection
+create action method to get the view
+create a folder for images in wwwroot folder
+use MarkUpString is used to represent Html markup as a string to display dynamic html
-------Toggle swtich for theme background-------------
58.Add a toggle swtich to swtich black and white background for web
+Get the element by id of the theme 
+when click ->AddEventListener->change the background color and save the color in a cookie
+use color of the cookie to set the attribute of the background 
-------VillaNumber for web---------
+It is recommended using ViewModel for large system
58.Create Villa Number GET
+Create a new folder for view model in model folder
+populate that VM in VillaNumberController GetView for CreateVillaNumber
59.Create VillaNumber POST
+Create CreateVillaNumber view and import CreateVillaNumberVM
60.Special Validation and Base Service Update
+fix issue when having same VillaNumber but no error message shows up
+in VillaNumberController in API project, api content has custom error but api response set the isSuccess=true
+how to fix it:
++in VIllaNumberAPIControllers, set ErrorMessages from CustomError
++do samething in VillaAPICOntroller
++do try catch when return API response in baseService (return var/T or return APIResponse/APIResponse)
+fix the drop down in VillaNumberController in Web Proj
++if Post CreateVillaNumber does not go well, need to populate the GetView again
61. Display API Error Messages
+Change asp-validation-summary to All to display Error
+In CreateVillaNumber PostAction, check response.ErrorMessage.Count to add the error to ModelState
62.Update and Delete Action Method for VillaNumber
+Add ViewModel
+Add Get and Post action method
+add view
+for delete method, careful with hidden asp-for since it needs to match the VillaNo 
63.Add sweet alert for notifications
+https://sweetalert2.github.io/
+In Shared Views, create a cshtml called _Notification
+put script in _Notification
+import it in layout.cshtml and implement in Controller action methods
----------------------------API Security--------------------
/////////
64.Add models for login and registeration
+in API Project:
+Create a model called LocalUser
+Create APIResponse Model (if have not)
++Need to initlize ErrorMessage in APIResponse model
+Add it in the database in Application DbContext
+in PM>Add-migration AddUsersToDb
+update-database
+Create DTO = LoginRequestDTO +LoginResponseDTO+RegisterationRequestDTO
65.Add UserRepository
+In API Project:
+Create Interface for UserRepository
+Create UserRepository
++Inherit Interface
+Add DbContext using dependency injection
66.Implement UserRepository-Register
+Work with the Register Method
++Add new user to database
+Work with the Login method
++check the name and password
++if works return a Token
++when generating a JWT token, we need a secret key
++in appsettings.json, add "Secret" for "ApiSettings"
++implement secret in Repository by using dependency injection
and call IConfiguration
67.Generate Token on Successful Login
+In login method of the repository
//if user was found->generate JWT Token
//encode secretkey
//token descriptor
//generate token
68.UserController (API Endpoint)
+Create UsersController 
+Create the route for api
+implement IUserRepository by using dependency injection
+implement APIResponse 
+implement repository in program.cs
+Create HttpPost methods for Login and register (place name for these method)
+++ Note!! when you use method name for Http, in the url need to include it (/api/UsersAuth/login) later in web proj for APIRequest
69.Login and Register in Action
70.Secure API Endpoints
+In Controller, to control the HTTP verb, add [Authorize(Roles="name of role"]
71.Authentication in Action
+pass the token in Bearer
++Add app.UseAuthentication() before app.UseAuthorization() in program.cs
+in program.cs, configure the setting 
72.Swagger and Bearer in Action
+configure for Swagger authentication in program.cs
--------Consume Secured API  of User(WEB Proj)-------------
73. Add Dtos for Login and registeration
+In Web Proj
+Copy, paste and modify dtos models from API Proj 
74.Add Auth Service (AuthService)
+Create Interface IAuthService in IServices
++In the Interface, create 2 methods LoginAsync and RegisterAsync
+Create AuthService in Services
++implement interface, api proj url, httpclientfactory
++For login method action, use SendAsync
++ Note!! when you use method name for Http, in the url need to include it (/api/UsersAuth/login) for APIRequest
++samething to register method
++implement Service in Program.cs (AddHttpClient)
75.Auth Controller Action Methods (Controller +View)
+Create AuthController
+implement AuthService Interface using dependency injection
+create get and post http for Login action method
++for get, no need of async and Task
+create get and post http for register action method
++for get, no need of async and Task
++for post,  await for the APIResponse result
+Create 2 actions Logout and AccessDenied
76.Login and Register View
+Create views for Login, Register and Access Denied
77.Auth Controller Action Methods
+Need to preserve the token in somewhere in the application
+In program.cs, add DistributedMemorycache + session and add UseSession after pipline
+modify action methods in the Auth controller
++for login resposne, after deserialize response, ned to retrieve token and store it in the session
+++go to SD in utility and add public static string SessionToken="JWTToken"
++in log out, remove and make session token empty
78.NavBar Display
+Add login, logout, register in the varbar display in layout.cshmtl
+to switch log in and log out
++inject ihttp context accessor in layout.cshtml
++add Singleton in program.cs
++use  ihttp context accessor to check the condition of token to show log in or log out
++ if you have issue like this: 'isession' does not contain a definition for 'getstring' and no accessible extension method 'getstring' accepting a first argument of type 'isession' could be found
-->>@using Microsoft.AspNetCore.Http 
79.Authentication in Web Project
+In Web Proj
+Add [Authorize(Roles="")] for action methods in controller
+Error still shows after toggle the action methods because we have not configured the authentication in web proj
+configure authentication in web proj
++in program.cs, add app.UseAuthentication() before authorization
++add authentication in builder.Services
+++remember to add loginPath and AccessDeniedPath in Authentication 
+still can not login---> need to tell httpContext that user is logged In
++In AuthController, need to create new claim identity
++add Claim properties
++create principal for the claim identity
++use await httpContext.SignInAsync using the principal to assign the user
+Note! In Api proj, using [authorize] means web application is able to use authorization based on the token that we are sending from the API, but it is not able to pass tokens to our API.
80.Pass token to API from Web Proj 
+need to pass tokenwhen making a call
+since we put [authorize] for Villa in api,
++in IVillaService in Web proj, add a string of token in action methods
++in API request model, create a string token 
++add token in all action methods in VillaService
++do the same thing to VillaNumberService
++modifty baseService, validate token by pass it to headervalue
++pass Token in Action methods in Controllers.
---------------Versioning in API-------------------------
////////////////////////////////////////////////////////////
81.Nuget Packages for API Versioning
+install microsoft.aspNetcore.mvc.versioning for api proj
+install microsoft.asoNetcore.mvc.versioning.apiexplorer for api proj
82.Add versioning to API services
+in API proj, in program.cs, add 
builder.Services.AddApiVersioning(options => {
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true; (this one added later)
});
83.API version Attribute
+In Api proj,
+Add [ApiVersion("NumberVersion")] under [ApiController] in Controller
84.Multiple version in same controller
+in api proj
+add  [Route("api/v{version:apiVersion}/VillaNumberAPI")]
to the route api in troller
+add AddApiVersionApiExplorer in program.cs
85.API Version Configuration
+add route version and apiversion to other controller
+add  options.ReportApiVersions = true;  in builder.Services.AddApiVersioning to show the api version
+add options.SubstituteApiVersionInUrl = true; in builder.Services.AddVersionedApiExplorer to replace api version route with api version value 
86.Swagger Document for v1
+can add options.SwaggerDoc("v1", new OpenApiInfo
{
	Version = "v1.0",
	Title = "Magic Villa V1",
	Description = "API to manage Villa",		
}); in builder.services.AddSwagger
+ can use if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
	app.UseSwaggerUI(options => {
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "VillaV1");
		options.SwaggerEndpoint("/swagger/v2/swagger.json", "VillaV2");
	});
} to switch api version in swagger
87.Managing multiple versions- cleaner approach
+split controllers into different folders for api version
88.API neutral and depreciating API
+add [ApiVersionNeutral] in Controller 
89.Consume Versioned API
+modify apiresponse validation in baseservices 
if (ApiResponse != null && (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest || apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound))
+modify url link to update v1 
----------CACHING, FILTER AND PAGINATION-------
///////////////////////////////////////////////////
90.caching request
+In api proj
+in program.cs, add builder.services.AddResponseCaching();
+in VillaAPI controller,
+after each http verb, we can add [ResponseCache(Duration =30)]
91.caching profile
+instead of add [ResponseCache(Duration =30)] in each http verb, 
we can add it in builder.services.addcontroller in program.cs and change to [ResponseCache(cacheProfileName="cacheName")]
92.filters in API
+in api proj
+in Villa APIcontroller, when u want to filter som props, u can try to include it as the parameter in action method
GetVillas([FromQuery(Name = "filterName")] int? Props(occupancy))
93. Search Villa Name
94.pagination in API
+to show api results page by page in api proj
+have to modify repository and controller
++in Irepository and repository, in getallasync, include 2 parameters pagesize and pageNumber
95.add pagination to response header
+create a model in models called pagination
+modify in villaapicontroller:
96.StatusCode and isSuccess
+fix the error of isSuccess when typing invalid value in GetVilla
-------------.NET IDENTITY---------------------------
////////////////////////////
97.Add Identity Library and Tables
+need to secure the password in the entity database for localUsers
+in api proj
+in program.cs,configure AddIdentity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
+install AspNetCore.Identity.EntityFrameworkcore for api proj
+create a model called ApplicationUser that inherits from IdentityUser
+need to implement Identity in DBContext
++in ApplicationDbContext, inherit IdentityDbContext instead of DbContext.Ex: IdentityDbContext<ApplicationUser>
+to use ApplicationUser in DbContext
++add a navigation prop : public DbSet<ApplicationUser> ApplicationUsers { get; set; }
+try to make a new migration with add-migration AddIdentityUserTable but error shows up:
The entity type 'IdentityUserLogin<string>' requires a primary key to be defined. If you intended to use a keyless entity type, call 'HasNoKey' in 'OnModelCreating'. For more information on keyless 
++need to use base.OnModelCreating(modelBuilder); to override the default
++add the migration again and update database
98.User Repository-login 
+in API proj
+No need to work with LocalUser model 
+in UserRepository, implement UserManager,RoleManager
+replace and modify LocalUsers with ApplicationUsers
++check username and password
++get roles from identity
+create a new DTO called UserDTO since LoginResponseDTO is related to LocalUser Model
+Map ApplicationUser and UserDTO
+implement Automapper in UserRepository
+add Role to LoginResponseDTO
99.User Repository-Register
+Modify the code to return UserDTO
++create new ApplicationUser user and pass data into it
++create names of roles for choosing
++add a role to it if success (admin role = default)
++return UserDTO back
+Update IUserRepository
100.Login and Registration in Web App
+Identity can not convert string to integer 
++we have to change all ID datatype to string in MVC models
+remove role from LoginResponseDTO in api proj since we have roles inside token
+need to retrieve role from the token
+in api proj/UserRepository modify the claims and loginresponseDTO 
+in web proj/model UserDTo we can remove Roles
+in web proj/AuthController, retrieve values from jwt token and claim role and username from token
---------------------V2 Configuration-------------
101.Configure V2 for all Endpoints
+Create V2 version of VillaAPIController and VillaNumberAPIController to allow upload img
102.Make API Version Dynamic in Web Project
+Make the Web proj dynamic when retrieving the api version 
++in SD.cs in Utility, add public static string CurrentAPIVersion ="v2"
++in Services, update the url with it.
103.Modify the Navbar
+Using bootstrap
104. Load PlaceHold Image
105.Show Roles in Dropdown on Register
+Show options of roles when registering
++in SD.cs library, add public const string Admin="admin",  public const string Customer="customer"
++in GET VIEW of register in AuthController in Web proj, populate a new select list item
++Modify Register.cshtml
106.Assign Role while Registering
+Implement roles in POST VIEW of register in AuthController
+modify UserRepository task Register in api proj
107.Modify Navbar based on Role
----------------FILE UPLOAD---------------------------
+Create a way to upload image properly
108.Modify DTO's for Villa
+Modify VillaUpdateDTO, VillaDTO, VillaCreateDTO for both projects
++Add public IFormFile? Image {  get; set; } to VillaCreateDTO
++In Villa Model, add  public string? ImageLocalPath { get; set; } 
++In VillaUpdateDTO, add public string? ImageLocalPath { get; set; }
public IFormFile? Image {get; set;}
++In VillaDTO, add public string? ImageLocalPath { get; set; }
++add new migration and update database
109.Content type for FormFile
+In api Proj
+Create a folder called wwwroot/ProductImage to store the image
++add app.UseStaticFiles() in pipeline program.cs to access www.root
+Modify the accept file type by adding ContentType in SD.cs
+Add ContentType in APIRequest model in Web proj
+Modify APIRequest in Villa Service and Base Service
110.How to handle FormFile in BaseService
+Modify header, content of HttpRequestMessage to implement FormFile 
111.Create Villa With Image
+In Api proj
+In VillaAPIController/Create Villa method/ in parameter change FromBody to FromForm
++modify Create Villa method
112. Update and Delete Villa
+Modify Update and Delete method in VillaController
+Use FromForm to load the image
113.Create Villa in Web proj
+In Create Villa View, need to add "enctype="multipart/form-data"
----------------CLEAN CODE-----------------------------------------
+Using refresh token
114. Rename JWTToken
+change prop name SessionToken in SD to AccessToken
115.Rename LoginResponseDTO
+Remove User prop from LoginResponseDTO model
+modify the code after removing User prop
+Rename it to TokenDTO in both projects since it only returns token
+rename all the loginresponse to something meaningful like tokenDto
116. Create ITokenProvider
+To avoid calling httpcontext all over the website-> create a service to manage the token
+Create Service ITokenProvider and TokenProvider to do 3 things: set the user when user log in, retrieve a token when calling api and clear/reset token when user log out
117. Implement Token Provider
+register TokenProvider in Program.cs
+when work with token/session, need to inject httpcontext provider
+implement httpcontext provider in TokenProvider
+modify action methods in TokenProvider
118.Rename Token in TokenDTO
+In TokenDTO, rename Token to AccessToken in both projects
119.Consume TokenProvider in AuthController
+Modify AuthController with TokenProvider
++In AuthController, implement ITokenProvider
120.Remove Token from API Calls using session
+Do not need to provide token through session in action methods in Controllers.
+Remove httpAcc.HttpContext.Session.GetString(SD.AccessToken) from all parameters of action methods in controller
+Modify the Services and IServices
+Still have error since response is null
121.Pass Bearer Token on HttpClient
+Want to pass token when making api calls
+Implement ITokenProvider in BaseServices
+check token provider and provider access token in httprequestmessage in sendasync method
+But when we do it we will have issue with base service
+Thus, try to inherit base service using dependency injection in all the services
122. Add Base Service in DI
+Remove inherit of Base Service in all Services
+register BaseService in Program.cs
+implement IBaseService using dependency injection in services and remove base(clientFactory)
+use async and await + call SendAsync from _baseService to modify Services
123.Add Bearer Flag
+For api that do not need token
+To do that, in BaseService, add 1 parameter bool withBearer = true for SendAsync
+Set withBearer parameter for action methods in Services that need/do not need token
124.Separating out access token generator call
+in API proj
+In UserRepository, create a method called GetAccessToken to split the code from login action method
---------------------REFRESH TOKEN-------------------
125. How Refresh Tokens work
+refresh token will last longer than access token
+when login, server will give access +refreshtoken
+when access token expires, client need access token+refreshtoken to get the request done
+when both expires, need to login again
+need to make sure refresh token is only valid for 1 use
126. Create Table to refresh token
+In api proj
+create model called RefreshToken with Id=unique key
+add public DbSet<RefreshToken> RefreshTokens {get;set;} in ApplicationDbContext to create the table
+add migration and update database
127.User Controller Endpoint
+In IUserRepository, need another action method for RefreshAccessToken
+create RefreshToken prop in TokenDTO model
+in UserController in api proj, create another endpoint using post method called GetNewTokenFromRefreshToken
128.Read Access Token
+For the RefreshAccessToken action method in UserRepository, we need to read the token->make sure it is valid-> create a refresh token 
+in Login method in UserRepo, create jwtTokenId 
+pass that jwtTokenId into GetAccessToken to claim that TokenId in token
+we can also claim UserID in token 
+Create another endpoint in UserRepo to read the token to return userID and jwtTokenID
129.Create Refresh Token
+in UserRepo, create a private method to create a refreshtoken 
+implement CreateNewRefreshToken method after getting accesstoken in login method in User Repo
130.Add RefreshToken to tokenProvider
+Create RefreshToken prop in SD.cs
+Modify ClearToken in TokenProvider Service to delete RefreshToken
+check RefreshToken in GetToken action method in TokenProvider Service
+append RefreshToken in SetToken action method in TokenProvider Service
131. Steps to generate refresh token
+Work with RefreshAccessToken action method in User Repo
++Find an existing refresh token
++Compare data from existing refresh and access token provided and if there is any missmatch then consider it as a fraud
++When someone tries to use not valid refresh token, fraud possible
++If just expired then mark as invalid and return empty
++Replace old refresh with a new one with updated expire date 
++revoke existing refresh token
++generate new access token
----------IMPLEMENT REFRESH TOKEN IN WEB PROJ---------
132.Better Design for Base Service
+In Base Service, use a messagefactory instead of a http request message since it is forbidden to use same message object more than 1
133.Modify base service for refresh tokens
+in BaseService, create another method called SendWithRefreshToken to send message request and recieve the response when working with fresh token
+this method will be more details than just using httpClient.SendAsync
+Use SendWithRefreshToken in SendAsync
134.Invoke Refresh Endpoint from Web Proj
+In baseService, create a new method called InvokeRefreshTokenEndpoint to call the refresh end point in api proj 
+In this method,make a request to call refresh endpoint
+if response not valid -> sign out and clear token
+if response valid, need another method to sign in with the new token
135.Get New Refresh Token and sign in User
+Create a new method for user to sign in with new token
+Use this method in InvokeRefreshTokenEndpoint method
+Call InvokeRefreshTokenEndpoint in SendWithRefreshTokenAsync method when need to pass a refresh token
+In program.cs in API proj, in AddAuthentication, in tokenvalidationParameters, need to add ClockSkew=Timespan.zero to evaluate token expire times precisely
136.Modify Base Service to handle Error codes
137.Custom Auth Exception
+Create another Service called AuthException and inherit from Exception
+Throw it in BaseService
+Need to make the extension so when the auth exception triggers, it will redirect to homepage
+In Web proj, create a folder called Extensions, inside create a class called AuthExceptionRedirection
+this class will inherit from IExceptionFilter
+Implement the filter in program.cs of web proj: inside AddcontrollersWithViews
138.Seperate out message builder
+In web proj
+Add new service and iservice called IApiMessageRequestBuilder
+Implement it in program.cs and use addsingleton since we do need a different one
+implement in baseservice
139.Clean Design
+Modify User Repo by adding MarkAllTokenInChainAsInvalid,  MarkTokenAsInvalid
140.Revoke token on log out
+when user logs out, we want to mark the refresh token as invalid
+add      Task RevokeRefreshToken(TokenDTO tokenDTO); in UserRepo
+in UsersController, add action method called revoke using http post
+in AuthService , add logoutasync method and modify AuthController
