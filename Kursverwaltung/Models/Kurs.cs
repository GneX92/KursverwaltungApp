using System.ComponentModel;

namespace Kursverwaltung.Models;

public class Kurs
{
    public int KursID { get; set; }

    public string? Name { get; set; }

    public DateTime Start { get; set; }

    public int Dauer { get; set; }
}