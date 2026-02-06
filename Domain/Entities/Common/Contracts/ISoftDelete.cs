using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Common.Contracts;

public interface ISoftDelete
{
    DateTime? DeletedOn { get; set; }
    Guid? DeletedBy { get; set; }
}
