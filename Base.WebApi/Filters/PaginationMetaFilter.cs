using Base.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Common;

namespace Host.Filters;

public class PaginationMetaFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { Value: ApiResponse<object> apiResponse }
            && apiResponse.Data is IPagedList pagedList)
        {
            apiResponse.Data = pagedList.Items;
            apiResponse.Meta = new PaginationMeta
            {
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                HasPreviousPage = pagedList.HasPreviousPage,
                HasNextPage = pagedList.HasNextPage
            };
        }

        await next();
    }
}
