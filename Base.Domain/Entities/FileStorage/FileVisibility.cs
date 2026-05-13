using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities.FileStorage;

public enum FileVisibility
{
    // All users can see
    PUBLIC = 0,
    // Non Admin & Superadmin can see it only if they fit the role
    AUTHENTICATED = 1,
    // Only the user upload & the Admin & Superadmin
    PRIVATE = 2
}
