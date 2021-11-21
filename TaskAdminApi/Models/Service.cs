using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TaskAdminApi.Models
{
    public class Service
    {
        public int Id { get; set; }

        // Приоритет задачи
        [Required(ErrorMessage = "Введите наименование услуги")]
        [StringLength(40)]
        [Display(Name = "Наименование услуги")]
        public string Service_Name { get; set; }

      /*  //Тип измерения: минуты или часы

        [Required(ErrorMessage = "Введите описание задачи")]
        [StringLength(200)]
        [Display(Name = "Описание задачи")]
        public string Task_Description { get; set; }*/

        // Краткое описание задачи
       
        [StringLength(200)]
        [Display(Name = "Описание услуги")]
        public string Service_Description { get; set; }

        /*Перенесено в модель клиента. Данные поля актуальны для экзепляра клиента, а для экземпляра услуги*/
        /*Вопрос: создается ли отдельно экземпляр услуги для каждого клиента? Вряд ли. Просто они связываются*
         * / Вообще, глянуть в базу, что по итогу создается при создании модели из-под NF Core. 
         * Но у клиента может быть несколько услуг. Тогда потом создадим соединительную таблицу и класс под нее*/

        [Display(Name = "Статус выполнения")]
        [DefaultValue(false)]
        public bool Service_Status_Complete { get; set; }

        [Display(Name = "Статус оплаты")]
        [DefaultValue(false)]
        public bool Service_Status_Pay { get; set; }

        //затраченное время в часах
        [Display(Name = "Затраченное время в часах")]
        [DefaultValue(0)]
        public double Service_Time_Hours { get; set; }

        //затраченное время в минутах       
        [DefaultValue(0)]
        [Display(Name = "Затраченное время в минутах")]
        public int Service_Time_Minutes { get; set; } 

        /*В чем измереять*/
        [DefaultValue(false)]
        public bool Service_Time_Type_Minutes { get; set; }

        [DefaultValue(false)]
        public bool Service_Time_Type_Hours { get; set; }


        //Список сотрудников для конкректной задачи
        public virtual ICollection<Client> Clients { get; set; }

        public Service() { Clients = new List<Client>(); }
    }
}
