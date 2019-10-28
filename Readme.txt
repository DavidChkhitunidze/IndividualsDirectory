IndividualsDirectory project includes: ASP.NET Core 2.2 MVC Web Application with API, some references for Entities, Db configurations, Dtos and related logics.

Technologies:
SDK - Microsoft.NETCore, ASP.NET Core 2.2, MVC, EnfityFramework.Core, FluentValidation, Json, Razor Views, Javascript, jQuery, HTML, CSS.

1) Clone github project to your local machine.
2) Run it with Visual Studio .
3) Create local db with name "IndividualsDirectory" by SQL Server Object Explorer "(localdb)\MSSQLLocalDB".
		ConnectionString: "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IndividualsDirectory;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

4) Run command: update-database in console with EntityFramework to apply all migrations.
5) Build "IndividualsDirectory" project.
6) Enjoy.

Respectfully

David Chkhitunidze
