CREATE TABLE [dbo].[TrackingsGestion] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [GestionId]     INT           NOT NULL,
    [Accion]        VARCHAR (100) NOT NULL,
    [Comentario]    VARCHAR (MAX) NOT NULL,
    [UsuarioNombre] VARCHAR (100) NOT NULL,
    [Fecha]         DATETIME2 (7) NOT NULL,
    [SolicitudId]   INT           NOT NULL,
    [UsuarioId]     INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TrackingsGestion_Solicitud] FOREIGN KEY ([GestionId]) REFERENCES [dbo].[SolicitudesCredito] ([Id])
);

