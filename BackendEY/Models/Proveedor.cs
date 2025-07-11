using System.ComponentModel.DataAnnotations;

namespace BackendEY.Models
{
    public class Proveedor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres para la razón social.")]
        public required string RazonSocial { get; set; }

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres para el nombre comercial.")]
        public required string NombreComercial { get; set; }

        [Required(ErrorMessage = "La identificación tributaria es obligatoria.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "La identificación tributaria debe tener exactamente 11 dígitos.")]
        public required string IdentificacionTributaria { get; set; }

        [Phone(ErrorMessage = "Formato de teléfono inválido.")]
        public required string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        public required string Email { get; set; }

        [Url(ErrorMessage = "Formato de URL inválido.")]
        public required string SitioWeb { get; set; }

        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres para la dirección.")]
        public required string Direccion { get; set; }

        [Required(ErrorMessage = "El país es obligatorio.")]
        public required string Pais { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La facturación anual debe ser un número positivo.")]
        public required decimal FacturacionAnual { get; set; }

        public DateTime FechaEdicion { get; set; }
    }
}
