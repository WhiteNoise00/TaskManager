using DBRepository;
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
        private readonly IRepository db;

        public ServiceController(IRepository context)
        {
            db = context;
        }

        public async Task<IActionResult> ViewServices(int page = 1)
        {
            var services = db.GetServicesListForPage(page);
            int pageSize = 5;
            var count =  services.Count();
            var items =await services.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Services = items
            };

            if (services != null)
            {
                return View(viewModel);
            }
            else
            { return NotFound(); }
        }
 
        [Route("Service/CreateService")]
        public async Task<IActionResult> CreateService()
        {
            return View();
        }

        [Route("Service/CreateService")]
        [HttpPost]
        public async Task<IActionResult> CreateService(Service serv, bool selectedTime)
        {
            if (ModelState.IsValid)
            {
                db.CreateService(serv, selectedTime);
                return RedirectToAction("ViewServices");
            }
            return View();
        }

        [HttpGet]
        [Route("Service/ServiceEdit")]
        public async Task<IActionResult> ServiceEdit(int? id)
        {
            if (id != null)
            {
                Service serv = db.GetServiceWithClients(id.Value);
                return View(serv);
            }
            return NotFound();
        }


        [Route("Service/ServiceEdit")]
        [HttpPost]
        public async Task<IActionResult> ServiceEdit(Service serv)
        {
            Service service = db.EditPostService(serv);
            service.Service_Name = serv.Service_Name;
            service.Service_Description = serv.Service_Description;
            service.Service_Time_Type_Minutes = serv.Service_Time_Type_Minutes;
            service.Service_Time_Type_Hours = serv.Service_Time_Type_Hours;

            if (ModelState.IsValid)
            {
                db.UpdateService(service);
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
                Service serv = db.GetService(id.Value);
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
                db.DeleteService(id.Value);
                return RedirectToAction("ViewServices");
            }
            return NotFound();
        }
    }
}
