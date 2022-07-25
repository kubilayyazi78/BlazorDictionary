using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Api.Domain.Models;
using BlazorDictionary.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Infrastructure.Persistence.Repositories;

public class EntryRepository : GenericRepository<Entry>, IEntryRepository
{
    public EntryRepository(BlazorDictionaryContext dbContext) : base(dbContext)
    {
    }
}