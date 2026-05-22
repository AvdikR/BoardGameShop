using BoardGameShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardGameShop.Domain.Interfaces
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetActivePromotionsAsync(DateTime at);
    }
}
