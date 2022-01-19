using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Currency_Exchange.Models
{

    //FOR THE VIEW, CREATE, UPDATE AND DELETE OF STAFFS
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        public string StaffName { get; set; }

        public string StaffEmail { get; set; }

        public int StaffPhNum { get; set; }
    }
}
