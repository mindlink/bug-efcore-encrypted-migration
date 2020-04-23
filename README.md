# bug-efcore-encrypted-migration
A repository containing a sample project to demonstrate a potential bug in the EFCore migration APIs

# Pre-requisites
An SQL Server 2016+ installation, locally preferably, since it's setup expecting that.

# Steps to reproduce the bug
1. Run the application
2. Follow the application prompts
3. When it comes to executing the PowerShell script, you could equally use SQL Server Management Studio to enable column encryption on the generated table (Content column)
4. Continue on to migration - at this stage the application will try to use `InsertData` to add new data to the migrated database

We expect this to work, but it throws an exception instead.