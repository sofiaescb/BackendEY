using BackendEY.Models;
using BackendEY.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BackendEY.Controllers
{
    [Route("api/proveedores")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly ProveedorService _service;

        public ProveedorController(ProveedorService service)
        {
            _service = service;
        }

        // GET: api/proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Get()
        {
            var lista = await _service.ListarProveedores();
            return Ok(lista);
        }

        // GET api/proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetById(int id)
        {
            var proveedor = await _service.ObtenerProveedor(id);
            if (proveedor == null)
                return NotFound(new { mensaje = "Proveedor no encontrado" });

            return Ok(proveedor);
        }

        // POST api/proveedores
        [HttpPost]
        public async Task<ActionResult<Proveedor>> Create(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevo = await _service.CrearProveedor(proveedor);
            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, nuevo);
        }

        // PUT api/proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != proveedor.Id)
                return BadRequest(new { mensaje = "El ID en la URL no coincide con el del cuerpo." });

            var ok = await _service.ActualizarProveedor(id, proveedor);
            return ok ? NoContent() : NotFound(new { mensaje = "Proveedor no encontrado" });
        }

        // DELETE api/proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.EliminarProveedor(id);
            return ok ? NoContent() : NotFound(new { mensaje = "Proveedor no encontrado" });
        }
    }
}
