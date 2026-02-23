using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identities;

public class ApplicationRole : IdentityRole
{
    public AccessLevel AccessLevel { get; set; }
}
