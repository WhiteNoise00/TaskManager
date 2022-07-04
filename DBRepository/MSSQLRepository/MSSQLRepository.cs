using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAdminApi;
using TaskAdminApi.Models;

namespace DBRepository.MSSQLRepository
{
    public class MSSQLRepository: IRepository
    {
        private readonly TaskAdminContext db;

        public MSSQLRepository(TaskAdminContext context)
        {
            db = context;
        }

        /*Авторизация*/
        public async Task<User> GetUser(string email, string password)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
        public async Task<User> GetUserForEmail(string email)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task AddNewUser(string email, string password)
        {
            db.Users.Add(new User { Email = email, Password = password });
            await db.SaveChangesAsync();
        }


        /*Функции для работы с обьектами типа "Клиент"*/
        public IQueryable<Client> GetClientsListForPage(string name, int? id, int page = 1)
        {
            IQueryable<Client> clients = db.Clients.Include(t => t.Services).Include(s => s.ServicesForClients);

            if (id != null && id != 0)
            {
                clients = clients.Where(e => e.Services.Any(t => t.Id == id));
            }

            if (!String.IsNullOrEmpty(name))
            {
                clients = clients.Where(x => EF.Functions.Like(x.Client_Name, $"%{name}%"));
            }          

            if (clients != null)
            {
                return clients.OrderBy(o=>o.Client_Name);
            }
            else
            { 
                return null; 
            } 
        }       

        public async Task<List<Client>> GetClientsList()
        {
            return await db.Clients.ToListAsync();            
        }
       
        public async Task<Client> GetClient(int id)
        {
            return await db.Clients.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Client> GetClientWithServices(int id)
        {     
            return await db.Clients.Include(e => e.Services).FirstOrDefaultAsync(e => e.Id == id);
        }

        public Client EditPostClient(Client cl, int[] selectedServices)
        {
            Client client =db.Clients.Include(e => e.Services).FirstOrDefault(e => e.Id == cl.Id);
            client.Client_Name = cl.Client_Name;
            client.Client_Contact_Email = cl.Client_Contact_Email;
            client.Client_Contact_Person = cl.Client_Contact_Person;
            client.Client_Contact_Phone = cl.Client_Contact_Phone;
            client.Services.Clear();

            if (selectedServices != null)
            {
                foreach (var c in db.Services.Where(co => selectedServices.Contains(co.Id)))
                {
                    client.Services.Add(c);
                }
            }
            return client;
        }

        public void CreateClient(Client cl)
        {
            db.Clients.Add(cl);
            db.SaveChanges();
        }

        public void UpdateClient(Client client)
        {
            db.Clients.Update(client);
            db.SaveChanges();
        }

        public void DeleteClient(int? id)
        {
            if (id != null)
            {
                Client cl = new Client { Id = id.Value };
                db.Entry(cl).State = EntityState.Deleted;
                db.SaveChanges();               
            }          
        }

        /*Функции для работы с обьектами типа "Услуга"*/
        public IQueryable<Service> GetServicesListForPage(int page = 1)
        {
            return db.Services.Include(t => t.Clients).Include(s => s.ServicesForClients);           
        }

        public async Task<List<Service>> GetServicesList()
        {
            return await db.Services.ToListAsync();
        }

        public async Task<Service> GetService(int id)
        {
            return await db.Services.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Service> GetServiceWithClients(int id)
        {
            return await db.Services.Include(e => e.Clients).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void CreateService(Service serv, bool selectedTime)
        {
            if (selectedTime == true)
            {
                serv.Service_Time_Type_Minutes = true;
            }
            else
            {
                serv.Service_Time_Type_Hours = true;
            }
            db.Services.Add(serv);
            db.SaveChanges();
        }

        public Service EditPostService(Service serv)
        {
            Service service = db.Services.FirstOrDefault(e => e.Id == serv.Id);
            service.Service_Name = serv.Service_Name;
            service.Service_Description = serv.Service_Description;
            service.Service_Time_Type_Minutes = serv.Service_Time_Type_Minutes;
            return service;
        }

        public void UpdateService(Service service)
        {
            db.Services.Update(service);
            db.SaveChanges();
        }

        public void DeleteService(int? id)
        {
            Service serv = new Service { Id = id.Value };
            db.Entry(serv).State = EntityState.Deleted;
            db.SaveChanges();           
        }

        /*Функции для работы с обьектами  в промежуточной таблице(ServicesForClient)*/
        public async Task<ServicesForClient> ServiceClientGet(int? client_id, int? service_id)
        {
            Client client = await db.Clients.Include(s => s.ServicesForClients).FirstOrDefaultAsync(s => s.Id == client_id);
            Service service = await db.Services.FirstOrDefaultAsync(c => c.Id == service_id);

            bool time_type = service.Service_Time_Type_Minutes;

            if (client != null && service != null)
            {
                return client.ServicesForClients.FirstOrDefault(sc => sc.ServiceId == service.Id);
            }
            else return null;
        }

        public ServicesForClient ServiceClientEditPost(ServicesForClient serv, int client_id, int service_id)
        {
            Client client = db.Clients.Include(s => s.ServicesForClients).FirstOrDefault(s => s.Id == client_id);
            Service service = db.Services.FirstOrDefault(c => c.Id == service_id);
            bool time_type = service.Service_Time_Type_Minutes;

            if (client != null && service != null)
            {
                ServicesForClient srcl = client.ServicesForClients.FirstOrDefault(sc => sc.ServiceId == service.Id);
                client.ServicesForClients.Remove(srcl);

                DateTime date_beginning, date_ending;              

                if (serv.Client_Service_Beginning_Date.ToString() == "01.01.0001 0:00:00")
                {
                    date_beginning = DateTime.Today; 
                }
                else 
                { 
                    date_beginning = serv.Client_Service_Beginning_Date;
                }

                if (serv.Client_Service_Ending_Date.ToString() == "01.01.0001 0:00:00")
                {
                   date_ending = date_beginning.AddDays(3);
                }
                else
                {
                    date_ending = serv.Client_Service_Ending_Date;
                }

                client.ServicesForClients.Add(new ServicesForClient
                {
                    Service = service,
                    Service_Status_Complete = serv.Service_Status_Complete,
                    Service_Status_Pay = serv.Service_Status_Pay,
                    Client_Service_Beginning_Date = date_beginning,
                    Client_Service_Ending_Date = date_ending,
                    Client_Service_Payment_Date = serv.Client_Service_Payment_Date,
                    Service_Time_Type_Minutes = time_type,
                    Service_Time = serv.Service_Time
                }
                ); 
              
            db.SaveChanges();
                return srcl;
            }
            else { return null; }
        }

        public void ServiceClientDelete(int? client_id, int? service_id)
        {
            if (client_id != null && service_id != null)
            {
                Client client = db.Clients.Include(s => s.ServicesForClients).FirstOrDefault(s => s.Id == client_id);
                Service service = db.Services.FirstOrDefault(c => c.Id == service_id);
                if (client != null && service != null)
                {
                    var srcl = client.ServicesForClients.FirstOrDefault(sc => sc.ServiceId == service.Id);
                    client.ServicesForClients.Remove(srcl);
                    db.SaveChanges();
                }
            }
        }

        /* Управление ресурсами*/
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
    }
}
