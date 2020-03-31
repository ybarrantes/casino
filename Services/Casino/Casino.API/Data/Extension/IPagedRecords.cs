using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Extension
{
    public interface IPagedRecords
    {
        int RecordsPerPage { get; set; }
        int TotalRecords { get; }
        int Page { get; set; }
        int TotalPages { get; }
        IEnumerable Result { get; set; }

        Task Build();
    }
}
