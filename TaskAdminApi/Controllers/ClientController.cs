using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAdminApi.Models;
using DBRepository;
using Microsoft.AspNetCore.Authorization;

namespace TaskAdminApi.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IRepository db;

        public ClientController(IRepository context)
        {
            db= context;
        }

        public async  Task<IActionResult> GetClient(int id )
        {
            var client = await db.GetClient(id);
            return View(client);
        }

        public async Task<IActionResult> ViewClients(string name, int? id, int page = 1)
        {
            var clients = db.GetClientsListForPage(name, id, page);
            List<Service> service_values = await db.GetServicesList();
            service_values.Insert(0, new Service { Service_Name = "Все", Id = 0 });
            SelectList services = new SelectList(service_values, "Id", "Service_Name");
            ViewBag.ServicesList = services;

            int pageSize = 5;
            int count = clients.Count();
            var items = await clients.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Clients = items
            };

            if (clients != null)
            {
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("Client/ClientEdit")]
        [ActionName("ClientEdit")]
        public async Task<IActionResult> EditClient(int? id)
        {
            if (id != null)
            {
                Client cl = await db.GetClientWithServices(id.Value);
                List<Service> serv = await db.GetServicesList();
                SelectList services = new SelectList(serv, "Id", "Service_Name");
                ViewBag.ServicesList = services;
                ViewBag.Services = await db.GetServicesList();
                return View(cl);
            }
            return NotFound();
        }

        [Route("Client/ClientEdit")]
        [HttpPost]
        [ActionName("ClientEdit")]
        public async Task<IActionResult> EditClient(Client cl, int[] selectedServices)
        {
            Client client = db.EditPostClient(cl, selectedServices);
            if (ModelState.IsValid)
            {
                db.UpdateClient(client);
                return RedirectToAction("ViewClients");
            }
            else
            {
                ViewBag.Services = await db.GetServicesList();
                return View(client);
            }
        }

        [Route("Client/CreateClient")]
        public async Task<IActionResult> CreateClient()
        {
            var ServicesValue = await db.GetServicesList();
            if (ServicesValue != null)
            {
                List<Service> serv = ServicesValue;
                SelectList services = new SelectList(serv, "Id", "Service_Name");
                ViewBag.ServiceList = services;
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("Client/CreateClient")]
        [HttpPost]
        public IActionResult CreateClient(Client cl)
        {
            if (ModelState.IsValid)
            {
                db.CreateClient(cl);
                return RedirectToAction("ViewClients");
            }
            return View();
        }

        [Route("Client/ClientDetails/{id?}")]
        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id != null)
            {
                Client cl =  await db.GetClientWithServices(id.Value);
                if (cl != null)
                {
                    return View(cl);
                }
            }
            return NotFound();
        }

        [HttpGet]
        [Route("Client/ServiceClientEdit")]
        public async Task<IActionResult> ServiceClientEdit(int? client_id, int? service_id, string service_name)
        {
            ViewBag.Name = service_name;
            var service = await db.GetService(service_id.Value);
            ViewBag.Minutes = service.Service_Time_Type_Minutes;

            var serv = await db.ServiceClientGet(client_id.Value, service_id.Value);
            if (serv != null)
            {              
                return View(serv);
            }
            else { return NotFound(); }
        }

        [Route("Client/ServiceClientEdit")]
        [HttpPost]
        public IActionResult ServiceClientEdit(ServicesForClient serv, int client_id, int service_id)
        {
            ServicesForClient servcl = db.ServiceClientEditPost(serv, client_id, service_id);
            if (servcl != null)
            {
                return RedirectToAction("ViewClients");
            }
            else { return NotFound(); }
        }

        [Route("Client/ServiceClientDelete")]
        [HttpGet]
        [ActionName("ServiceClientDelete")]
        public async Task<IActionResult> ServiceClientConfirmDelete(int? client_id, int? service_id)
        {
            if (client_id != null && service_id != null)
            {
                var srcl = await db.ServiceClientGet(client_id.Value, service_id.Value);
                return View(srcl);
            }
            return NotFound();
        }

        [Route("Client/ServiceClientDelete")]
        [HttpPost]
        [ActionName("ServiceClientDelete")]
        public IActionResult ServiceClientDelete(int? client_id, int? service_id)
        {
            if (client_id != null && service_id != null)
            {
                db.ServiceClientDelete(client_id, service_id);
                return RedirectToAction("ViewClients");
            }
            return NotFound();
        }    

        [Route("Client/ClientDelete/{id?}")]
        [HttpGet]
        [ActionName("ClientDelete")]
        public async Task<IActionResult> ClientConfirmDelete(int? id)
        {
            if (id != null)
            {
                Client cl = await db.GetClient(id.Value);
                if (cl != null)
                return View(cl);
            }
            return NotFound();
        }

        [Route("Client/ClientDelete/{id?}")]
        [HttpPost]
        [ActionName("ClientDelete")]
        public IActionResult DeleteEntity(int? id)
        {
            if (id != null)
            {
                db.DeleteClient(id);
                return RedirectToAction("ViewClients");
            }
            return NotFound();
        }
    }
}
