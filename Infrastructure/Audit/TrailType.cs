using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Audit;

internal enum TrailType
{
    None = 1,
    Create = 2,
    Update = 3,
    Delete = 4,
}
