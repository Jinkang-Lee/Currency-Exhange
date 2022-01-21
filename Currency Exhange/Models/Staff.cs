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
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UserPw { get; set; }

        public string FullName { get; set; }

        
        public int Ph_Num { get; set; }
    }
}
