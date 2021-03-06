USE [master]
GO
/****** Object:  Database [CatchMeDB]    Script Date: 16-Mar-16 2:25:24 PM ******/
CREATE DATABASE [CatchMeDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CatchMeDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\CatchMeDB.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CatchMeDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\CatchMeDB_log.ldf' , SIZE = 784KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CatchMeDB] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CatchMeDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CatchMeDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CatchMeDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CatchMeDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CatchMeDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CatchMeDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CatchMeDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [CatchMeDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CatchMeDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CatchMeDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CatchMeDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CatchMeDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CatchMeDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CatchMeDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CatchMeDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CatchMeDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CatchMeDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CatchMeDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CatchMeDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CatchMeDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CatchMeDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CatchMeDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CatchMeDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CatchMeDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CatchMeDB] SET  MULTI_USER 
GO
ALTER DATABASE [CatchMeDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CatchMeDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CatchMeDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CatchMeDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [CatchMeDB]
GO
/****** Object:  Table [dbo].[category]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[category_id] [int] IDENTITY(100,1) NOT NULL,
	[description] [nvarchar](60) NULL,
	[display_order] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[comment]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comment](
	[comment_id] [int] IDENTITY(100,1) NOT NULL,
	[task_id] [int] NOT NULL,
	[username] [nvarchar](30) NOT NULL,
	[title] [nvarchar](60) NULL,
	[description] [nvarchar](max) NULL,
	[created_on] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[comment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[employee]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employee](
	[user_id] [varchar](30) NOT NULL,
	[empl_num] [char](10) NOT NULL,
	[title_code] [char](3) NULL,
	[fam_name] [varchar](50) NULL,
	[given_name] [varchar](100) NULL,
	[common_name] [varchar](50) NULL,
	[gender] [char](1) NULL,
	[position_code] [int] NULL,
	[position_title] [varchar](100) NULL,
	[bu_code] [varchar](12) NULL,
	[bu_name] [varchar](100) NULL,
	[tel_num] [varchar](50) NULL,
	[is_active] [char](1) NULL,
	[team] [varchar](100) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[log]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[log](
	[log_id] [int] IDENTITY(100,1) NOT NULL,
	[username] [nvarchar](30) NOT NULL,
	[task_id] [int] NOT NULL,
	[operation] [nvarchar](60) NULL,
	[type] [nvarchar](60) NULL,
	[logtime] [datetime] NULL,
	[description] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[project]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[project](
	[project_id] [int] IDENTITY(100,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[description] [nvarchar](300) NULL,
	[is_active] [bit] NOT NULL DEFAULT ((1)),
PRIMARY KEY CLUSTERED 
(
	[project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[task]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[task](
	[task_id] [int] IDENTITY(100,1) NOT NULL,
	[project_id] [int] NOT NULL,
	[status] [int] NULL,
	[test_status] [int] NULL,
	[title] [nvarchar](500) NULL,
	[description] [nvarchar](max) NULL,
	[creator] [nvarchar](30) NOT NULL,
	[complexity] [int] NULL,
	[due_date] [date] NULL,
	[type] [int] NULL,
	[severity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[task_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[task_category]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[task_category](
	[task_id] [int] NOT NULL,
	[category_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[task_id] ASC,
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[user]    Script Date: 16-Mar-16 2:25:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[username] [nvarchar](30) NOT NULL,
	[firstname] [nvarchar](60) NULL,
	[lastname] [nvarchar](60) NULL,
	[job_title] [nvarchar](60) NULL,
	[team] [nvarchar](60) NULL,
	[role] [int] NOT NULL,
	[num_logins] [int] NULL,
	[is_active] [bit] NOT NULL,
	[email] [nvarchar](60) NULL,
	[active_project] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[comment] ADD  DEFAULT (getdate()) FOR [created_on]
GO
ALTER TABLE [dbo].[log] ADD  DEFAULT (getdate()) FOR [logtime]
GO
ALTER TABLE [dbo].[user] ADD  DEFAULT ((0)) FOR [role]
GO
ALTER TABLE [dbo].[user] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[comment]  WITH CHECK ADD  CONSTRAINT [FK_comment_task_id] FOREIGN KEY([task_id])
REFERENCES [dbo].[task] ([task_id])
GO
ALTER TABLE [dbo].[comment] CHECK CONSTRAINT [FK_comment_task_id]
GO
ALTER TABLE [dbo].[comment]  WITH CHECK ADD  CONSTRAINT [FK_comment_username] FOREIGN KEY([username])
REFERENCES [dbo].[user] ([username])
GO
ALTER TABLE [dbo].[comment] CHECK CONSTRAINT [FK_comment_username]
GO
ALTER TABLE [dbo].[log]  WITH CHECK ADD  CONSTRAINT [FK_log_task_id] FOREIGN KEY([task_id])
REFERENCES [dbo].[task] ([task_id])
GO
ALTER TABLE [dbo].[log] CHECK CONSTRAINT [FK_log_task_id]
GO
ALTER TABLE [dbo].[log]  WITH CHECK ADD  CONSTRAINT [FK_log_username] FOREIGN KEY([username])
REFERENCES [dbo].[user] ([username])
GO
ALTER TABLE [dbo].[log] CHECK CONSTRAINT [FK_log_username]
GO
ALTER TABLE [dbo].[task]  WITH CHECK ADD  CONSTRAINT [FK_task_project_id] FOREIGN KEY([project_id])
REFERENCES [dbo].[project] ([project_id])
GO
ALTER TABLE [dbo].[task] CHECK CONSTRAINT [FK_task_project_id]
GO
ALTER TABLE [dbo].[task_category]  WITH CHECK ADD  CONSTRAINT [FK_task_category_id] FOREIGN KEY([category_id])
REFERENCES [dbo].[category] ([category_id])
GO
ALTER TABLE [dbo].[task_category] CHECK CONSTRAINT [FK_task_category_id]
GO
ALTER TABLE [dbo].[task_category]  WITH CHECK ADD  CONSTRAINT [FK_task_task_id] FOREIGN KEY([task_id])
REFERENCES [dbo].[task] ([task_id])
GO
ALTER TABLE [dbo].[task_category] CHECK CONSTRAINT [FK_task_task_id]
GO
USE [master]
GO
ALTER DATABASE [CatchMeDB] SET  READ_WRITE 
GO
