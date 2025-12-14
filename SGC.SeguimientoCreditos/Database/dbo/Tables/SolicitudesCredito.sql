CREATE TABLE [dbo].[SolicitudesCredito] (
    [Id]                    INT             IDENTITY (11550, 1) NOT NULL,
    [ClienteId]             INT             NOT NULL,
    [IdentificacionCliente] VARCHAR (20)    NOT NULL,
    [Monto]                 DECIMAL (18, 2) NOT NULL,
    [Comentarios]           VARCHAR (MAX)   NOT NULL,
    [Estado]                VARCHAR (50)    NOT NULL,
    [Fecha]                 DATETIME2 (7)   NOT NULL,
    [Documentos]            VARCHAR (MAX)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SolicitudesCredito_Cliente] FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[Clientes] ([Id])
);

