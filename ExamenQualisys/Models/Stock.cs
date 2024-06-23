using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenQualisys.Models
{
    public class Stock
    {
        [Key]
        [Required]
        public int Codigo_Stck { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public string lote { get; set; } = string.Empty;

        [Required]
        public string fecha { get; set; } = string.Empty;

        [Required]
        public int Codigo_Alm { get; set; }

        [Required]
        public int Codigo_Art { get; set; }

        //Acceso a llaves foraneas, se decidio comentar debido a que no se usara por ahora
        //[ForeignKey("Codigo_Alm")]
        //public Almacenes? Almacenes { get; set; }
 
        //[ForeignKey("Codigo_Art")]
        //public Articulos? Articulos { get; set; }
    }
}
