using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenQualisys.Models
{
    public class Stock
    {
        [Key]
        public int Codigo_Stck { get; set; }

        [Required]
        public int Cantidad {  get; set; }

        [Required]
        public string lote { get; set; } = string.Empty;

        [Required]
        public string fecha { get; set; } = string.Empty;

        [ForeignKey("Codigo_Alm")]
        [Required]
        public Almacenes? Almacenes { get; set; }

        [ForeignKey("Codigo_Art")]
        [Required]
        public Articulos? Articulos { get; set; }
    }
}
