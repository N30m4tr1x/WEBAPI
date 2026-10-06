namespace WebApiProyecto.Models;

public class Juego
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    public int Anio { get; set; }
}