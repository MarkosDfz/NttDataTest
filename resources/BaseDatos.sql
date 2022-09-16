CREATE DATABASE NttDataTest;
GO
USE [NttDataTest]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 12/09/2022 0:19:41 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 12/09/2022 0:19:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Contrasenia] [nvarchar](max) NOT NULL,
	[Estado] [bit] NOT NULL,
	[ClienteGuid] [nvarchar](max) NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Genero] [int] NOT NULL,
	[Edad] [int] NOT NULL,
	[Identificacion] [nvarchar](15) NOT NULL,
	[Direccion] [nvarchar](250) NOT NULL,
	[Telefono] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cuentas]    Script Date: 12/09/2022 0:19:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuentas](
	[CuentaGuid] [uniqueidentifier] NOT NULL,
	[NumeroCuenta] [nvarchar](12) NOT NULL,
	[TipoCuenta] [int] NOT NULL,
	[SaldoInicial] [decimal](18, 2) NULL,
	[Estado] [bit] NOT NULL,
	[ClienteGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Cuentas] PRIMARY KEY CLUSTERED 
(
	[CuentaGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movimientos]    Script Date: 12/09/2022 0:19:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movimientos](
	[MovimientoGuid] [uniqueidentifier] NOT NULL,
	[CuentaGuid] [uniqueidentifier] NOT NULL,
	[TipoMovimiento] [int] NOT NULL,
	[ValorMovimiento] [decimal](18, 2) NOT NULL,
	[SaldoDisponible] [decimal](18, 2) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Movimientos] PRIMARY KEY CLUSTERED 
(
	[MovimientoGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT (newid()) FOR [CuentaGuid]
GO
ALTER TABLE [dbo].[Movimientos] ADD  DEFAULT (newid()) FOR [MovimientoGuid]
GO
