using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskAdminApi.Models
{
    public class ServicesForClient
    {
        public int Id { get; set; }

        // Статус выполнения услуги
        [Display(Name = "Услуга выполнена:")]
        public bool Service_Status_Complete { get; set; }

        // Статус оплаты услуги
        [Display(Name = "Услуга оплачена:")]
        public bool Service_Status_Pay { get; set; }

        // Затраченное время в часах
        [Display(Name = "Затраченное время:")]
        public double Service_Time_Hours { get; set; } 

        // Затраченное время в минутах 
        [Display(Name = "Затраченное время(минуты):")]
        public int Service_Time_Minutes { get; set; }

        // Значение по умолчание - день прикрепления или создания услуги
        [Display(Name = "Дата начала выполнения услуги:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Client_Service_Beginning_Date { get; set; }

        // Дата окончания выполнения услуги 
        [Display(Name = "Дата окончания выполнения услуги:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Client_Service_Ending_Date { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }

    }
}
