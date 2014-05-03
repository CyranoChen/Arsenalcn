
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/03/2014 13:10:33
-- Generated from EDMX file: C:\Projects\Arsenalcn\Arsenalcn.Framework.Service\ModelFirst\Framework.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [acnframework];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserAccount_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccount] DROP CONSTRAINT [FK_UserAccount_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccount_Account]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccount] DROP CONSTRAINT [FK_UserAccount_Account];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUser_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleUser] DROP CONSTRAINT [FK_RoleUser_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUser_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleUser] DROP CONSTRAINT [FK_RoleUser_User];
GO
IF OBJECT_ID(N'[dbo].[FK_User_inherits_Entity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entities_User] DROP CONSTRAINT [FK_User_inherits_Entity];
GO
IF OBJECT_ID(N'[dbo].[FK_Account_inherits_Entity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entities_Account] DROP CONSTRAINT [FK_Account_inherits_Entity];
GO
IF OBJECT_ID(N'[dbo].[FK_Role_inherits_Entity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entities_Role] DROP CONSTRAINT [FK_Role_inherits_Entity];
GO
IF OBJECT_ID(N'[dbo].[FK_Privilege_inherits_Entity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entities_Privilege] DROP CONSTRAINT [FK_Privilege_inherits_Entity];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Entities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entities];
GO
IF OBJECT_ID(N'[dbo].[Entities_User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entities_User];
GO
IF OBJECT_ID(N'[dbo].[Entities_Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entities_Account];
GO
IF OBJECT_ID(N'[dbo].[Entities_Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entities_Role];
GO
IF OBJECT_ID(N'[dbo].[Entities_Privilege]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entities_Privilege];
GO
IF OBJECT_ID(N'[dbo].[UserAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccount];
GO
IF OBJECT_ID(N'[dbo].[RoleUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleUser];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Entities'
CREATE TABLE [dbo].[Entities] (
    [TID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ObjectType] nvarchar(max)  NOT NULL,
    [IsActive] nvarchar(max)  NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [CreateTime] nvarchar(max)  NOT NULL,
    [UpdateTime] nvarchar(max)  NOT NULL,
    [Remark] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DisplayName] nvarchar(max)  NOT NULL,
    [Gender] nvarchar(max)  NOT NULL,
    [Mobile] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Entities_Account'
CREATE TABLE [dbo].[Entities_Account] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [TID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Entities_Role'
CREATE TABLE [dbo].[Entities_Role] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [TID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Entities_Privilege'
CREATE TABLE [dbo].[Entities_Privilege] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [TID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserAccount'
CREATE TABLE [dbo].[UserAccount] (
    [Users_ID] int  NOT NULL,
    [Accounts_TID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'RoleUser'
CREATE TABLE [dbo].[RoleUser] (
    [Roles_TID] uniqueidentifier  NOT NULL,
    [Users_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [TID] in table 'Entities'
ALTER TABLE [dbo].[Entities]
ADD CONSTRAINT [PK_Entities]
    PRIMARY KEY CLUSTERED ([TID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [TID] in table 'Entities_Account'
ALTER TABLE [dbo].[Entities_Account]
ADD CONSTRAINT [PK_Entities_Account]
    PRIMARY KEY CLUSTERED ([TID] ASC);
GO

-- Creating primary key on [TID] in table 'Entities_Role'
ALTER TABLE [dbo].[Entities_Role]
ADD CONSTRAINT [PK_Entities_Role]
    PRIMARY KEY CLUSTERED ([TID] ASC);
GO

-- Creating primary key on [TID] in table 'Entities_Privilege'
ALTER TABLE [dbo].[Entities_Privilege]
ADD CONSTRAINT [PK_Entities_Privilege]
    PRIMARY KEY CLUSTERED ([TID] ASC);
GO

-- Creating primary key on [Users_ID], [Accounts_TID] in table 'UserAccount'
ALTER TABLE [dbo].[UserAccount]
ADD CONSTRAINT [PK_UserAccount]
    PRIMARY KEY CLUSTERED ([Users_ID], [Accounts_TID] ASC);
GO

-- Creating primary key on [Roles_TID], [Users_ID] in table 'RoleUser'
ALTER TABLE [dbo].[RoleUser]
ADD CONSTRAINT [PK_RoleUser]
    PRIMARY KEY CLUSTERED ([Roles_TID], [Users_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Users_ID] in table 'UserAccount'
ALTER TABLE [dbo].[UserAccount]
ADD CONSTRAINT [FK_UserAccount_User]
    FOREIGN KEY ([Users_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Accounts_TID] in table 'UserAccount'
ALTER TABLE [dbo].[UserAccount]
ADD CONSTRAINT [FK_UserAccount_Account]
    FOREIGN KEY ([Accounts_TID])
    REFERENCES [dbo].[Entities_Account]
        ([TID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccount_Account'
CREATE INDEX [IX_FK_UserAccount_Account]
ON [dbo].[UserAccount]
    ([Accounts_TID]);
GO

-- Creating foreign key on [Roles_TID] in table 'RoleUser'
ALTER TABLE [dbo].[RoleUser]
ADD CONSTRAINT [FK_RoleUser_Role]
    FOREIGN KEY ([Roles_TID])
    REFERENCES [dbo].[Entities_Role]
        ([TID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_ID] in table 'RoleUser'
ALTER TABLE [dbo].[RoleUser]
ADD CONSTRAINT [FK_RoleUser_User]
    FOREIGN KEY ([Users_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUser_User'
CREATE INDEX [IX_FK_RoleUser_User]
ON [dbo].[RoleUser]
    ([Users_ID]);
GO

-- Creating foreign key on [TID] in table 'Entities_Account'
ALTER TABLE [dbo].[Entities_Account]
ADD CONSTRAINT [FK_Account_inherits_Entity]
    FOREIGN KEY ([TID])
    REFERENCES [dbo].[Entities]
        ([TID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TID] in table 'Entities_Role'
ALTER TABLE [dbo].[Entities_Role]
ADD CONSTRAINT [FK_Role_inherits_Entity]
    FOREIGN KEY ([TID])
    REFERENCES [dbo].[Entities]
        ([TID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TID] in table 'Entities_Privilege'
ALTER TABLE [dbo].[Entities_Privilege]
ADD CONSTRAINT [FK_Privilege_inherits_Entity]
    FOREIGN KEY ([TID])
    REFERENCES [dbo].[Entities]
        ([TID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------