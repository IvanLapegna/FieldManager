using Application.Interfaces;
using Application.UseCases;
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
using System.Net.WebSockets;

namespace UnitTest
{
    public class AddPlayer_UnitTesting
    {
        [Fact]
        public async Task AddPlayer_CorrectExecution()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();


            var mockPlayesQuery = new Mock<IPlayersQuery>();
            var playersService = new PlayersService(
            mockPlayersCommand.Object,
            mockPlayesQuery.Object,
            mapper
            );


            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                playersService,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );


            var playerRequest = new PlayersRequest
            {
                ReservationID = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E"),
                UserID = 3,
            };

            var playerResponse = new PlayersResponse
            {
                Id = 3,
                Name = "Marcos",
                Email = "Marcos@gmail.com"
            };

            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = playerRequest.ReservationID,
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
                                ReservationID = playerRequest.ReservationID,
                                UserID = 1,
                            },
                            new Players { ReservationID = playerRequest.ReservationID, UserID = 2, }
                        }
                    }

                );


            var token = "test-token";
            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(),
                token
            )).ReturnsAsync(playerResponse);




            // Act
            await reservationService.AddPlayer(playerRequest);

            // Assert
            mockReservationQuery.Verify(x => x.GetReservationById(playerRequest.ReservationID), Times.Once);
            mockReservationQuery.Verify(x => x.ReservationExists(playerRequest.ReservationID), Times.Once);
            mockPlayersCommand.Verify(x => x.InsertPlayer(It.IsAny<Players>()), Times.Once);

        }





        [Fact]
        public async Task AddPlayer_CannotAddReservationOwnerException()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();


            var mockPlayesQuery = new Mock<IPlayersQuery>();
            var playersService = new PlayersService(
            mockPlayersCommand.Object,
            mockPlayesQuery.Object,
            mapper
            );


            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                playersService,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );


            var playerRequest = new PlayersRequest
            {
                ReservationID = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E"),
                UserID = 1,
            };

            var playerResponse = new PlayersResponse
            {
                Id = 1,
                Name = "Ivan",
                Email = "Ivan@gmail.com"
            };

            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = playerRequest.ReservationID,
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
                                ReservationID = playerRequest.ReservationID,
                                UserID = 1,
                            },
                            new Players { ReservationID = playerRequest.ReservationID, UserID = 2, }
                        }
                    }

                );


            var token = "test-token";
            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(),
                token
            )).ReturnsAsync(playerResponse);




            // Act

            // Assert
            await Assert.ThrowsAsync<CannotAddReservationOwnerException>(async () => await reservationService.AddPlayer(playerRequest));

        }




        [Fact]
        public async Task AddPlayer_PlayerAlreadyExistsException()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();


            var mockPlayesQuery = new Mock<IPlayersQuery>();
            var playersService = new PlayersService(
            mockPlayersCommand.Object,
            mockPlayesQuery.Object,
            mapper
            );


            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                playersService,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );


            var playerRequest = new PlayersRequest
            {
                ReservationID = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E"),
                UserID = 2,
            };

            var playerResponse = new PlayersResponse
            {
                Id = 2,
                Name = "Juan",
                Email = "Juan@gmail.com"
            };

            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = playerRequest.ReservationID,
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
                                ReservationID = playerRequest.ReservationID,
                                UserID = 1,
                            },
                            new Players { ReservationID = playerRequest.ReservationID, UserID = 2, }
                        }
                    }

                );


            var token = "test-token";
            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(),
                token
            )).ReturnsAsync(playerResponse);




            // Act

            // Assert
            await Assert.ThrowsAsync<PlayerAlreadyExistsException>(async () => await reservationService.AddPlayer(playerRequest));

        }


        [Fact]
        public async Task AddPlayer_FullPlayersListExceptionn()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();


            var mockPlayesQuery = new Mock<IPlayersQuery>();
            var playersService = new PlayersService(
            mockPlayersCommand.Object,
            mockPlayesQuery.Object,
            mapper
            );


            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                playersService,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );


            var playerRequest = new PlayersRequest
            {
                ReservationID = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E"),
                UserID = 3,
            };

            var playerResponse = new PlayersResponse
            {
                Id = 3,
                Name = "Marcos",
                Email = "Marcos@gmail.com"
            };

            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = playerRequest.ReservationID,
                        FieldID = new Guid("11111111-9C67-48B9-D011-08DD124C3A2E"),
                        OwnerUserID = 1,
                        ReservationStatusID = 1,
                        Date = new DateOnly(2025, 1, 14),
                        StartTime = TimeOnly.Parse("10:00"),
                        EndTime = TimeOnly.Parse("11:00"),
                        MaxJugadores = 2,
                        StatusNavigator = new ReservationStatus
                        {
                            Id = 1,
                            Status = "Reserved"
                        },
                        Players = new List<Players>
                        {
                            new Players
                            {
                                ReservationID = playerRequest.ReservationID,
                                UserID = 1,
                            },
                            new Players { ReservationID = playerRequest.ReservationID, UserID = 2, }
                        }
                    }

                );


            var token = "test-token";
            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(),
                token
            )).ReturnsAsync(playerResponse);




            // Act

            // Assert
            await Assert.ThrowsAsync<FullPlayersListException>(async () => await reservationService.AddPlayer(playerRequest));

        }



        [Fact]
        public async Task AddPlayer_InvalidPlayerException()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var MockEncryptionService = new Mock<IEncryptionService>();
            var mockReservationCommand = new Mock<IReservationCommand>();
            var mockReservationQuery = new Mock<IReservationQuery>();
            var mockPlayersCommand = new Mock<IPlayersCommand>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();


            var mockPlayesQuery = new Mock<IPlayersQuery>();
            var playersService = new PlayersService(
            mockPlayersCommand.Object,
            mockPlayesQuery.Object,
            mapper
            );


            var reservationService = new ReservationService(
                mockReservationCommand.Object,
                mockReservationQuery.Object,
                MockEncryptionService.Object,
                mapper,
                playersService,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                mockPlayersCommand.Object,
                mockContextAccessor.Object
            );


            var playerRequest = new PlayersRequest
            {
                ReservationID = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E"),
                UserID = 3,
            };

            var playerResponse = new PlayersResponse
            {
                Id = 3,
                Name = "Marcos",
                Email = "Marcos@gmail.com"
            };

            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = playerRequest.ReservationID,
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
                                ReservationID = playerRequest.ReservationID,
                                UserID = 1,
                            },
                            new Players { ReservationID = playerRequest.ReservationID, UserID = 2, }
                        }
                    }

                );


            var token = "test-token";
            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(),
                token
            )).ReturnsAsync((PlayersResponse)null);




            // Act

            // Assert
            await Assert.ThrowsAsync<InvalidPlayerException>(async () => await reservationService.AddPlayer(playerRequest));

        }
    }
}
