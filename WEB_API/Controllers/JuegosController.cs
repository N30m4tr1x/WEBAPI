using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProyecto.Data;
using WebApiProyecto.Models;
using WebApiProyecto.Services;

namespace WebApiProyecto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JuegosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<JuegosController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IApiService _apiService;

    public JuegosController(
        AppDbContext context,
        ILogger<JuegosController> logger,
        IConfiguration configuration,
        IApiService apiService)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
        _apiService = apiService;
    }

    // GET: api/Juegos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Juego>>> GetJuegos()
    {
        _logger.LogInformation("Se solicitaron todos los juegos.");

        var juegos = await _context.Juegos.ToListAsync();

        return Ok(juegos);
    }

    // GET: api/Juegos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Juego>> GetJuego(int id)
    {
        _logger.LogInformation("Se solicitó el juego con ID {Id}.", id);

        var juego = await _context.Juegos.FindAsync(id);

        if (juego == null)
        {
            _logger.LogWarning("No se encontró el juego con ID {Id}.", id);
            return NotFound();
        }

        return Ok(juego);
    }

    // POST: api/Juegos
    [HttpPost]
    public async Task<ActionResult<Juego>> CrearJuego(Juego juego)
    {
        _logger.LogInformation("Creando el juego {Nombre}.", juego.Nombre);

        _context.Juegos.Add(juego);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetJuego),
            new { id = juego.Id },
            juego);
    }

    // PUT: api/Juegos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarJuego(int id, Juego juego)
    {
        if (id != juego.Id)
        {
            return BadRequest();
        }

        _context.Entry(juego).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Se actualizó el juego con ID {Id}.", id);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JuegoExiste(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/Juegos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarJuego(int id)
    {
        var juego = await _context.Juegos.FindAsync(id);

        if (juego == null)
        {
            return NotFound();
        }

        _context.Juegos.Remove(juego);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Se eliminó el juego con ID {Id}.", id);

        return NoContent();
    }
    
    [HttpGet("info")]
    public ActionResult ObtenerInfo()
    {
    var nombreApi = _configuration["ApiName"] ?? "WebApiProyecto";
    var mensaje = _apiService.ObtenerMensaje();

    return Ok(new
    {
        api = nombreApi,
        mensaje = mensaje
    });
        }
    private bool JuegoExiste(int id)
    {
        return _context.Juegos.Any(e => e.Id == id);
    }
}