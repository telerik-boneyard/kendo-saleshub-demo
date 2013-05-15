using System;
using System.Collections.Generic;
using System.Linq;
using SalesHub.Core.Models;
using SalesHub.Core.Repositories;

namespace SalesHub.Data.Repositories
{
    public class SuggestedValueRepository : ISuggestedValueRepository
    {
        public IQueryable<SuggestedValue> GetAllSuggestedValues()
        {
            return new List<SuggestedValue> {
                new SuggestedValue {
                    Value = "Suggested Value 1"
                },
                new SuggestedValue {
                    Value = "Suggested Value 2"
                }
            }.AsQueryable();
        }
    }
}
