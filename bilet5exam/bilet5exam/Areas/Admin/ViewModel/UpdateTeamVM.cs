using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bilet5exam.Areas.Admin.ViewModel
{
    public class UpdateTeamVM
    {
        
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(30)]

        public string Surname { get; set; }
        [Required, MaxLength(30)]

        public string Position { get; set; }
        [Required, MaxLength(30)]

        public string JobDescription { get; set; }
        [Required]

        public IFormFile Image { get; set; }
    }
}
