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
    public class UpdateReservation_UnitTesting
    {
        [Fact]
        public async Task UpdateReservation_ShouldReturnReservationResponse()
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

            var reservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");

            var token = "test-token";
            var fieldResponse = new FieldResponse
            {
                Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                Name = "Test Field",
                Size = "11",
                Availabilities = new List<AvailabilityResponse>
                {
                    new AvailabilityResponse
                    {
                        Id = 1,
                        Day = "Monday",
                        OpenHour = TimeSpan.Parse("08:00"),
                        CloseHour = TimeSpan.Parse("22:00") // Convertir string a TimeSpan
                    }
                },
                FieldType = new FieldTypeResponse
                {
                    Id = 1,
                    Description = "cesped"
                }

            };

            var playerResponse = new PlayersResponse
            {
                Id = 1,
                Name = "Ivan",
                Email = "ivan@gmail.com"

            };

            var request = new ReservationUpdateRequest
            {
                FieldID = fieldResponse.Id, //cambio
                Day = 20, //cambio
                Month = 1,
                Year = 2025,
                StartHour = 12, //cambio
                EndHour = 13, //cambio
                MaxJugadores = 22 //cambio
            };

            var expectedResult = new ReservationResponse
            {
                ReservationID = reservationId,
                OwnerUserID = 1,
                Field = new FieldResponse
                {
                    Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                    Name = "Test Field",
                    Size = "11",
                    Availabilities = new List<AvailabilityResponse>
                    {
                        new AvailabilityResponse
                        {
                           Id = 1,
                           Day = "Monday",
                           OpenHour = TimeSpan.Parse("08:00"),
                           CloseHour = TimeSpan.Parse("22:00") // Convertir string a TimeSpan
                        }
                    },
                    FieldType = new FieldTypeResponse
                    {
                        Id = 1,
                        Description = "cesped"
                    }
                },

                Status = new ReservationStatusResponse
                {
                    Id = 1,
                    Status = "Reserved"
                },

                Date = new DateOnly(2025, 1, 20),
                StartTime = TimeOnly.Parse("12:00"),
                EndTime = TimeOnly.Parse("13:00"),

                AddPlayerLink = ":///reservation/player-invitation?encryptedId=encriptado",

                Players = new List<PlayersResponse>
                {
                    playerResponse
                }
            };


            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = reservationId,
                        FieldID = new Guid("11111111-9C67-48B9-D011-08DD124C3A2E"),
                        OwnerUserID = playerResponse.Id,
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
                                UserID = playerResponse.Id,
                            }
                        }
                    }

                );



            // Simular la respuesta del microservicio field
            mockHttpService.Setup(x => x.GetAsync<FieldResponse>(
                It.IsAny<string>(), 
                token
            )).ReturnsAsync(fieldResponse);



            mockReservationCommand.Setup(x => x.UpdateReservation(It.IsAny<Reservation>()))
            .Callback<Reservation>(reservation =>
            {
                reservation.FieldID = request.FieldID;
                reservation.Date =  new DateOnly(request.Year, request.Month, request.Day);
                reservation.StartTime = new TimeOnly(request.StartHour, 0);
                reservation.EndTime = new TimeOnly(request.EndHour, 0);
                reservation.MaxJugadores = request.MaxJugadores;

            });


            mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(playerResponse);

            
            



            //mockeo query de ReservationExists


            //mockeo query de GetReservationById
            

            MockEncryptionService.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("encriptado");


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



            // Act
            var result = await reservationService.UpdateReservation(request, reservationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.ReservationID, result.ReservationID);
            Assert.Equal(expectedResult.OwnerUserID, result.OwnerUserID);

            Assert.Equal(expectedResult.Field.Id, result.Field.Id);
            Assert.Equal(expectedResult.Field.Name, result.Field.Name);
            Assert.Equal(expectedResult.Field.Size, result.Field.Size);
            Assert.Equal(expectedResult.Field.FieldType.Id, result.Field.FieldType.Id);
            Assert.Equal(expectedResult.Field.FieldType.Description, result.Field.FieldType.Description);

            Assert.Equal(expectedResult.Status.Id, result.Status.Id);
            Assert.Equal(expectedResult.Status.Status, result.Status.Status);

            Assert.Equal(expectedResult.Date, result.Date);
            Assert.Equal(expectedResult.StartTime, result.StartTime);
            Assert.Equal(expectedResult.EndTime, result.EndTime);


            var playerResult = Assert.Single(result.Players);
            Assert.Equal(playerResponse.Id, playerResult.Id);
            Assert.Equal(playerResponse.Name, playerResult.Name);
            Assert.Equal(playerResponse.Email, playerResult.Email);

            
        }

        [Fact]
        public async Task UpdateReservation_ShouldReturnInvalidReservationException()
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

            var token = "test-token";
            var fieldResponse = new FieldResponse
            {
                Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                Name = "Test Field",
                Size = "11",
                Availabilities = new List<AvailabilityResponse>
                {
                    new AvailabilityResponse
                    {
                        Id = 1,
                        Day = "Monday",
                        OpenHour = TimeSpan.Parse("08:00"),
                        CloseHour = TimeSpan.Parse("22:00") // Convertir string a TimeSpan
                    }
                },
                FieldType = new FieldTypeResponse
                {
                    Id = 1,
                    Description = "cesped"
                }

            };

            var playerResponse = new PlayersResponse
            {
                Id = 1,
                Name = "Ivan",
                Email = "ivan@gmail.com"

            };

            var request = new ReservationUpdateRequest
            {
                FieldID = fieldResponse.Id, //cambio
                Day = 20, //cambio
                Month = 1,
                Year = 2025,
                StartHour = 12, //cambio
                EndHour = 13, //cambio
                MaxJugadores = 22 //cambio
            };

            var expectedResult = new ReservationResponse
            {
                ReservationID = reservationId,
                OwnerUserID = 1,
                Field = new FieldResponse
                {
                    Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                    Name = "Test Field",
                    Size = "11",
                    Availabilities = new List<AvailabilityResponse>
                    {
                        new AvailabilityResponse
                        {
                           Id = 1,
                           Day = "Monday",
                           OpenHour = TimeSpan.Parse("08:00"),
                           CloseHour = TimeSpan.Parse("22:00") // Convertir string a TimeSpan
                        }
                    },
                    FieldType = new FieldTypeResponse
                    {
                        Id = 1,
                        Description = "cesped"
                    }
                },

                Status = new ReservationStatusResponse
                {
                    Id = 1,
                    Status = "Reserved"
                },

                Date = new DateOnly(2025, 1, 20),
                StartTime = TimeOnly.Parse("12:00"),
                EndTime = TimeOnly.Parse("13:00"),

                AddPlayerLink = ":///reservation/player-invitation?encryptedId=encriptado",

                Players = new List<PlayersResponse>
                {
                    playerResponse
                }
            };


            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            //mockeo query ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = reservationId,
                        FieldID = new Guid("11111111-9C67-48B9-D011-08DD124C3A2E"),
                        OwnerUserID = playerResponse.Id,
                        ReservationStatusID = 3,
                        Date = new DateOnly(2025, 1, 14),
                        StartTime = TimeOnly.Parse("10:00"),
                        EndTime = TimeOnly.Parse("11:00"),
                        MaxJugadores = 14,
                        StatusNavigator = new ReservationStatus
                        {
                            Id = 3,
                            Status = "Cancelled"
                        },
                        Players = new List<Players>
                        {
                            new Players
                            {
                                ReservationID = reservationId,
                                UserID = playerResponse.Id,
                            }
                        }
                    }

                );






            // Act

            // Assert
            await Assert.ThrowsAsync<InvalidReservationException>(
            async () => await reservationService.UpdateReservation(request,reservationId)
            );


        }
    }
}
