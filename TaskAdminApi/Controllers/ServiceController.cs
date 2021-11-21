using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAdminApi.Models;

namespace TaskAdminApi.Controllers
{
    public class ServiceController : Controller
    {
       

        private TaskAdminContext db;
        public ServiceController(TaskAdminContext context)
        {
            db = context;
        }

        public IActionResult ViewServices()
        {
            var services = db.Services.ToList();
            return View(services);
        }

        [Route("Service/CreateService")]
        public async Task<IActionResult> CreateService()
        {
            // var positionsValue = await db.Positions.ToListAsync();
            /* var ServicesValue = await db.Services.ToListAsync();
             if (ServicesValue != null)
             {
                 //List<Position> pos = await db.Positions.ToListAsync();
                 List<Service> serv = ServicesValue;
                 SelectList services = new SelectList(serv, "Id", "Service_Name");
                 ViewBag.ServiceList = services;
                 return View();
             }
             else
             {
                 return NotFound();
             }*/

            //здесь я должна отментить состояние checkbox

            return View();
        }

        [Route("Service/CreateService")]
        [HttpPost]
        public async Task<IActionResult> CreateService(Service serv, bool selectedTime)
        {
            if (selectedTime ==true)
            {
                serv.Service_Time_Type_Minutes = true;
            }
            else
            {
                serv.Service_Time_Type_Hours = true;
            }
            
            if (ModelState.IsValid)
            {              
                db.Services.Add(serv);
                db.SaveChanges();
                return RedirectToAction("ViewServices");
            }

            return View();
        }

        [Route("Service/ServiceDetails/{id?}")]
        public async Task<IActionResult> ServiceDetails(int? id)
        {
            if (id != null)
            {
                Service serv = await db.Services.Include(e => e.Clients).FirstOrDefaultAsync(e => e.Id == id);
                if (serv != null)
                {
                    return View(serv);
                }
            }
            return NotFound();
        }


        [HttpGet]
        [Route("Service/ServiceEdit")]
        public async Task<IActionResult> ServiceEdit(int? id)
        {
            //Думаю, что добавление услуги лучше пока сделать только из страницы клиента, чтобы не было путаницы
            if (id != null)
            {
                Service serv = await db.Services.Include(e => e.Clients).FirstOrDefaultAsync(e => e.Id == id);
             
                return View(serv);
            }
            return NotFound();
        }


        [Route("Service/ServiceEdit")]
        [HttpPost]
        public async Task<IActionResult> ServiceEdit(Service serv)
        {
            Service service = await db.Services.FirstOrDefaultAsync(e => e.Id == serv.Id);

            service.Service_Name = serv.Service_Name;
            service.Service_Description = serv.Service_Description;                  

            if (ModelState.IsValid)
            {
                db.Services.Update(service);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewServices");
            }

            else
            {                
                return View(service);
            }
        }


        [Route("Service/ServiceDelete/{id?}")]
        [HttpGet]
        [ActionName("ServiceDelete")]
        public async Task<IActionResult> ServiceConfirmDelete(int? id)
        {
            if (id != null)
            {
                Service serv = await db.Services.FirstOrDefaultAsync(p => p.Id == id);
                if (serv != null)
                    return View(serv);
            }
            return NotFound();
        }

        [Route("Service/ServiceDelete/{id?}")]
        [HttpPost]
        public async Task<IActionResult> ServiceDelete(int? id)
        {
            if (id != null)
            {
                Service serv = new Service { Id = id.Value };
                db.Entry(serv).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ViewServices");
            }
            return NotFound();
        }
    }
}
