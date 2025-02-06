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
    public class DeletePlayerReservation_UnitTesting
    {
        [Fact]
        public async Task DeletePlayer_CorrectExecution()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockMapper = new Mock<IMapper>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();

            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                null,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );

            var reservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");
            int playerId = 2;


            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = reservationId,
                        FieldID = new Guid("11111111-9C67-48B9-D011-08DD124C3A2E"),
                        OwnerUserID = 1,
                        ReservationStatusID = 1,
                        Date = new DateOnly(2025, 1, 14),
                        StartTime = TimeOnly.Parse("10:00"),
                        EndTime = TimeOnly.Parse("11:00"),
                        MaxJugadores = 14,
                        StatusNavigator = new ReservationStatus
                        {
                            Id = 1,
                            Status = "Reserved"
                        },
                        Players = new List<Players>
                        {
                            new Players
                            {
                                ReservationID = reservationId,
                                UserID = 1,
                            },
                            new Players { ReservationID = reservationId, UserID = 2, }
                        }
                    }

                );






            // Act
            await reservationService.DeletePlayerReservation(reservationId, playerId);

            // Assert
            mockReservationQuery.Verify(x => x.GetReservationById(reservationId), Times.Once);
            mockPlayersCommand.Verify(x => x.DeletePlayer(playerId, reservationId), Times.Once);


        }


        [Fact]
        public async Task DeletePlayer_CannotDeleteOwnerException()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockMapper = new Mock<IMapper>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();

            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                null,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );

            var reservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");
            int playerId = 1;


            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = reservationId,
                        FieldID = new Guid("11111111-9C67-48B9-D011-08DD124C3A2E"),
                        OwnerUserID = 1,
                        ReservationStatusID = 1,
                        Date = new DateOnly(2025, 1, 14),
                        StartTime = TimeOnly.Parse("10:00"),
                        EndTime = TimeOnly.Parse("11:00"),
                        MaxJugadores = 14,
                        StatusNavigator = new ReservationStatus
                        {
                            Id = 1,
                            Status = "Reserved"
                        },
                        Players = new List<Players>
                        {
                            new Players
                            {
                                ReservationID = reservationId,
                                UserID = 1,
                            },
                            new Players { ReservationID = reservationId, UserID = 2, }
                        }
                    }

                );






            // Act

            // Assert
            await Assert.ThrowsAsync<CannotDeleteOwnerException>(
            async () => await reservationService.DeletePlayerReservation(reservationId,playerId)
            );


        }


        [Fact]
        public async Task DeletePlayer_PlayerNotInReservationException()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockMapper = new Mock<IMapper>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();

            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                null,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );

            var reservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");
            int playerId = 3;


            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = reservationId,
                        FieldID = new Guid("11111111-9C67-48B9-D011-08DD124C3A2E"),
                        OwnerUserID = 1,
                        ReservationStatusID = 1,
                        Date = new DateOnly(2025, 1, 14),
                        StartTime = TimeOnly.Parse("10:00"),
                        EndTime = TimeOnly.Parse("11:00"),
                        MaxJugadores = 14,
                        StatusNavigator = new ReservationStatus
                        {
                            Id = 1,
                            Status = "Reserved"
                        },
                        Players = new List<Players>
                        {
                            new Players
                            {
                                ReservationID = reservationId,
                                UserID = 1,
                            },
                            new Players { ReservationID = reservationId, UserID = 2, }
                        }
                    }

                );






            // Act

            // Assert
            await Assert.ThrowsAsync<PlayerNotInReservationException>(
            async () => await reservationService.DeletePlayerReservation(reservationId, playerId)
            );


        }

    }
}
