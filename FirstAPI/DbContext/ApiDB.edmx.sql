
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/25/2018 02:19:27
-- Generated from EDMX file: e:\documents\visual studio 2015\Projects\FirstAPI\FirstAPI\DbContext\ApiDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Database1];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ChampionTags_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChampionTags] DROP CONSTRAINT [FK_ChampionTags_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_ChampionTags_ToTable_1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChampionTags] DROP CONSTRAINT [FK_ChampionTags_ToTable_1];
GO
IF OBJECT_ID(N'[dbo].[FK_MatchupMid_ChampionId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MatchupMid] DROP CONSTRAINT [FK_MatchupMid_ChampionId];
GO
IF OBJECT_ID(N'[dbo].[FK_MatchupMid_PlayerId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MatchupMid] DROP CONSTRAINT [FK_MatchupMid_PlayerId];
GO
IF OBJECT_ID(N'[dbo].[FK_MatchupMid_ResponseChampionId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MatchupMid] DROP CONSTRAINT [FK_MatchupMid_ResponseChampionId];
GO
IF OBJECT_ID(N'[dbo].[FK_MatchupMid_TagId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MatchupMid] DROP CONSTRAINT [FK_MatchupMid_TagId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Champions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Champions];
GO
IF OBJECT_ID(N'[dbo].[ChampionTags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChampionTags];
GO
IF OBJECT_ID(N'[dbo].[MatchupMid]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MatchupMid];
GO
IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Champions'
CREATE TABLE [dbo].[Champions] (
    [ChampionId] uniqueidentifier  NOT NULL,
    [ChampionName] varchar(50)  NOT NULL
);
GO

-- Creating table 'MatchupMid'
CREATE TABLE [dbo].[MatchupMid] (
    [MatchupMidId] uniqueidentifier  NOT NULL,
    [ChampionId] uniqueidentifier  NOT NULL,
    [ResponseChampionId] uniqueidentifier  NOT NULL,
    [TagId] uniqueidentifier  NOT NULL,
    [PlayerId] uniqueidentifier  NOT NULL,
    [Comments] varchar(max)  NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [PlayerId] uniqueidentifier  NOT NULL,
    [Nickname] varchar(50)  NOT NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [TagId] uniqueidentifier  NOT NULL,
    [TagName] varchar(50)  NOT NULL
);
GO

-- Creating table 'ChampionTags'
CREATE TABLE [dbo].[ChampionTags] (
    [PlayerId] uniqueidentifier  NOT NULL,
    [TagId] uniqueidentifier  NOT NULL,
    [ChampionId] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ChampionId] in table 'Champions'
ALTER TABLE [dbo].[Champions]
ADD CONSTRAINT [PK_Champions]
    PRIMARY KEY CLUSTERED ([ChampionId] ASC);
GO

-- Creating primary key on [MatchupMidId] in table 'MatchupMid'
ALTER TABLE [dbo].[MatchupMid]
ADD CONSTRAINT [PK_MatchupMid]
    PRIMARY KEY CLUSTERED ([MatchupMidId] ASC);
GO

-- Creating primary key on [PlayerId] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([PlayerId] ASC);
GO

-- Creating primary key on [TagId] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([TagId] ASC);
GO

-- Creating primary key on [PlayerId], [TagId], [ChampionId] in table 'ChampionTags'
ALTER TABLE [dbo].[ChampionTags]
ADD CONSTRAINT [PK_ChampionTags]
    PRIMARY KEY CLUSTERED ([PlayerId], [TagId], [ChampionId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ChampionId] in table 'MatchupMid'
ALTER TABLE [dbo].[MatchupMid]
ADD CONSTRAINT [FK_MatchupMid_ChampionId]
    FOREIGN KEY ([ChampionId])
    REFERENCES [dbo].[Champions]
        ([ChampionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MatchupMid_ChampionId'
CREATE INDEX [IX_FK_MatchupMid_ChampionId]
ON [dbo].[MatchupMid]
    ([ChampionId]);
GO

-- Creating foreign key on [ResponseChampionId] in table 'MatchupMid'
ALTER TABLE [dbo].[MatchupMid]
ADD CONSTRAINT [FK_MatchupMid_ResponseChampionId]
    FOREIGN KEY ([ResponseChampionId])
    REFERENCES [dbo].[Champions]
        ([ChampionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MatchupMid_ResponseChampionId'
CREATE INDEX [IX_FK_MatchupMid_ResponseChampionId]
ON [dbo].[MatchupMid]
    ([ResponseChampionId]);
GO

-- Creating foreign key on [PlayerId] in table 'MatchupMid'
ALTER TABLE [dbo].[MatchupMid]
ADD CONSTRAINT [FK_MatchupMid_PlayerId]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MatchupMid_PlayerId'
CREATE INDEX [IX_FK_MatchupMid_PlayerId]
ON [dbo].[MatchupMid]
    ([PlayerId]);
GO

-- Creating foreign key on [TagId] in table 'MatchupMid'
ALTER TABLE [dbo].[MatchupMid]
ADD CONSTRAINT [FK_MatchupMid_TagId]
    FOREIGN KEY ([TagId])
    REFERENCES [dbo].[Tags]
        ([TagId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MatchupMid_TagId'
CREATE INDEX [IX_FK_MatchupMid_TagId]
ON [dbo].[MatchupMid]
    ([TagId]);
GO

-- Creating foreign key on [ChampionId] in table 'ChampionTags'
ALTER TABLE [dbo].[ChampionTags]
ADD CONSTRAINT [FK_ChampionTags_ToTable_1]
    FOREIGN KEY ([ChampionId])
    REFERENCES [dbo].[Champions]
        ([ChampionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChampionTags_ToTable_1'
CREATE INDEX [IX_FK_ChampionTags_ToTable_1]
ON [dbo].[ChampionTags]
    ([ChampionId]);
GO

-- Creating foreign key on [TagId] in table 'ChampionTags'
ALTER TABLE [dbo].[ChampionTags]
ADD CONSTRAINT [FK_ChampionTags_ToTable]
    FOREIGN KEY ([TagId])
    REFERENCES [dbo].[Tags]
        ([TagId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChampionTags_ToTable'
CREATE INDEX [IX_FK_ChampionTags_ToTable]
ON [dbo].[ChampionTags]
    ([TagId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------