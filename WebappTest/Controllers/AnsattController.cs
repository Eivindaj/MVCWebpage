using Microsoft.AspNetCore.Mvc;
using WebappTest.Data;
using WebappTest.Models;

namespace WebappTest.Controllers
{
    public class AnsattController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnsattController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Ansatt> objAnsattList = _context.ansatts;
            return View(objAnsattList);
        }

        public IActionResult Search(int id)
        {
            IEnumerable<Ansatt> objAnsattList = _context.ansatts;
            return View(objAnsattList);
        }

        //GET
        public IActionResult Create()
        {
            
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ansatt obj)
        {
            
            Ansatt isNew = _context.ansatts.SingleOrDefault(x => x.Navn == obj.Navn);

            /* Check if the name is already in database */
            if(isNew != null)
            {
                ModelState.AddModelError("Navn", "Personen eksisterer allerede");
            }
            

            if (ModelState.IsValid)
            {
                _context.ansatts.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("index");
            }
            return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            if(id == null || id < 0)
            {
                return NotFound();
            }
            var ansattDb = _context.ansatts.Find(id);

            if(ansattDb == null)
            {
                return NotFound();
            }


            return View(ansattDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ansatt obj)
        {
            Ansatt isNew = _context.ansatts.SingleOrDefault(x => x.Navn == obj.Navn);

            /* Check if the name is already in database */
            if (isNew != null && isNew.Id != obj.Id)
            {
                ModelState.AddModelError("Navn", "Personen eksisterer allerede");
            }

            if (ModelState.IsValid)
            {
                _context.ansatts.Update(obj);
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
            var ansattDb = _context.ansatts.Find(id);

            if (ansattDb == null)
            {
                return NotFound();
            }


            return View(ansattDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {

            Ansatt isInDb = _context.ansatts.Find(id);
            if(isInDb == null)
            {
                return NotFound();
            }

            _context.ansatts.Remove(isInDb);
            _context.SaveChanges();
            return RedirectToAction("index");

        }



    }
}
