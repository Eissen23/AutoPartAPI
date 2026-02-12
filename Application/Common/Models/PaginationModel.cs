using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Common.Models;

public abstract class PaginationModel<T>(List<T> data, int page, int count, int pageSize)
{
    public List<T> Data { get; set; } = data;

    public int CurrentPage { get; set; } = page;

    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);

    public int TotalCount { get; set; } = count;

    public int PageSize { get; set; } = pageSize;

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;


}
