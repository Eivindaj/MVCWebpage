using Microsoft.AspNetCore.Mvc;
using WebappTest.Data;
using WebappTest.Models;

namespace WebappTest.Controllers
{
    public class StillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StillingController(ApplicationDbContext context)
        {
            _context = context;
        }

        static bool checkIfTimeOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if ((start1 <= end2) && (start2 <= end1))
            {
                return true;
            }
            return false;
        }

        public IActionResult Index()
        {
            var stillingModel = 
                from stilling in _context.stilling
                join ansatts in _context.ansatts on stilling.Ansatt_id equals ansatts.Id into Ansatts
                from m in Ansatts.DefaultIfEmpty()
                select new StillingModel
                {
                    Ansatt_id = m.Id,
                    Ansatt_navn = m.Navn,
                    Stilling_id = stilling.Id,
                    Stilling_navn = stilling.Navn,
                    Stilling_start = stilling.Periode_Start,
                    Stilling_slutt = stilling.Periode_Slutt
                };

            return View(stillingModel);
        }
        
        public IActionResult Search(int id)
        {


            
            var stillingModel =
                from stilling in _context.stilling
                join ansatts in _context.ansatts on stilling.Ansatt_id equals ansatts.Id into Ansatts
                from m in Ansatts.DefaultIfEmpty()
                where stilling.Id == id
                select new StillingModel
                {
                    Ansatt_id = m.Id,
                    Ansatt_navn = m.Navn,
                    Stilling_id = stilling.Id,
                    Stilling_navn = stilling.Navn,
                    Stilling_start = stilling.Periode_Start,
                    Stilling_slutt = stilling.Periode_Slutt
                };

            return View(stillingModel);
        }
        
        //GET
        public IActionResult Create()
        {

            var ansatte = _context.ansatts;

            StillingModelCreate stillingCreate = new StillingModelCreate();
            stillingCreate.Ansatt = ansatte;
            stillingCreate.Stilling_start = DateTime.Now;
            stillingCreate.Stilling_slutt = DateTime.Now;

            return View(stillingCreate);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StillingModelCreate obj)
        {
            if(obj.Stilling_navn == null || obj.Stilling_navn == "")
            {
                ModelState.AddModelError("Navn", "Navn kan ikke være tomt");
            }

            IEnumerable<Stilling> overlap =
                from stilling in _context.stilling
                where stilling.Ansatt_id == obj.Ansatt_id
                select stilling;
            foreach(Stilling overlapItem in overlap)
            {
                if(checkIfTimeOverlap(obj.Stilling_start, obj.Stilling_slutt, overlapItem.Periode_Start, overlapItem.Periode_Slutt))
                {
                    ModelState.AddModelError("Stilling_start", "Stillingen overlapper");
                }
            }

            Stilling stil = new Stilling();
            stil.Navn = obj.Stilling_navn;
            stil.Ansatt_id = obj.Ansatt_id;
            stil.Periode_Start = obj.Stilling_start;
            stil.Periode_Slutt = obj.Stilling_slutt;
            
            
            if (ModelState.IsValid)
            {
                _context.stilling.Add(stil);
                _context.SaveChanges();
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }
            var stillingDb = _context.stilling.Find(id);

            if (stillingDb == null)
            {
                return NotFound();
            }


            return View(stillingDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Stilling obj)
        {

            var duplicate =
                from stilling in _context.stilling
                where stilling.Navn == obj.Navn
                where stilling.Periode_Start == obj.Periode_Start
                where stilling.Periode_Slutt == obj.Periode_Slutt
                where stilling.Ansatt_id == obj.Ansatt_id
                select stilling;

            if (duplicate.Any())
            {
                ModelState.AddModelError("Navn", "Stillingen eksisterer allerede");
            }

            if (ModelState.IsValid)
            {
                _context.stilling.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }
            var stillingDb = _context.stilling.Find(id);

            if (stillingDb == null)
            {
                return NotFound();
            }


            return View(stillingDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {

            Stilling isInDb = _context.stilling.Find(id);
            if (isInDb == null)
            {
                return NotFound();
            }

            _context.stilling.Remove(isInDb);
            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }

    public class StillingModel
    {
        public int Ansatt_id { get; set; }
        public string Ansatt_navn { get; set; }
        public int Stilling_id { get; set; }
        public string Stilling_navn { get; set; }
        public DateTime Stilling_start { get; set; }
        public DateTime Stilling_slutt { get; set; }
    }

    public class StillingModelCreate
    {
        public int Stilling_id { get; set; }
        public string Stilling_navn { get; set; }
        public DateTime Stilling_start { get; set; }
        public DateTime Stilling_slutt { get; set; }
        public int Ansatt_id { get; set; }
        public IEnumerable<Ansatt> Ansatt { get; set; }
    }
}
