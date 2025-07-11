use EY;
GO

-- Crear tabla Proveedores
CREATE TABLE Proveedores (
	Id INT PRIMARY KEY IDENTITY(1,1),
	RazonSocial NVARCHAR(100) NOT NULL,
	NombreComercial NVARCHAR(100) NOT NULL,
	IdentificacionTributaria VARCHAR (11) NOT NULL,
	Telefono VARCHAR(15) NOT NULL,
	Email NVARCHAR(100) NOT NULL,
	SitioWeb NVARCHAR(255) NOT NULL,
	Direccion NVARCHAR(200) NOT NULL,
	Pais NVARCHAR(50) NOT NULL,
	FacturacionAnual DECIMAL(18,2) NOT NULL DEFAULT 0,
	FechaEdicion DATETIME NOT NULL DEFAULT GETUTCDATE()
);

GO