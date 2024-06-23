using System.ComponentModel.DataAnnotations;

namespace ExamenQualisys.Models
{
    public class Almacenes
    {
        [Key]
        [Required]
        public int Codigo_Alm { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;
    }
}
