﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserProfile
    {
        [Display(Name = "Id пользователя")]
        public string UserId { get; set; }

        [Display(Name = "Пользователь")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Имя профайла")]
        public string Name { get; set; }

        public virtual ICollection<GPSTracker> GPSTrackers { get; set; }
        
        public UserProfile()
        {
            GPSTrackers = new HashSet<GPSTracker>();
        }
    }

    
}