USE [master]
GO
/****** Object:  Database [MatrixCollege]    Script Date: 10/03/2025 20:58:21 ******/
CREATE DATABASE [MatrixCollege]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MatrixCollege', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MatrixCollege.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MatrixCollege_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MatrixCollege_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MatrixCollege] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MatrixCollege].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MatrixCollege] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MatrixCollege] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MatrixCollege] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MatrixCollege] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MatrixCollege] SET ARITHABORT OFF 
GO
ALTER DATABASE [MatrixCollege] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MatrixCollege] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MatrixCollege] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MatrixCollege] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MatrixCollege] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MatrixCollege] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MatrixCollege] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MatrixCollege] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MatrixCollege] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MatrixCollege] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MatrixCollege] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MatrixCollege] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MatrixCollege] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MatrixCollege] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MatrixCollege] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MatrixCollege] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [MatrixCollege] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MatrixCollege] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MatrixCollege] SET  MULTI_USER 
GO
ALTER DATABASE [MatrixCollege] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MatrixCollege] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MatrixCollege] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MatrixCollege] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MatrixCollege] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MatrixCollege] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MatrixCollege] SET QUERY_STORE = ON
GO
ALTER DATABASE [MatrixCollege] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MatrixCollege]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/03/2025 20:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 10/03/2025 20:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](3000) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollments]    Script Date: 10/03/2025 20:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollments](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CourseId] [uniqueidentifier] NOT NULL,
	[EnrolledAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Enrollments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lessons]    Script Date: 10/03/2025 20:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lessons](
	[Id] [uniqueidentifier] NOT NULL,
	[CourseId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[VideoUrl] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Lessons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Progresses]    Script Date: 10/03/2025 20:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Progresses](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[LessonId] [uniqueidentifier] NOT NULL,
	[WatchedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Progresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10/03/2025 20:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/03/2025 20:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](450) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Password] [nvarchar](800) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250304161723_initial migration', N'9.0.2')
