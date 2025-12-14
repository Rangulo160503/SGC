CREATE TABLE [dbo].[Clientes] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Identificacion] VARCHAR (20)  NOT NULL,
    [Nombre]         VARCHAR (100) NOT NULL,
    [Telefono]       VARCHAR (20)  NULL,
    [Email]          VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Clientes_Identificacion]
    ON [dbo].[Clientes]([Identificacion] ASC);

