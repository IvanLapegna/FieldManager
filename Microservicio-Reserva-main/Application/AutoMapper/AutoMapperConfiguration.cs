using Application.Dtos.Request;
using Application.Dtos.Response;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration() 
        {
            CreateMap<ReservationCreateRequest, Reservation>()
                .ForMember(r => r.Date, opt => opt.MapFrom(rr => ConvertToDateOnly(rr.Year ,rr.Month, rr.Day)))
                .ForMember(r => r.StartTime, opt => opt.MapFrom(rr => ConvertToTimeOnly(rr.StartHour)))
                .ForMember(r => r.EndTime, opt => opt.MapFrom(rr => ConvertToTimeOnly(rr.EndHour)))
                .ForMember(r => r.FieldID, opt => opt.MapFrom(rr => rr.FieldID))
                .ForMember(r => r.OwnerUserID, opt => opt.MapFrom(rr => rr.UserID))
                .ForMember(r => r.MaxJugadores, opt => opt.MapFrom(rr => rr.MaxJugadores));

            CreateMap<ReservationUpdateRequest, ReservationCreateRequest>()
               .ForMember(r => r.Day, opt => opt.MapFrom(rr => rr.Day))
               .ForMember(r => r.Month, opt => opt.MapFrom(rr => rr.Month))
               .ForMember(r => r.StartHour, opt => opt.MapFrom(rr => rr.StartHour))
               .ForMember(r => r.EndHour, opt => opt.MapFrom(rr => rr.EndHour))
               .ForMember(r => r.FieldID, opt => opt.MapFrom(rr => rr.FieldID))
               .ForMember(r => r.MaxJugadores, opt => opt.MapFrom(rr => rr.MaxJugadores))
               .ReverseMap();



            CreateMap<ReservationUpdateRequest, Reservation>()
                .ForMember(r => r.Date, opt => opt.MapFrom(rr => ConvertToDateOnly(rr.Year, rr.Month, rr.Day)))
                .ForMember(r => r.StartTime, opt => opt.MapFrom(rr => ConvertToTimeOnly(rr.StartHour)))
                .ForMember(r => r.EndTime, opt => opt.MapFrom(rr => ConvertToTimeOnly(rr.EndHour)))
                .ForMember(r => r.FieldID, opt => opt.MapFrom(rr => rr.FieldID))
                .ForMember(r => r.MaxJugadores, opt => opt.MapFrom(rr => rr.MaxJugadores))
                .ForMember(r => r.OwnerUserID, opt => opt.Ignore());


            CreateMap<Reservation, ReservationResponse>()
                .ForMember(rp => rp.ReservationID, opt => opt.MapFrom(r => r.ReservationID))
                .ForMember(rp => rp.OwnerUserID, opt => opt.MapFrom(r => r.OwnerUserID))
                .ForMember(rp => rp.Date, opt => opt.MapFrom(r => r.Date))
                .ForMember(rp => rp.StartTime, opt => opt.MapFrom(r => r.StartTime))
                .ForMember(rp => rp.EndTime, opt => opt.MapFrom(r => r.EndTime))
                .ForMember(rp => rp.Status, opt => opt.MapFrom(r => r.StatusNavigator))
                .ForMember(rp => rp.Players, opt => opt.MapFrom(r => r.Players))           
                .ReverseMap();

            CreateMap<ReservationStatus, ReservationStatusResponse>()
            .ForMember(rsr => rsr.Id, opt => opt.MapFrom(rs => rs.Id))
            .ForMember(rsr => rsr.Status, opt => opt.MapFrom(rs => rs.Status));

            CreateMap<PlayersRequest, Players>()
                .ForMember(pr => pr.ReservationID, opt => opt.MapFrom(p => p.ReservationID))
            
                .ForMember(pr => pr.UserID, opt => opt.MapFrom(p => p.UserID));


            CreateMap<Players, PlayersResponse>()

                .ForMember(pr => pr.ReservationID, opt => opt.MapFrom(p => p.ReservationID))
                .ForMember(pr => pr.Id, opt => opt.MapFrom(p => p.UserID))
                .ReverseMap();
                



        }

        private DateOnly ConvertToDateOnly(int year ,int month, int day)
        {               
            return new DateOnly(year, month, day);
        }

        private TimeOnly ConvertToTimeOnly(int hour)
        {
            return new TimeOnly(hour, 0); // Minutos en 0
        }


    }
}
