using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursverwaltung.Models;
using System.Globalization;
using CsvHelper;
using System.Text;

namespace Kursverwaltung.Controllers
{
    public class KurseController : Controller
    {
        private readonly KursDbContext _context;

        public KurseController( KursDbContext context )
        {
            _context = context;
        }

        // GET: Kurse
        public IActionResult Index()
        {
            return View(  _context.Kurse.ToList() );
        }

        // GET: Kurse/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var kurs = await _context.Kurse
                .FirstOrDefaultAsync( m => m.KursID == id );
            if ( kurs == null )
            {
                return NotFound();
            }

            return View( kurs );
        }

        // GET: Kurse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kurse/Create To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind( "KursID,Name,Start,Dauer" )] Kurs kurs )
        {
            if ( ModelState.IsValid )
            {
                _context.Add( kurs );
                await _context.SaveChangesAsync();
                return RedirectToAction( nameof( Index ) );
            }
            return View( kurs );
        }

        [HttpPost]
        public async Task<IActionResult> Upload( IFormFile file )
        {
            if ( file.Length > 0 )
            {
                List<string> rows = new();

                //using(var fs = file.OpenReadStream())
                //{
                //    using(var reader = new StreamReader(fs))
                //    {
                //        while ( reader.EndOfStream )
                //        {
                //            rows.Add(reader.ReadLine());
                //        }
                //    }
                //}

                //foreach ( var row in rows )
                //{
                //    var properties = row.Split( ";" );

                //    var kurs = new Kurs()
                //    {
                //    };
                //}

                using ( var fs = file.OpenReadStream() )
                {
                    using ( var reader = new StreamReader( fs ) )
                    {
                        var config = new CsvHelper.Configuration.CsvConfiguration( CultureInfo.InvariantCulture )
                        {
                            Delimiter = ";" ,
                            HasHeaderRecord = true
                        };

                        using ( CsvReader csvReader = new CsvReader( reader , config ) )
                        {
                            csvReader.Context.RegisterClassMap<KursMap>();
                            var kurse = csvReader.GetRecords<Kurs>();

                            foreach ( var kurs in kurse )
                            {
                                if ( !KursExists( kurs.KursID ) )
                                    _context.Kurse.Add( kurs );
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction( "Index" );
        }

        [HttpPost]
        public IActionResult Download()
        {
            if ( _context.Kurse != null )
            {
                string csvstring = "KursID;Name;Start;Dauer\n";

                List<Kurs> rows = _context.Kurse.ToList();

                foreach ( Kurs kurs in rows )
                {
                    csvstring += $"{kurs.KursID};{kurs.Name};{kurs.Start.ToShortDateString()};{kurs.Dauer}\n";
                }

                byte [] data = ASCIIEncoding.ASCII.GetBytes( csvstring );

                return File( data , "" , "Kurse.csv" );
            }
            else
            {
                return RedirectToAction( "Index" );
            }
        }

        // GET: Kurse/Edit/5
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var kurs = await _context.Kurse.FindAsync( id );
            if ( kurs == null )
            {
                return NotFound();
            }
            return View( kurs );
        }

        // POST: Kurse/Edit/5 To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id , [Bind( "KursID,Name,Start,Dauer" )] Kurs kurs )
        {
            if ( id != kurs.KursID )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                try
                {
                    _context.Update( kurs );
                    await _context.SaveChangesAsync();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !KursExists( kurs.KursID ) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction( nameof( Index ) );
            }
            return View( kurs );
        }

        // GET: Kurse/Delete/5
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var kurs = await _context.Kurse
                .FirstOrDefaultAsync( m => m.KursID == id );
            if ( kurs == null )
            {
                return NotFound();
            }

            return View( kurs );
        }

        // POST: Kurse/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            var kurs = await _context.Kurse.FindAsync( id );
            if ( kurs != null )
            {
                _context.Kurse.Remove( kurs );
            }

            await _context.SaveChangesAsync();
            return RedirectToAction( nameof( Index ) );
        }

        private bool KursExists( int id )
        {
            return _context.Kurse.Any( e => e.KursID == id );
        }
    }
}