using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Models
{
    public enum TransactionStatus
    {
        Pending,
        Closed,
    }

    public enum ErrorType
    {
        None = 0,
        NotFound,
        BadRequest,
    }
}
