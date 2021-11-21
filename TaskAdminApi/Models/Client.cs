using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAdminApi.Models
{
    public class Client
    {
        public int Id { get; set; }

        // Личное имя
        [Required(ErrorMessage = "Введите наименование компании-заказчика")]
        [StringLength(50)]
        [Display(Name = "Компания:")]
        public string Client_Name { get; set; }

        // Фамилия
        [Required(ErrorMessage = "Введите имя контактного лица")]
        [StringLength(100)]
        [Display(Name = "Контактное лицо:")]
        public string Client_Contact_Person { get; set; }
     


        // Электронная почта 
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [Display(Name = "Адрес электронной почты:")]
        [StringLength(100)]
        public string Client_Contact_Email { get; set; }

        // Физический адрес
       // public string Client_Location{ get; set; }


        // Телефонный номер обратной связи
        [Required(ErrorMessage = "Введите телефонный номер")]
        [Display(Name = "Телефонный номер для обратной связи:")]
        [StringLength(12)]
        public string Client_Contact_Phone { get; set; }


       
        [Required(ErrorMessage = "Введите дату принятия задачи к исполнению: dd.mm.yyyy")]
        [Display(Name = "Дата принятия задачи к исполнению:")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Client_Service_Beginning_Date { get; set; }

        // Дата окончания задачи. Можо потом добавить алерты при приближении даты
        [Required(ErrorMessage = "Введите дату окончания задачи: dd.mm.yyyy")]
        [Display(Name = "Дата окончания задачи:")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Client_Service_Ending_Date { get; set; }
        

       /* [Display(Name = "Статус выполнения")]
         [DefaultValue(false)]
         public bool Client_Service_Status_Complete { get; set; }

         [Display(Name = "Статус оплаты")]
         [DefaultValue(false)]
         public bool Client_Service_Status_Pay { get; set; }

         //затраченное время в часах
         [Display(Name = "Затраченное время в часах")]
         [DefaultValue(0)]
         public double Client_Service_Time_Hours { get; set; }

         //затраченное время в минутах       
         [DefaultValue(0)]
         [Display(Name = "Затраченное время в минутах")]
         public int Client_Service_Time_Minutes { get; set; } */


        // Список прикрепелнных задач
        public virtual ICollection<Service> Services { get; set; }

        public Client() { Services = new List<Service>(); }
    }
}
