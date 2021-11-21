using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskAdminApi.Models
{
    public class WorkTime
    {
        public int Id { get; set; }





        // Дата начала задачи
        [Required(ErrorMessage = "Введите дату постановки задачи: dd.mm.yyyy")]
        [Display(Name = "Дата постановки задачи:")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Task_Beginning_Date { get; set; }

        // Дата окончания задачи
        [Required(ErrorMessage = "Введите дату окончания задачи: dd.mm.yyyy")]
        [Display(Name = "Дата окончания задачи:")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Task_Ending_Date { get; set; }
        //Список сотрудников для конкректной задачи
   
    }
}
