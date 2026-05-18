using BoardGameShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();

        Task<Reservation?> GetByIdAsync(int id);

        Task AddAsync(Reservation reservation);
    }
}
