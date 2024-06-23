using ExamenQualisys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamenQualisys.Controllers
{
    public class AlmacenesController : Controller
    {
        private readonly ContextModel _contex;
        private readonly ILogger<AlmacenesController> _logger;

        public AlmacenesController(ContextModel context, ILogger<AlmacenesController> logger)
        {
            _contex = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Almacen/Lista")]
        public async Task<ActionResult> ListaProductos()
        {
            if (_contex.Almacenes != null)
            {
                var list = await _contex.Almacenes.ToListAsync();
                if (list.Count == 0)
                {
                    _logger.LogInformation("Error 404 -> No se encontraron registros");
                    return NotFound("No se encontraron registros");
                }
                return Ok(list);
            }
            else
            {   
                _logger.LogInformation("Error 500 -> Entity set 'ContextModel.Sucursales'  is null");
                return Problem("Entity set 'ContextModel.Sucursales'  is null.");
            }
        }

        [HttpGet]
        [Route("Almacen/Consulta")]
        public async Task<ActionResult> ConsultaAlmacen(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation("Error 500 -> Hubo un error con el ID");
                return Problem("Hubo un error con el ID");
            }

            var almacen = await _contex.Almacenes.FirstOrDefaultAsync(x => x.Codigo_Alm == id);
            if (almacen == null)
            {
                _logger.LogInformation("Error 404 -> No se encontro el registro");
                return NotFound("No se encontro el registro");
            }
            return Ok(almacen);
        }

        [HttpPut]
        [Route("Almacen/Modificar")]
        public async Task<ActionResult> ModificarAlmacen(Almacenes almacen)
        {
            try
            {
                var ConsultaAlm = await _contex.Almacenes.Where(y => y.Codigo_Alm == almacen.Codigo_Alm).FirstOrDefaultAsync();
                if (ConsultaAlm != null)
                {
                    ConsultaAlm.Nombre = almacen.Nombre;
                    ConsultaAlm.Descripcion = almacen.Descripcion;
                    _contex.SaveChanges();
                    return Ok(ConsultaAlm);
                }
                else
                {
                    _logger.LogInformation("Error 404 -> No se encontro el registro");
                    return NotFound("No se encontro el registro");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada, {ex}",ex);
                return Problem("Algo salio mal con la información Proporcionada");
            }
        }

        [HttpPost]
        [Route("Almacen/Crear")]
        public async Task<ActionResult> CrearAlmacen(string nombre, string descripcion)
        {
            try
            {
                if (nombre == null || descripcion == null)
                {
                    _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada");
                    return Problem("Algo salio mal con la información Proporcionada");
                }

                var newAlm = new Almacenes();
                newAlm.Nombre = nombre;
                newAlm.Descripcion = descripcion;               
                _contex.Add(newAlm);
                await _contex.SaveChangesAsync();
                return Ok(newAlm);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error 500 -> Algo salio mal con la información Proporcionada, {ex}", ex);
                return Problem("Algo salio mal con la información Proporcionada");
            }         
        }

        [HttpDelete]
        [Route("Almacen/Eliminar")]
        public async Task<ActionResult> EliminarAlmacen(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation("Error 500 -> Hubo un error con el ID");
                return Problem("Hubo un error con el ID");
            }

            var almacen = await _contex.Almacenes.FindAsync(id);
            if (almacen != null)
            {
                _contex.Almacenes.Remove(almacen);
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
