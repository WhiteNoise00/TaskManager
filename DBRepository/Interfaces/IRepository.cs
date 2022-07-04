using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAdminApi.Models;

namespace DBRepository
{
    public interface IRepository : IDisposable
    {
        //Функции для работы с обьектами типа "Пользователь"
        Task<User> GetUser(string email, string password);
        Task<User> GetUserForEmail(string email);
        Task AddNewUser(string email, string password);


        //Функции для работы с обьектами типа "Клиент"
        void CreateClient(Client cl);
        IQueryable<Client> GetClientsListForPage(string name, int? id, int page = 1);    
        Task<List<Client>> GetClientsList();
        Task<Client> GetClient(int id);      
        Task<Client> GetClientWithServices(int id);
        Client EditPostClient(Client cl, int[] selectedServices);
        void UpdateClient(Client client);        
        void DeleteClient(int? id);

        //Функции для работы с обьектами типа "Услуга"
        void CreateService(Service serv, bool selectedTime);
        IQueryable<Service> GetServicesListForPage(int page = 1);
        Task<List<Service>> GetServicesList();              
        Task<Service> GetServiceWithClients(int id);
        Task<Service> GetService(int id);
        Service EditPostService(Service serv);
        void UpdateService(Service service);
        void DeleteService(int? id);

        //Функции для работы с обьектами в промежуточной таблице(ServicesForClient)
         Task<ServicesForClient> ServiceClientGet(int? client_id, int? service_id);
        ServicesForClient ServiceClientEditPost(ServicesForClient serv, int client_id, int service_id);
        void ServiceClientDelete(int? client_id, int? service_id);
    }
}
