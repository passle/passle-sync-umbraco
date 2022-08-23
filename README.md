# Passle for Umbraco
Passle integration with Umbraco CMS

## Prerequisites
To run the Passle Umbraco extension on IIS, you will need to download and install the following prerequisites:

* ~[SQL Server 2019](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (download the developer edition)~
* ~[SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)~
* ~[SQL Server Compact 4.0 Runtime](https://www.microsoft.com/en-us/download/confirmation.aspx?id=30709)~

## Clone the repository
Create a folder called *PassleCMSIntegrations* under *C:\\*. The clone the repository as *PassleUmbraco* inside that directory.

## Configure permissions
We need to configure folder permissions to ensure IIS can run Umbraco. Open the folder permissions for *PassleUmbraco* and add the following accounts with *Full control* permissions:

* XX-PC\IIS_IUSRS (where XX-PC is the name of your PC)
* IUSR

You may get an error stating that the permissions could not propagate. If you do, you will have to recursively take ownership of all files and folders in the *PassleUmbraco* direcory with the following command:

```
icacls C:\PassleCMSIntegrations\PassleUmbraco /setowner "Your Username" /T /C
```

(where 'Your Username' is your username)

## Build the project
Run Visual Studio as administrator, and open the project solution. ~Once all the projects have loaded, you'll probably want to right click on the solution and select 'Properties', then change the startup project to 'Single startup project' and select 'Passle.Site'.~

Hit OK to accept changes, then build the solution.


## Deprecated

## IIS Setup
Next, set up IIS to run Umbraco:

### Create the application pool
Create a new application pool with the following details:

* **Name:** www.umbracodemo.localhost
* **.NET CLR Version:** .NET CLR Version v4.0.30319
* **Managed pipeline mode:** Integrated
* **Start application pool immediately:** True

### Create the site
Create a new site with the following details:

* **Site name:** www.umbracodemo.localhost
* **Application pool:** www.umbracodemo.localhost
* **Physical path:** C:\\PassleCMSIntegrations\\PassleUmbraco\\Web\\PassleDotCom.PasslePlugin.Web

Add the following bindings:

* **Type:** http; **Host name:** www.umbracodemo.localhost; **Port:** 80
* **Type:** https; **Host name:** www.umbracodemo.localhost; **Port:** 443

## Hosts file
Run notepad as an administrator, then open your hosts file (C:\\Windows\\System32\\drivers\\etc\\hosts) and add the following entry:

```
127.0.0.1 www.umbracodemo.localhost
```

## The moment of truth
You should now be able to access your Umbraco installation at www.umbracodemo.localhost. If anything was missing from the README, please add it yourself!



## SQL Server Setup
Next, set up SQL server to work with IIS:

### Connect via SSMS
To connect to the SQL Server Installation via SSMS, first run the app as an administrator. Once the app is running,select add connection, and enter the following details:

* **Server type:** Database Engine
* **Server name:** The name of your PC (e.g. XX-PC)
* **Authentication:** Windows Authentication

### Add the database
Right click on *Databases* in the object explorer, and select *New database*. Enter *umbraco* for the database name, and hit OK.

### Add the login
Expand the *Security* folder in the object explorer, then right click on *Logins* and select *New login*. Enter *umbraco* for the login name, then select *SQL Server Authentication*, and enter the password, which can be found in the DB connection string inside `Passle.Core\appsettings.json`.

Uncheck *Enforce password policy*, and set the default database for the login to the *umbraco* database created in the previous step.

In the *User mapping* section, tick the box next to *umbraco*, then highlight the *umbraco* entry and ticket the box next to *db_owner* in the panel below.

Hit OK to to create the login.

### Configure the server
Right click on the server in the object explorer, and select *Properties*. In the *Security* section, under *Server authentication*, select *SQL Server and Windows Authentication mode*, and hit OK.

Right click on the server again, and select *Restart*.