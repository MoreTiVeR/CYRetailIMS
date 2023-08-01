How to generate EF (In VisualStudio)

1.) Tools > Nuget Package Manager > Package Manager Console
2.) Run command : Install-Package Microsoft.EntityFrameworkCore.Tools 
3.) Run command : 
Pattern : Scaffold-DbContext "Server=.\;Data Source=DB_ADDRESS;Initial Catalog=DB_NAME;Persist Security Info=True;User ID=DB_USER;Password=DB_PASSWORD;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o EF_FILE_PATH -Context DBCONTEXT_NAME -UseDatabaseNames -DataAnnotations

e.g.
Scaffold-DbContext "Server=.\;Data Source=localhost;Initial Catalog=CYDB;Persist Security Info=True;User ID=ccc;Password=ccc;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Domain\Entities -Context CYDBContext -UseDatabaseNames -DataAnnotations
