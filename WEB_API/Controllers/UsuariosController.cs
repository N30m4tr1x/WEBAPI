using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProyecto.Data;
using WebApiProyecto.Models;

namespace WebApiProyecto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(
        AppDbContext context,
        ILogger<UsuariosController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        _logger.LogInformation("Se solicitaron todos los usuarios.");

        var usuarios = await _context.Usuarios.ToListAsync();

        return Ok(usuarios);
    }

    // GET: api/Usuarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        _logger.LogInformation("Se solicitó el usuario con ID {Id}.", id);

        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            _logger.LogWarning("No se encontró el usuario con ID {Id}.", id);
            return NotFound();
        }

        return Ok(usuario);
    }

    // POST: api/Usuarios
    [HttpPost]
    public async Task<ActionResult<Usuario>> CrearUsuario(Usuario usuario)
    {
        _logger.LogInformation("Creando el usuario {Nombre}.", usuario.Nombre);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUsuario),
            new { id = usuario.Id },
            usuario);
    }

    // PUT: api/Usuarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarUsuario(
        int id,
        Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest();
        }

        _context.Entry(usuario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Se actualizó el usuario con ID {Id}.",
                id);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExiste(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Se eliminó el usuario con ID {Id}.",
            id);

        return NoContent();
    }

    private bool UsuarioExiste(int id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}