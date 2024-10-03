# ApiTestFramework

This project is an API testing framework using NUnit, RestSharp.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 )
- [Visual Studio Code](https://code.visualstudio.com/) or any other IDE of your choice

## Dependencies

The project uses the following NuGet packages:

- `ExtentReports` (version 5.0.4)
- `ExtentReports.Core` (version 1.0.3)
- `FluentAssertions` (version 6.12.1)
- `Microsoft.AspNetCore.Razor.Language` (version 6.0.33)
- `Microsoft.NET.Test.Sdk` (version 17.11.1)
- `Newtonsoft.Json` (version 13.0.3)
- `NUnit` (version 4.2.2)
- `NUnit3TestAdapter` (version 4.6.0)
- `ReportUnit` (version 1.2.1)
- `RestSharp` (version 112.0.0)

## Setup

1. **Clone the repository:**

   ```sh
   git clone https://github.com/hashan7/ApiTestFramework.git
   
   cd ApiTestFramework
   ```

2. **Restore NuGet packages:**
   ```sh
   dotnet restore
   ```

## Running Tests

1. To run the tests, use the following command:
   ```
   dotnet test
   ```

## Cleaning and Reinstalling Packages

If you need to clear and reinstall NuGet packages, follow these steps:

1. Delete the bin and obj folders:

```
rm -rf bin obj
```

2. Clear the NuGet cache:

```
dotnet nuget locals all --clear
```

3. Restore packages:

```
dotnet restore
```
