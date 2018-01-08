using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data
{
    public class ApiRepository<T> : IRepository<T> where T : BaseEntity
    {
        private static IReadOnlyDictionary<Type, string> STORED_PROCEDURES = new Dictionary<Type, string>
        {
        };

        private ApiDbContext _context;

        public ApiRepository(ApiDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
