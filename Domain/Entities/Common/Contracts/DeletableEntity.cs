using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Common.Contracts;

public class DeletableEntity : DeleteableEntity<DefaultIdType>
{
}

public abstract class DeleteableEntity<T> : BaseEntity<T>, IDeletableEntity
{
    public DefaultIdType CreatedBy { get; set; }

    public DateTime CreatedOn {get; set; }

    public DefaultIdType LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }

    protected DeleteableEntity()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }
}