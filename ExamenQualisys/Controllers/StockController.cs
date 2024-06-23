using ExamenQualisys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamenQualisys.Controllers
{
    public class StockController : Controller
    {
        private readonly ContextModel _contex;
        private readonly ILogger<StockController> _logger;

        public StockController(ContextModel context, ILogger<StockController> logger)
        {
            _contex = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Stock/Lista")]
        public async Task<ActionResult> ListaArticulos()
        {
            if (_contex.Stock != null)
            {
                //Acceso a llaves foraneas, se decidio comentar debido a que no se usara por ahora
                //var list = await _contex.Stock.Include(p => p.Articulos).Include(p => p.Almacenes).ToListAsync();
                var list = await _contex.Stock.ToListAsync();
                if (list.Count == 0)
                {
                    _logger.LogInformation("Error 404 -> No se encontraron registros");
                    return NotFound("No se encontraron registros");
                }           
                return Ok(list);
            }
            else
            {
                _logger.LogInformation("Error 500 -> Entity set 'ContextModel.Articulos'  is null");
                return Problem("Entity set 'ContextModel.Articulos'  is null.");
            }
        }

        [HttpGet]
        [Route("Stock/Consulta")]
        public async Task<ActionResult> ConsultaStock(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation("Error 500 -> Hubo un error con el ID");
                return Problem("Hubo un error con el ID");
            }

            //Acceso a llaves foraneas, se decidio comentar debido a que no se usara por ahora
            //var stck = await _contex.Stock
            //    .Include(x => x.Almacenes)
            //    .Include(y => y.Articulos)
            //    .FirstOrDefaultAsync(z => z.Codigo_Stck == id);
            var stck = await _contex.Stock.FirstOrDefaultAsync(z => z.Codigo_Stck == id);
            if (stck == null)
            {
                _logger.LogInformation("Error 404 -> No se encontro el registro");
                return NotFound("No se encontro el registro");
            }
            return Ok(stck);
        }

        [HttpPost]
        [Route("Stock/Crear")]
        public async Task<ActionResult> CrearStock(int Ncantidad, string Nlote, string Nfecha, int NCodigo_Alm, int NCodigo_Art)
        {
            try
            {
                if (Ncantidad < 0  || Nlote == null || Nfecha == null || NCodigo_Alm < 0 || NCodigo_Art < 0)
                {
                    _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada");
                    return Problem("Algo salio mal con la información Proporcionada");
                }

                var stck = new Stock
                {
                    Codigo_Alm = NCodigo_Alm,
                    Codigo_Art = NCodigo_Art,
                    Cantidad = Ncantidad,
                    lote = Nlote,
                    fecha = Nfecha
                };
                _contex.Add(stck);
                await _contex.SaveChangesAsync();
                return Ok(stck);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada, {ex}", ex);
                return Problem("Algo salio mal con la información Proporcionada");
            }
        }

        [HttpPut]
        [Route("Stock/Modificar")]
        public async Task<ActionResult> ModificarStock(Stock Nstock)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada");
                    return Problem("Algo salio mal con la información Proporcionada");
                }
                var consultStck = await _contex.Stock.Where(i => i.Codigo_Stck == Nstock.Codigo_Stck).FirstOrDefaultAsync();
                if (consultStck != null)
                {
                    consultStck.lote = Nstock.lote;
                    consultStck.Cantidad = Nstock.Cantidad;
                    consultStck.fecha = Nstock.fecha;
                    consultStck.Codigo_Alm = Nstock.Codigo_Alm;
                    consultStck.Codigo_Art = Nstock.Codigo_Art;                 
                    _contex.SaveChanges();
                    return Ok(consultStck);
                }
                else
                {
                    _logger.LogInformation("Error 404 -> No se encontro el registro");
                    return NotFound("No se encontro el registro");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada, {ex}", ex);
                return Problem("Algo salio mal con la información Proporcionada");
            }
        }

        [HttpDelete]
        [Route("Stock/Eliminar")]
        public async Task<ActionResult> EliminarStock(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation("Error 500 -> Hubo un error con el ID");
                return Problem("Hubo un error con el ID");
            }
            var stock = await _contex.Stock.FindAsync(id);
            if (stock != null)
            {
                _contex.Stock.Remove(stock);
                await _contex.SaveChangesAsync();
                return Ok("Registro Eliminado Correctamente");
            }
            else
            {
                _logger.LogInformation("Error 404 -> No se encontro el registro");
                return NotFound("No se encontro el registro");
            }
        }
    }
}
