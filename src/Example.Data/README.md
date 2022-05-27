## Preparation

### Create tool manifest file

```bash 
$ dotnet new tool-manifest
```

> Create .config directory in project root

### Install dotnet-ef tool

```bash
$ dotnet tool install dotnet-ef --local
```

# Migration

## Add

```bash
$ dotnet ef migrations add "<name>" --context TestDbContext --startup-project ../Example/Example.csproj --project ../Example.Data.Sqlite/Example.Data.Sqlite.csproj 
```