using CsvHelper.Configuration;

namespace Kursverwaltung.Models;

public class KursMap : ClassMap<Kurs>
{
    public KursMap()
    {
        Map( m => m.KursID ).Index( 0 ).Ignore();
        Map( m => m.Name ).Index( 1 );
        Map( m => m.Start ).Index( 2 ).TypeConverterOption.Format( "d.M.yyyy" );
        Map( m => m.Dauer ).Index( 3 );
    }
}