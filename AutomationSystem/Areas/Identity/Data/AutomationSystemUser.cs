using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AutomationSystem.Areas.Identity.Data;

public class AutomationSystemUser : IdentityUser
{

    public string Name { get; set; }

    public string SecondName { get; set; }
    
}

