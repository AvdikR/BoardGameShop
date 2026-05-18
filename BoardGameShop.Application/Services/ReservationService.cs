using System;
using System.Collections.Generic;
using System.Text;
using BoardGameShop.Application.DTOs;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Enums;
using BoardGameShop.Domain.Interfaces;

namespace BoardGameShop.Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();

            return list.Select(r => new ReservationDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                GameSessionName = r.GameSessionName,
                Date = r.Date,
                DurationHours = r.DurationHours,
                Status = r.Status.ToString()
            });
        }

        public async Task CreateAsync(CreateReservationDto dto)
        {
            var reservation = new Reservation
            {
                CustomerId = dto.CustomerId,
                GameSessionName = dto.GameSessionName,
                Date = dto.Date,
                DurationHours = dto.DurationHours,
                Status = ReservationStatus.Created
            };

            await _repository.AddAsync(reservation);
        }
    }
}
