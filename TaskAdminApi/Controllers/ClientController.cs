using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAdminApi.Models;

namespace TaskAdminApi.Controllers
{
    public class ClientController : Controller
    {
     

        private TaskAdminContext db;
        public ClientController(TaskAdminContext context)
        {
            db = context;
        }

        public async Task<IActionResult> ViewClients()
        {
            var clients = await db.Clients.ToListAsync();
            //IQueryable<Client> clients = (IQueryable<Client>)db.Clients.ToList();//А должно тянуться из связки Клиент - Услуги
            if (clients != null)
            {
                return View(clients);
            }

            else
            { return NotFound(); }
        }



        /* public IActionResult ViewClients()
         {
             var services = db.Services.ToList();
             return View(services);
         }*/

        /*  [Route("Department/ViewDepartments")]
          public async Task<IActionResult> ViewDepartments(int page = 1)
          {
              IQueryable<Department> dep = db.Departments.Include(t => t.Positions);

              int pageSize = 3;

              var count = await dep.CountAsync();
              var items = await dep.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

              PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
              IndexViewModel viewModel = new IndexViewModel
              {
                  PageViewModel = pageViewModel,
                  Departments = items
              };
              return View(viewModel);
          }
        */

        /*[Route("Employee/ViewEmployees")]
        public async Task<IActionResult> ViewEmployees(string name, int? id, int page=1)
        {                      
            IQueryable<Employee> em = db.Employees.Include(t => t.Tasks).Include(t => t.Position.Department);

            int pageSize = 3;

            if (id != null && id != 0)
            {
                em = em.Where(e => e.Tasks.Any(t => t.Id == id));
            }

            if (!String.IsNullOrEmpty(name))
            {
                em = em.Where(x => EF.Functions.Like(x.Employee_Last_Name, $"%{name}%"));
            }

            var tasksValue = await db.Tasks.ToListAsync();
            tasksValue.Insert(0, new Models.Task { Task_Name = "Все", Id = 0 });
            SelectList tasks = new SelectList(tasksValue, "Id", "Task_Name");
            ViewBag.TasksList = tasks;            

            //Постраничный вывод
            var count = await em.CountAsync();
            var items = await em.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Employees = items
            };
            return View(viewModel);

        }*/

        [Route("Client/CreateClient")]
        public async Task<IActionResult> CreateClient()
        {
           // var positionsValue = await db.Positions.ToListAsync();
            var ServicesValue = await db.Services.ToListAsync();
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
            }

        }

        [Route("Client/CreateClient")]
        [HttpPost]
        public async Task<IActionResult> CreateClient(Client cl, int servid)
        {
          // Service serv = await db.Services.FirstOrDefaultAsync(p => p.Id == servid);
            
            if (ModelState.IsValid)
            {
                //cl.Services.Add(serv);//у меня ведь может добавляться несколько услуг на одного клиента
                // И это будет не выпадающий список
                db.Clients.Add(cl);
                db.SaveChanges();
                return RedirectToAction("ViewClients");
            }

            return View();
        }

        //добавить здесь выпадающий список услуг с отметкой - выполнено и статус оплаты
        [Route("Client/ClientDetails/{id?}")]
        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id != null)
            {
                Client cl = await db.Clients.Include(e => e.Services).FirstOrDefaultAsync(e => e.Id == id);
                if (cl != null)
                {
                    return View(cl);
                }
            }
            return NotFound();
        }

        
        [HttpGet]
        [Route("Client/ClientEdit")]
        public async Task<IActionResult> ClientEdit(int? id)
        {
            if (id != null)
            {
                Client cl = await db.Clients.Include(e => e.Services).FirstOrDefaultAsync(e => e.Id == id);
                List<Service> serv = await db.Services.ToListAsync();
                SelectList services = new SelectList(serv, "Id", "Service_Name");
                ViewBag.ServicesList = services;
                ViewBag.Services = db.Services.ToList();
                return View(cl);
            }
            return NotFound();
        }


        [Route("Client/ClientEdit")]
        [HttpPost]
        public async Task<IActionResult> ClientEdit(Client cl, int[] selectedServices, int[] selectedStatus)
        {

            Client client = await db.Clients.Include(e => e.Services).FirstOrDefaultAsync(e => e.Id == cl.Id);


            client.Client_Name = cl.Client_Name;
            client.Client_Contact_Email = cl.Client_Contact_Email;
            client.Client_Contact_Person = cl.Client_Contact_Person;
            client.Client_Contact_Phone = cl.Client_Contact_Phone;
            //здесь добавить возможность отмечать оплату выбранных услуг
            client.Services.Clear();

            if (selectedServices != null)
            {
                foreach (var c in db.Services.Where(co => selectedServices.Contains(co.Id)))
                {
                    client.Services.Add(c);
                }
            }

           /* if (selectedStatus != null)
            {
                foreach (var c in db.Services.Where(co => selectedStatus.Contains(co.Id)))
                {
                    client.Services.;
                }
            }*/
           // Лучше пока сделать отдельную страницу -Клиент с такой то услугой и изменять статус именно этой связки

            if (ModelState.IsValid)
            {
                db.Clients.Update(client);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewClients");
            }

            else
            {
                ViewBag.Services = db.Services.ToList();
                return View(client);
            }
        }

     
        [Route("Client/ClientDelete/{id?}")]
        [HttpGet]
        [ActionName("ClientDelete")]
        public async Task<IActionResult> ClientConfirmDelete(int? id)
        {
            if (id != null)
            {
                Client cl = await db.Clients.FirstOrDefaultAsync(p => p.Id == id);
                if (cl != null)
                    return View(cl);
            }
            return NotFound();
        }

        [Route("Client/ClientDelete/{id?}")]
        [HttpPost]
        [ActionName("ClientDelete")]
        public async Task<IActionResult> ClientDelete(int? id)
        {
            if (id != null)
            {
                Client cl = new Client { Id = id.Value };
                db.Entry(cl).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ViewClients");
            }
            return NotFound();
        }
    }
}
