﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QualityCaps.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //week 7
       
     //   public String Name { get; set; }
        public String PhoneNumber { get; set; }
        public bool Enabled { get; set; }
        public String Address { get; set; }
    }
}
