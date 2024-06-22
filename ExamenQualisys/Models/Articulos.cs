using System.ComponentModel.DataAnnotations;

namespace ExamenQualisys.Models
{
    public class Articulos
    {
        [Key]
        public int Codigo_Art { get; set; }

        [Required]
        public string Nombre { get; set;} = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public double Precio { get; set; }

    }
}
