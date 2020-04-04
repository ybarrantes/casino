using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Casino.Services.Util.Collections
{
    public sealed class PagedRecords<T> : IPagedRecords<T>, IDisposable where T : class
    {
        private int _recordsPerPage = 15;
        private int _page = 0;
        private int _totalPages = 0;
        private int _totalRecords = 0;

        public int RecordsPerPage => _recordsPerPage;

        public int TotalRecords => _totalRecords;

        public int Page => _page;

        public int TotalPages => _totalPages;

        public IEnumerable Result { get; set; }

        private int CheckMinimumValue(int value, int min)
        {
            if (value < min)
                throw new InvalidOperationException("page minimium value is 1");

            return value;
        }

        public async Task<IPagedRecords<T>> Build(IQueryable<T> entityQueryBuilder, int page, int recodsPerPage)
        {
            _page = CheckMinimumValue(page, 1);
            _recordsPerPage = CheckMinimumValue(recodsPerPage, 1);

            _totalRecords = await entityQueryBuilder.CountAsync();

            Result = await entityQueryBuilder
                .Skip(_recordsPerPage * (Page - 1))
                .Take(_recordsPerPage)
                .ToListAsync();

            _totalPages = ((int)Math.Ceiling((double)_totalRecords / _recordsPerPage));

            if (_totalPages == 0)
                _page = 0;

            return this;
        }

        public void Dispose()
        {
            Result = null;
        }
    }
}
