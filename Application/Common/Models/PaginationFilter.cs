using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.Common.Models;

public class PaginationFilter : BaseFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; } = int.MaxValue;
}
