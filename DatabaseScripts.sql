USE [SuperLogger]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2017-02-18 6:52:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Log](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Source] [int] NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Type] [char](1) NOT NULL,
	[CorrelationID] [varchar](50) NULL,
	[StackTrace] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LogData]    Script Date: 2017-02-18 6:52:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[Log] [int] NOT NULL,
 CONSTRAINT [PK_LogData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LogSource]    Script Date: 2017-02-18 6:52:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogSource](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LogSource] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[LogData]  WITH CHECK ADD  CONSTRAINT [FK_LogData_Log] FOREIGN KEY([Log])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[LogData] CHECK CONSTRAINT [FK_LogData_Log]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [CK_Log_Type] CHECK  (([Type]='W' OR [Type]='E' OR [Type]='I'))
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [CK_Log_Type]
GO
/****** Object:  StoredProcedure [dbo].[AddLogEntry]    Script Date: 2017-02-18 6:52:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[AddLogEntry] 
  @Source varchar(50) = null,
  @Type char(1),
  @CorrelationID varchar(50) = null,
  @CreatedOn DateTime,
  @Message varchar(max),
  @StackTrace varchar(max) = null,
  @LogEntryID int OUTPUT
  as
	declare @SourceId int
	select @SourceId = ID from LogSource where Name = @Source
	if (@SourceId is null) begin
		insert into LogSource (Name) values (@Source)
		select @SourceId = ID from LogSource where Name = @Source
	end
	insert into log ([Source], [Type], [CorrelationID], [CreatedOn], [Message], [StackTrace]) 
		values (@SourceId, @Type, @CorrelationID, @CreatedOn, @Message, @StackTrace)

	set @LogEntryID = @@IDENTITY

GO
/****** Object:  StoredProcedure [dbo].[AddLogEntryData]    Script Date: 2017-02-18 6:52:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[AddLogEntryData]
  @LogEntryID int,
  @Name varchar(50),
  @Value varchar(max)
  as
	insert into LogData ([Log], Name, Value) values (@LogEntryID, @Name, @Value)

GO