GO
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'21c799be-f2af-4231-7ebb-08dd5fd6d764', N'Advanced Software Engineering', N'Techniques for designing, developing, and maintaining complex software systems using modern frameworks and methodologies.', CAST(N'2025-03-10T15:24:09.7321522' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'a45e05cd-2652-4928-7ebc-08dd5fd6d764', N'Artificial Intelligence & Machine Learning', N'Fundamentals and applications of AI/ML, including neural networks, deep learning, and natural language processing.', CAST(N'2025-03-10T15:24:24.9212123' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'dbc60a6c-f1ae-481a-7ebd-08dd5fd6d764', N'Cybersecurity & Ethical Hacking', N'Principles of securing systems, networks, and applications, along with ethical hacking techniques for vulnerability assessment.', CAST(N'2025-03-10T15:24:43.6739351' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'bc8fed11-134c-4457-7ebe-08dd5fd6d764', N'Cloud Computing & DevOps', N'Cloud architecture, deployment models, CI/CD pipelines, containerization, and infrastructure automation.', CAST(N'2025-03-10T15:25:17.6469016' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'49151bc4-b95a-4d15-7ebf-08dd5fd6d764', N'Data Science & Big Data Analytics', N'Data mining, statistical analysis, and machine learning techniques for processing and extracting insights from large datasets.', CAST(N'2025-03-10T15:25:32.4963522' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'1c7f527b-60ce-4365-7ec0-08dd5fd6d764', N'Internet of Things (IoT) Systems', N'Designing and implementing smart systems, including sensors, communication protocols, and IoT security.', CAST(N'2025-03-10T15:25:50.7595123' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'facf046e-e9d1-42fe-7ec1-08dd5fd6d764', N'Blockchain Technology & Applications', N'Decentralized systems, consensus mechanisms, smart contracts, and real-world blockchain use cases.', CAST(N'2025-03-10T15:26:15.9635851' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'8fa67bac-4b9f-4f8f-7ec2-08dd5fd6d764', N'Computer Networks & Protocols', N'Networking principles, protocols, network architecture, and security techniques for modern communication systems.', CAST(N'2025-03-10T15:26:32.4725913' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'b9e0f45f-07c5-47ea-7ec3-08dd5fd6d764', N'Embedded Systems Design', N'Development of hardware-software integrated systems for applications like robotics, automotive systems, and IoT devices.', CAST(N'2025-03-10T15:26:50.3304047' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'94606a08-1df7-4e4f-7ec4-08dd5fd6d764', N'Human-Computer Interaction (HCI)', N'Designing intuitive user interfaces, usability testing, and enhancing user experience through design principles.', CAST(N'2025-03-10T15:27:08.4956585' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'fabf1151-6196-4058-9ece-27d1023a1460', N'Data Analyst', N'Learn to collet and analyze corproate levels of data', CAST(N'2025-02-02T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'6cbb2178-ae1b-4bfd-b422-3779a727c718', N'DevOps', N'Learn to integrate software from development stages to published applications.', CAST(N'2025-02-02T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'a81b8b41-9383-4216-98d8-4e6a7a838ca1', N'NodeJS & React', N'Learn fullstack development with node and react', CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'05bbdfb5-4c8f-4576-977d-4f0be28fdf17', N'Python', N'Learn to script with python', CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'Angular & .NET', N'Fullstack development', CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Courses] ([Id], [Title], [Description], [CreatedAt]) VALUES (N'ecd0f172-c268-472f-8c69-e03050451033', N'Cybersecurity', N'Learn to protect applications form software vulnerabilities', CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Enrollments] ([Id], [UserId], [CourseId], [EnrolledAt]) VALUES (N'51e382d4-da0f-422a-29b4-08dd5e900d58', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'05bbdfb5-4c8f-4576-977d-4f0be28fdf17', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Enrollments] ([Id], [UserId], [CourseId], [EnrolledAt]) VALUES (N'647f87d8-255f-4324-29b5-08dd5e900d58', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'05bbdfb5-4c8f-4576-977d-4f0be28fdf17', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Enrollments] ([Id], [UserId], [CourseId], [EnrolledAt]) VALUES (N'1fe92f8b-c67a-4bee-3a02-08dd5f32596a', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'a81b8b41-9383-4216-98d8-4e6a7a838ca1', CAST(N'2025-03-09T20:15:42.2450000' AS DateTime2))
INSERT [dbo].[Enrollments] ([Id], [UserId], [CourseId], [EnrolledAt]) VALUES (N'a7af73d0-16fe-4b57-a4d4-08dd5fa0e2ae', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'05bbdfb5-4c8f-4576-977d-4f0be28fdf17', CAST(N'2025-03-10T06:57:55.6180000' AS DateTime2))
INSERT [dbo].[Enrollments] ([Id], [UserId], [CourseId], [EnrolledAt]) VALUES (N'33156a46-8142-4dfb-9a8d-b9fda749c36c', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'a81b8b41-9383-4216-98d8-4e6a7a838ca1', CAST(N'2025-03-08T13:35:05.5800000' AS DateTime2))
INSERT [dbo].[Enrollments] ([Id], [UserId], [CourseId], [EnrolledAt]) VALUES (N'4dd7e1ce-ce56-48be-bdae-d98d6c849399', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'fabf1151-6196-4058-9ece-27d1023a1460', CAST(N'2025-03-08T13:34:48.8166667' AS DateTime2))
GO
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'3a6f5058-e528-468d-cf43-08dd5f552ef3', N'6cbb2178-ae1b-4bfd-b422-3779a727c718', N'Introduction to DevOps & CI/CD Fundamentals', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'b979cfa0-ebb9-43f0-cf44-08dd5f552ef3', N'6cbb2178-ae1b-4bfd-b422-3779a727c718', N'Version Control with Git & GitHub', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'ea4c5b5a-46d7-4947-cf45-08dd5f552ef3', N'6cbb2178-ae1b-4bfd-b422-3779a727c718', N'Infrastructure as Code (IaC) with Terraform', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'0a6cc193-a9f6-49dd-cf46-08dd5f552ef3', N'6cbb2178-ae1b-4bfd-b422-3779a727c718', N'Containerization with Docker', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'73a73b6c-e5c2-4d3c-cf47-08dd5f552ef3', N'6cbb2178-ae1b-4bfd-b422-3779a727c718', N'Orchestration with Kubernetes', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'07513a2d-b300-479c-cf48-08dd5f552ef3', N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'Intro to Angular & .NET API', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'f725764e-29f5-439d-cf49-08dd5f552ef3', N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'Dev Environment Setup', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'9a58f38f-2634-4c4f-cf4a-08dd5f552ef3', N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'Building .NET API', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'cdb5dd52-e319-4548-cf4b-08dd5f552ef3', N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'Angular Project Setup', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'2ad3a6d8-e95c-4106-cf4c-08dd5f552ef3', N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'Connecting Angular & API', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'fd86953a-c828-4cbc-cf4d-08dd5f552ef3', N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'CRUD Operations', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'277066e2-4ca5-42c0-cf4e-08dd5f552ef3', N'3ed42cb5-456f-4a25-a60b-7afcae276363', N'Auth & JWT', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'4b863782-041c-4808-cf4f-08dd5f552ef3', N'ecd0f172-c268-472f-8c69-e03050451033', N'Intro to Cybersecurity', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'ec04c9fc-1b02-4c44-cf50-08dd5f552ef3', N'ecd0f172-c268-472f-8c69-e03050451033', N'Network Security Basics', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'3e015bf2-b55f-4ca1-cf51-08dd5f552ef3', N'ecd0f172-c268-472f-8c69-e03050451033', N'Threats & Vulnerabilities', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'1218d18d-1346-4534-cf52-08dd5f552ef3', N'ecd0f172-c268-472f-8c69-e03050451033', N'Cryptography Essentials', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'6722ae0f-a632-4cf7-cf53-08dd5f552ef3', N'ecd0f172-c268-472f-8c69-e03050451033', N'Web Security Fundamentals', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'a83a5f1b-a840-4598-cf54-08dd5f552ef3', N'ecd0f172-c268-472f-8c69-e03050451033', N'Incident Response & Monitoring', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'dc16c7b4-c2dd-4199-849d-0eb960258d1d', N'fabf1151-6196-4058-9ece-27d1023a1460', N'Statistics', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'b7363776-0dd1-4108-9f40-2a6815c669c8', N'05bbdfb5-4c8f-4576-977d-4f0be28fdf17', N'Python basics', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'3ee797e8-7ee8-415e-afbc-3c584edd3c0e', N'a81b8b41-9383-4216-98d8-4e6a7a838ca1', N'JS Servers Using Node', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'28fcf5bc-ff49-44e3-b3c8-3c850c938520', N'a81b8b41-9383-4216-98d8-4e6a7a838ca1', N'Advanced React', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'e0ccfde6-9385-42e3-a158-7c6a7156714b', N'a81b8b41-9383-4216-98d8-4e6a7a838ca1', N'Component Based Programming', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'73960ef8-738d-40b6-9da9-7f01f98b7a0b', N'fabf1151-6196-4058-9ece-27d1023a1460', N'Introduction to Data', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'69432c0f-a1d5-496e-9d7d-82a59bb469d4', N'fabf1151-6196-4058-9ece-27d1023a1460', N'Managing Big Data', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'1a04f12c-30e2-447b-b52f-88935c4c0810', N'fabf1151-6196-4058-9ece-27d1023a1460', N'Tools for Data Analysis', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'6bab3300-5133-446a-a2a2-abaab3b671d3', N'05bbdfb5-4c8f-4576-977d-4f0be28fdf17', N'Advanced Python', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'446048b8-5389-4276-9af3-b745e5cf5758', N'fabf1151-6196-4058-9ece-27d1023a1460', N'Statistics Advanced', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'b3461c2d-0d48-4495-880a-d54acf7151dc', N'a81b8b41-9383-4216-98d8-4e6a7a838ca1', N'JS Basics', N'https://www.youtube.com/embed/K42anUuFaRI')
INSERT [dbo].[Lessons] ([Id], [CourseId], [Title], [VideoUrl]) VALUES (N'e5cacd92-06a6-44ed-bcce-dc295297d914', N'05bbdfb5-4c8f-4576-977d-4f0be28fdf17', N'Pycharm', N'https://www.youtube.com/embed/K42anUuFaRI')
GO
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'5f86b448-a6b0-4687-4503-08dd5f4700f7', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'3ee797e8-7ee8-415e-afbc-3c584edd3c0e', CAST(N'2025-03-09T22:21:39.8237437' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'7cdca300-e6d8-414c-4504-08dd5f4700f7', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'3ee797e8-7ee8-415e-afbc-3c584edd3c0e', CAST(N'2025-03-09T22:21:43.0884102' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'e38b2aa5-408f-4030-4505-08dd5f4700f7', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'3ee797e8-7ee8-415e-afbc-3c584edd3c0e', CAST(N'2025-03-09T22:21:45.8048620' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'886beb1a-2a3b-4678-4506-08dd5f4700f7', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'28fcf5bc-ff49-44e3-b3c8-3c850c938520', CAST(N'2025-03-09T22:21:49.1568620' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'4b555d7e-1932-46f1-c34e-08dd5f550e42', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'e0ccfde6-9385-42e3-a158-7c6a7156714b', CAST(N'2025-03-09T23:55:07.2479714' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'eae799e3-8040-4ebc-c34f-08dd5f550e42', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'28fcf5bc-ff49-44e3-b3c8-3c850c938520', CAST(N'2025-03-10T00:03:12.5125974' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'65ba44d6-60c6-48ee-c350-08dd5f550e42', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'b3461c2d-0d48-4495-880a-d54acf7151dc', CAST(N'2025-03-10T00:04:04.9595779' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'a644009c-8202-4e51-d39d-08dd5fa0fad5', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'b7363776-0dd1-4108-9f40-2a6815c669c8', CAST(N'2025-03-10T09:05:56.2577591' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'3159418e-ab6c-4922-d39e-08dd5fa0fad5', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'6bab3300-5133-446a-a2a2-abaab3b671d3', CAST(N'2025-03-10T09:06:06.0263694' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'07b0dc57-bd64-46ba-d39f-08dd5fa0fad5', N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'e5cacd92-06a6-44ed-bcce-dc295297d914', CAST(N'2025-03-10T09:20:46.5649902' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'f6641cea-89c4-4bda-d3a0-08dd5fa0fad5', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'dc16c7b4-c2dd-4199-849d-0eb960258d1d', CAST(N'2025-03-10T10:16:48.9529271' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'056e25d4-2c06-4ef0-d3a1-08dd5fa0fad5', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'1a04f12c-30e2-447b-b52f-88935c4c0810', CAST(N'2025-03-10T10:17:32.4927461' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'63278719-cf0c-4fdc-d3a2-08dd5fa0fad5', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'dc16c7b4-c2dd-4199-849d-0eb960258d1d', CAST(N'2025-03-10T10:29:18.0476477' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'686a82ee-922c-4078-8c9e-49afaeeb364c', N'0759b274-5fc0-4119-8bd5-36ef8ff7f291', N'b7363776-0dd1-4108-9f40-2a6815c669c8', CAST(N'2025-03-08T20:31:31.4933333' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'b5bd4766-8137-47f8-8694-62aa31b77d36', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'28fcf5bc-ff49-44e3-b3c8-3c850c938520', CAST(N'2025-03-08T15:34:56.8466667' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'bc8f2ee6-2156-43f2-adc0-67c5d42f91d0', N'1836ebdf-9342-45fb-4c67-08dd5d742c0e', N'28fcf5bc-ff49-44e3-b3c8-3c850c938520', CAST(N'2025-03-08T18:12:32.4766667' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'bad5006b-43cb-4cbd-a3cf-728d7eb7561e', N'1836ebdf-9342-45fb-4c67-08dd5d742c0e', N'b7363776-0dd1-4108-9f40-2a6815c669c8', CAST(N'2025-03-08T18:12:41.7133333' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'577cdd5c-548e-40df-90cc-7e2d64ad1fca', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'b7363776-0dd1-4108-9f40-2a6815c669c8', CAST(N'2025-03-08T15:33:04.2266667' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'497d57f2-0ee4-4b01-8ac8-8c38bdb6036c', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'e0ccfde6-9385-42e3-a158-7c6a7156714b', CAST(N'2025-03-08T15:35:03.6400000' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'7b064291-7238-45bd-b7c3-9cadba96232d', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'69432c0f-a1d5-496e-9d7d-82a59bb469d4', CAST(N'2025-03-08T15:35:41.5733333' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'0f9da6f3-93b9-415e-9b94-a0c59ea6f859', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'73960ef8-738d-40b6-9da9-7f01f98b7a0b', CAST(N'2025-03-08T15:35:28.3000000' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'0797dd02-92c3-425d-8f31-d79141fd10b8', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'e5cacd92-06a6-44ed-bcce-dc295297d914', CAST(N'2025-03-08T15:34:23.0833333' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'35977898-5b15-46fa-b3ab-d841cd545a5a', N'1836ebdf-9342-45fb-4c67-08dd5d742c0e', N'73960ef8-738d-40b6-9da9-7f01f98b7a0b', CAST(N'2025-03-08T18:12:47.4866667' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'fab0dc00-1b81-4cfe-b8f2-e1cdd302b014', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'3ee797e8-7ee8-415e-afbc-3c584edd3c0e', CAST(N'2025-03-08T15:34:48.6166667' AS DateTime2))
INSERT [dbo].[Progresses] ([Id], [UserId], [LessonId], [WatchedAt]) VALUES (N'45904f81-835b-4dfb-8c23-e8d562a8bdb4', N'6ddc0899-88d6-4a80-929e-b910683656a2', N'b3461c2d-0d48-4495-880a-d54acf7151dc', CAST(N'2025-03-08T15:35:11.1900000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'Student')
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (3, N'Professor')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'e0490704-7b41-41a9-4c66-08dd5d742c0e', N'bart simpson', N'bart@gmail.com', 2, N'IJijYtS8bPpNi7Wki9S0Rkk14iFFqm0pGQUhDcK+fynT9izFryVs1gaV5JhweSu+GKAr7IAQLxrbje5EOj/Ffw==')
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'1836ebdf-9342-45fb-4c67-08dd5d742c0e', N'lisa simpson', N'lisa@gmail.com', 2, N'mwVtoV9Kc4JmJE5dAokSPM0mUHSyBPpZiiiOO3jbpNIPZnLnBkB0+2YGWucXyWYWx45s5Mrqhk8df9HSm9HLUA==')
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'c0590bdb-1d26-4f60-f24d-08dd5e218cdd', N'gal neeman', N'gal@gmail.com', 2, N'y7Xq6Mwp4F3DsO5jdKvdu8OmC76jJlnD9GZ16H8vT/mJocPGQTyy3DLL5ro9ljke1PWS8FR0rDE5Wauwcnkd5w==')
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'4de708cd-92f9-4cc6-f24e-08dd5e218cdd', N'test test', N'test@gmail.com', 2, N'jYIkj2ow+ckSPhJ4qghHxLltlYYWmNeKetLMRmsAkHfVd3PnR/yy2Z8vJie5DzqAbmJz+n1ErU/8Xb3iK0JGXw==')
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'9aaacf18-fbe0-44e9-640c-08dd5e8767cb', N'erik nek', N'erik@gmail.com', 2, N'iGR+dJnbA2RsjP794UpubvFkr+aSM4ZhXQBbzW4ALRvENielym9vssy7Z/vJf53BbD+ipCon+n4zaM5WX5JCIw==')
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'0759b274-5fc0-4119-8bd5-36ef8ff7f291', N'admin', N'admin@gmail.com', 1, N'h+je1AX2qCsy4Geax+HrvuJjnPiYyZjPUTDJJT2IAPq8YhrPVTVmXF1MUa3Rcmgpe6nIP3X1uBWOYqoROQZbtg==')
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'6ddc0899-88d6-4a80-929e-b910683656a2', N'student', N'student@gmail.com', 2, N'o1X+Dd2qILnLQ/BHucYBj+MIVgx8dsBBLnLOYZlRktHWc6SMSXXSTjqz0AWZ1wm8dfzBl6w731HFTBpQsN4mLg==')
INSERT [dbo].[Users] ([Id], [Name], [Email], [RoleId], [Password]) VALUES (N'c485947c-2ad4-4818-ab26-cb4e45e33136', N'professor', N'professor@gmail.com', 3, N'2UERjyrwY2rtjjLRG2RAjZZ/fHCRW7jyQudtRLXdGcYYfrJhPm2H0DV9TQMFDUjIfhpSFkwFy46hXyogXyYntA==')
GO
/****** Object:  Index [IX_Enrollments_CourseId]    Script Date: 10/03/2025 20:58:22 ******/
CREATE NONCLUSTERED INDEX [IX_Enrollments_CourseId] ON [dbo].[Enrollments]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Enrollments_UserId]    Script Date: 10/03/2025 20:58:22 ******/
CREATE NONCLUSTERED INDEX [IX_Enrollments_UserId] ON [dbo].[Enrollments]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Lessons_CourseId]    Script Date: 10/03/2025 20:58:22 ******/
CREATE NONCLUSTERED INDEX [IX_Lessons_CourseId] ON [dbo].[Lessons]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Progresses_LessonId]    Script Date: 10/03/2025 20:58:22 ******/
CREATE NONCLUSTERED INDEX [IX_Progresses_LessonId] ON [dbo].[Progresses]
(
	[LessonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Progresses_UserId]    Script Date: 10/03/2025 20:58:22 ******/
CREATE NONCLUSTERED INDEX [IX_Progresses_UserId] ON [dbo].[Progresses]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Email]    Script Date: 10/03/2025 20:58:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_RoleId]    Script Date: 10/03/2025 20:58:22 ******/
CREATE NONCLUSTERED INDEX [IX_Users_RoleId] ON [dbo].[Users]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Courses_CourseId]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Users_UserId]
GO
ALTER TABLE [dbo].[Lessons]  WITH CHECK ADD  CONSTRAINT [FK_Lessons_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lessons] CHECK CONSTRAINT [FK_Lessons_Courses_CourseId]
GO
ALTER TABLE [dbo].[Progresses]  WITH CHECK ADD  CONSTRAINT [FK_Progresses_Lessons_LessonId] FOREIGN KEY([LessonId])
REFERENCES [dbo].[Lessons] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Progresses] CHECK CONSTRAINT [FK_Progresses_Lessons_LessonId]
GO
ALTER TABLE [dbo].[Progresses]  WITH CHECK ADD  CONSTRAINT [FK_Progresses_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Progresses] CHECK CONSTRAINT [FK_Progresses_Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles_RoleId]
GO
USE [master]
GO
ALTER DATABASE [MatrixCollege] SET  READ_WRITE 
GO
