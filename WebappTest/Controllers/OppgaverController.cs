using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebappTest.Data;
using WebappTest.Models;

namespace WebappTest.Controllers
{
    public class OppgaverController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OppgaverController(ApplicationDbContext context)
        {
            _context = context;
        }

        static bool checkIfTimeBetween(DateTime between, DateTime start, DateTime end)
        {
            if(DateTime.Compare(between, start) >= 0 && DateTime.Compare(between,end) <= 0)
            {
                return true;
            }
            return false;
        }

        public IActionResult Index()
        {
            var oppgaverModel =
                from oppgaver in _context.oppgavers
                join ansatts in _context.ansatts on oppgaver.Ansatt_id equals ansatts.Id into Ansatts
                from m in Ansatts.DefaultIfEmpty()
                select new OppgaverModel
                {
                    Ansatt_id = m.Id,
                    Ansatt_navn = m.Navn,
                    Oppgave_id = oppgaver.Id,
                    Oppgave_navn = oppgaver.Navn,
                    Oppgave_dato = oppgaver.Dato,
                };


            return View(oppgaverModel);
        }


        public IActionResult Create()
        {

            var stillinger = _context.stilling;

            OppgaverModelCreate stillingCreate = new OppgaverModelCreate();
            stillingCreate.Stilling = stillinger;
            stillingCreate.Dato = DateTime.Now;

            return View(stillingCreate);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OppgaverModelCreate obj)
        {

            Stilling stil = _context.stilling.Find(obj.Selecterd_Stilling);



            if (!checkIfTimeBetween(obj.Dato, stil.Periode_Start, stil.Periode_Slutt))
            {
                ModelState.AddModelError("Dato", "Dato må være innenfor stillings datoen");
            }


            var duplicate =
                from oppgaver in _context.oppgavers
                where oppgaver.Navn == obj.Navn
                where oppgaver.Dato == obj.Dato
                where oppgaver.Ansatt_id == stil.Ansatt_id
                select oppgaver;

            if (duplicate.Any())
            {
                ModelState.AddModelError("Navn", "Oppgaven eksisterer allerede");
            }


            if(obj.Selecterd_Stilling <= 0)
            {
                ModelState.AddModelError("Selecterd_Stilling", "Stilling må være valgt");
            }

            

            if (obj.Navn == null || obj.Navn == "")
            {
                ModelState.AddModelError("Navn", "Navn kan ikke være tomt");
            }
            Oppgaver oppg = new Oppgaver();
            oppg.Navn = obj.Navn;
            oppg.Ansatt_id = stil.Ansatt_id;
            oppg.Dato = obj.Dato;


            if (ModelState.IsValid)
            {
                _context.oppgavers.Add(oppg);
                _context.SaveChanges();
                return RedirectToAction("index");
            }

            // Legger tilbake stillinger
            var stillinger = _context.stilling;

            obj.Stilling = stillinger;

            return View(obj);
        }

    }

    public class OppgaverModel
    {
        public int Ansatt_id { get; set; }
        public string Ansatt_navn { get; set; }
        public int Oppgave_id { get; set; }
        public string Oppgave_navn { get; set; }
        public DateTime Oppgave_dato { get; set; }
    }

    public class OppgaverModelCreate
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public int Ansatt_id { get; set; }
        public DateTime Dato { get; set; }
        public IEnumerable<Stilling> Stilling { get; set; }
        public int Selecterd_Stilling { get; set; }
    }



}
