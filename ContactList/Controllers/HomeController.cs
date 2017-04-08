using ContactList.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContactList.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {
            var mgr = new ContactManager();
            var contactList = await mgr.GetAllAsync();
            return View(contactList);
        }

        // GET: Home/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var mgr = new ContactManager();
            var contact = await mgr.GetByIdAsync(id);
            return View(contact);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Contact newContact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mgr = new ContactManager();
                    await mgr.CreateAsync(newContact);
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new ArgumentException("Name is required.");
                }
            }
            catch
            {
                return View(newContact);
            }
        }

        // GET: Home/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var mgr = new ContactManager();
            var contact = await mgr.GetByIdAsync(id);
            return View(contact);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Contact editContact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mgr = new ContactManager();
                    await mgr.UpdateAsync(editContact);
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new ArgumentException("Name is required.");
                }
            }
            catch
            {
                return View(editContact);
            }
        }

        // GET: Home/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var mgr = new ContactManager();
            var contact = await mgr.GetByIdAsync(id);
            return View(contact);
        }

        // POST: Home/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Contact deleteContact)
        {
            try
            {
                var mgr = new ContactManager();
                await mgr.DeleteAsync(deleteContact.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
