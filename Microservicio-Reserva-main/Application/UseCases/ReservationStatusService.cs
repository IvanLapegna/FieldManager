using Application.Dtos.Response;
using Application.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class ReservationStatusService : IReservationStatusService
    {
        private readonly IReservationStatusQuery _reservationStatusQuery;
        private readonly IMapper _mapper;

        public ReservationStatusService(IReservationStatusQuery reservationStatusQuery, IMapper mapper)
        {
            _reservationStatusQuery = reservationStatusQuery;
            _mapper = mapper;
        }

        public async Task<ICollection<ReservationStatusResponse>> GetAllStatus()
        {
            var status = await _reservationStatusQuery.GetAll();
            return _mapper.Map<ICollection<ReservationStatusResponse>>(status);

        }
    }
}
