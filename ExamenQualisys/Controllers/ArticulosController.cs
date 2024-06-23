using ExamenQualisys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamenQualisys.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly ContextModel _contex;
        private readonly ILogger<ArticulosController> _logger;

        public ArticulosController(ContextModel context, ILogger<ArticulosController> loger)
        {
            _contex = context;
            _logger = loger;
        }

        [HttpGet]
        [Route("Articulo/Lista")]
        public async Task<ActionResult> ListaArticulos()
        {
            if (_contex.Articulos != null)
            {
                var list = await _contex.Articulos.ToListAsync();
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
        [Route("Articulo/Consulta")]
        public async Task<ActionResult> ConsultaArticulo(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation("Error 500 -> Hubo un error con el ID");
                return Problem("Hubo un error con el ID");
            }

            var articulo = await _contex.Articulos.FirstOrDefaultAsync(x => x.Codigo_Art == id);
            if (articulo == null)
            {
                _logger.LogInformation("Error 404 -> No se encontro el registro");
                return NotFound("No se encontro el registro");
            }
            return Ok(articulo);
        }

        [HttpPut]
        [Route("Articulo/Modificar")]
        public async Task<ActionResult> ModificarArticulo(Articulos art)
        {
            try 
            {

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada");
                    return Problem("Algo salio mal con la información Proporcionada");
                }
                var consultArt = await _contex.Articulos.Where(i => i.Codigo_Art == art.Codigo_Art).FirstOrDefaultAsync();
                if (consultArt != null)
                {
                    consultArt.Nombre = art.Nombre;
                    consultArt.Descripcion = art.Descripcion;
                    consultArt.Precio = art.Precio;
                    _contex.SaveChanges();
                    return Ok(consultArt);
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

        [HttpPost]
        [Route("Articulo/Crear")]
        public async Task<ActionResult> CrearArticulo(string nombre, string descripcion, double precio)
        {
            try
            {
                if (nombre == null || descripcion == null )
                {
                    _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada");
                    return Problem("Algo salio mal con la información Proporcionada");
                }

                var newArt = new Articulos();
                newArt.Nombre = nombre;
                newArt.Descripcion = descripcion;
                newArt.Precio = precio;
                _contex.Add(newArt);
                await _contex.SaveChangesAsync();
                return Ok(newArt);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada, {ex}", ex);
                return Problem("Algo salio mal con la información Proporcionada");
            }
        }

        [HttpDelete]
        [Route("Articulo/Eliminar")]
        public async Task<ActionResult> EliminarArticulo(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation("Error 500 -> Hubo un error con el ID");
                return Problem("Hubo un error con el ID");
            }
            var art = await _contex.Articulos.FindAsync(id);
            if (art != null)
            {
                _contex.Articulos.Remove(art);
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
