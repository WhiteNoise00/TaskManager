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

        // Затраченное время
        [Display(Name = "Затраченное время:")]
        public double Service_Time { get; set; }

        // Услуга измеряется в минутах
        [DefaultValue(false)]
        public bool Service_Time_Type_Minutes { get; set; }

        // Значение по умолчание - день прикрепления или создания услуги
        [Display(Name = "Дата начала выполнения услуги:")]       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Client_Service_Beginning_Date { get; set; }

        // Дата окончания выполнения услуги 
        [Display(Name = "Дата окончания выполнения услуги:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Client_Service_Ending_Date { get; set; }

        [Display(Name = "Дата оплаты услуги:")]
        [DisplayFormat(DataFormatString = "{YYYY-MM-DD}", ApplyFormatInEditMode = true)]
        public DateTime Client_Service_Payment_Date { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }

    }
}
