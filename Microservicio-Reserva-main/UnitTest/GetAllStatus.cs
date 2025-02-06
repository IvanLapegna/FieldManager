using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Interfaces;
using Application.UseCases;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Numerics;
using Microsoft.AspNetCore.Http;
using Application.AutoMapper;
using Application.Exceptions;

namespace UnitTest
{
    public class GetAllStatus
    {
        [Fact]
        public async Task GetAllStatus_ShouldReturnStatusList()
        {
            // Arrange

            var mockReservationStatusQuery = new Mock<IReservationStatusQuery>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();



            var reservationStatusService = new ReservationStatusService(
                mockReservationStatusQuery.Object,
                mapper
            );

            var statusList = new List<ReservationStatus> { 
                    new ReservationStatus { Id = 1, Status = "Reserved"},
                    new ReservationStatus { Id = 2, Status = "Finished" },
                    new ReservationStatus { Id = 3, Status = "Cancelled" }
            };

            mockReservationStatusQuery.Setup(x => x.GetAll())
                .ReturnsAsync(statusList);

            var expectedResult = new List<ReservationStatusResponse>
            {       new ReservationStatusResponse { Id = 1, Status = "Reserved"},
                    new ReservationStatusResponse { Id = 2, Status = "Finished" },
                    new ReservationStatusResponse { Id = 3, Status = "Cancelled" }
            };

            // Act
            var result = await reservationStatusService.GetAllStatus();

            // Assert
            Assert.NotNull( result );
            var resultList = result.ToList();
            for ( int i = 0; i < result.Count; i++) 
            {
                Assert.Equal(expectedResult[i].Id, resultList[i].Id );
                Assert.Equal(expectedResult[i].Status, resultList[i].Status);

            }



        }
    }
}
