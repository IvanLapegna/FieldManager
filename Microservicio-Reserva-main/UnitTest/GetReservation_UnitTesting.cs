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
    public class GetReservation_UnitTesting
    {

        //[Fact]
        //public async Task GetReservationById_ShouldReturnReservationResponse()
        //{
        //    // Arrange
        //    var mockReservationQuery = new Mock<IReservationQuery>();
        //    var MockEncryptionService = new Mock<IEncryptionService>();

        //    var mappingConfig = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile(new AutoMapperConfiguration());
        //    });
        //    var mapper = mappingConfig.CreateMapper();

        //    var mockHttpService = new Mock<IHttpService>();
        //    var mockContextAccessor = new Mock<IHttpContextAccessor>();



        //    var reservationService = new ReservationService(
        //        null,
        //        mockReservationQuery.Object,
        //        MockEncryptionService.Object,
        //        mapper,
        //        null,// Dependencias adicionales si son necesarias
        //        mockHttpService.Object,
        //        null,
        //        mockContextAccessor.Object
        //    );

        //    var IdSearch = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");


        //    var fieldResponse = new FieldResponse
        //    {
        //        Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
        //        Name = "Test Field",
        //        Size = "7",
        //        Availabilities = new List<AvailabilityResponse>
        //        {
        //            new AvailabilityResponse
        //            {
        //                Id = 1,
        //                Day = "Monday",
        //                OpenHour = TimeSpan.Parse("08:00"),
        //                CloseHour = TimeSpan.Parse("22:00") // Convertir string a TimeSpan
        //            }
        //        },
        //        FieldType = new FieldTypeResponse
        //        {
        //            Id = 1,
        //            Description = "cesped"
        //        }

        //    };

        //    var playerResponse = new PlayersResponse
        //    {
        //        Id = 1,
        //        Name = "Ivan",
        //        Email = "ivan@gmail.com"

        //    };

        //    var expectedResult = new ReservationResponse
        //    {
        //        ReservationID = IdSearch,
        //        OwnerUserID = 1,
        //        Field = new FieldResponse
        //        {
        //            Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
        //            Name = "Test Field",
        //            Size = "7",
        //            Availabilities = new List<AvailabilityResponse>
        //            {
        //                new AvailabilityResponse
        //                {
        //                   Id = 1,
        //                   Day = "Monday",
        //                   OpenHour = TimeSpan.Parse("08:00"),
        //                   CloseHour = TimeSpan.Parse("22:00") // Convertir string a TimeSpan
        //                }
        //            },
        //            FieldType = new FieldTypeResponse
        //            {
        //                Id = 1,
        //                Description = "cesped"
        //            }
        //        },

        //        Status = new ReservationStatusResponse
        //        {
        //            Id = 1,
        //            Status = "Reserved"
        //        },

        //        Date = new DateOnly(2025, 1, 13),
        //        StartTime = TimeOnly.Parse("10:00"),
        //        EndTime = TimeOnly.Parse("11:00"),

        //        AddPlayerLink = ":///reservation/player-invitation?encryptedId=encriptado",

        //        Players = new List<PlayersResponse>
        //        {
        //            playerResponse
        //        }
        //    };

            

        //    //mockeo query de ReservationExists
        //    mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);

        //    //mockeo query de GetReservationById
        //    mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
        //        .ReturnsAsync(
        //            new Reservation
        //            {
        //                ReservationID = IdSearch,
        //                FieldID = fieldResponse.Id,
        //                OwnerUserID = 1,
        //                ReservationStatusID = 1,
        //                Date = new DateOnly(2025, 1, 13),
        //                StartTime = TimeOnly.Parse("10:00"),
        //                EndTime = TimeOnly.Parse("11:00"),
        //                MaxJugadores = 22,
        //                StatusNavigator = new ReservationStatus
        //                {
        //                    Id = 1,
        //                    Status = "Reserved"
        //                },
        //                Players = new List<Players>
        //                {
        //                    new Players
        //                    {
        //                        ReservationID = IdSearch,
        //                        UserID = playerResponse.Id,
        //                    }
        //                }
        //            }

        //        );

        //    var token = "test-token";
        //    var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
        //    var mockHttpContext = new DefaultHttpContext();

        //    // Agregar el encabezado Authorization directamente
        //    mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

        //    // Configurar el mock del HttpContextAccessor
        //    mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

        //    mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
        //        It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
        //        token
        //    )).ReturnsAsync(playerResponse);

        //    mockHttpService.Setup(x => x.GetAsync<FieldResponse>(
        //        It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
        //        token
        //    )).ReturnsAsync(fieldResponse);


        //    MockEncryptionService.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("encriptado");


        //    // Act
        //    var result = await reservationService.GetReservationById(IdSearch);

        //    // Assert
        //    Assert.Equal(IdSearch, result.ReservationID);
        //    Assert.Equal(expectedResult.OwnerUserID, result.OwnerUserID);

        //    Assert.Equal(expectedResult.Field.Id, result.Field.Id);
        //    Assert.Equal(expectedResult.Field.Name, result.Field.Name);
        //    Assert.Equal(expectedResult.Field.Size, result.Field.Size);
        //    Assert.Equal(expectedResult.Field.FieldType.Id, result.Field.FieldType.Id);
        //    Assert.Equal(expectedResult.Field.FieldType.Description, result.Field.FieldType.Description);

        //    Assert.Equal(expectedResult.Status.Id, result.Status.Id);
        //    Assert.Equal(expectedResult.Status.Status, result.Status.Status);

        //    Assert.Equal(expectedResult.Date, result.Date);
        //    Assert.Equal(expectedResult.StartTime, result.StartTime);
        //    Assert.Equal(expectedResult.EndTime, result.EndTime);

        //    Assert.Equal(expectedResult.AddPlayerLink, result.AddPlayerLink);

        //    var playerResult = Assert.Single(result.Players);
        //    Assert.Equal(playerResponse.Id, playerResult.Id);
        //    Assert.Equal(playerResponse.Name, playerResult.Name);
        //    Assert.Equal(playerResponse.Email, playerResult.Email);
        //}




        [Fact]
        public async Task GetReservationById_ShouldReturnInvalidReservationException()
        {
            // Arrange
            var mockReservationQuery = new Mock<IReservationQuery>();

            var reservationService = new ReservationService(
                null,
                mockReservationQuery.Object,
                null,
                null,
                null,// Dependencias adicionales si son necesarias
                null,
                null,
                null
            );

            var IdSearch = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");


            var fieldResponse = new FieldResponse
            {
                Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                Name = "Test Field",
                Size = "7",
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

            var expectedResult = new ReservationResponse
            {
                ReservationID = IdSearch,
                OwnerUserID = 1,
                Field = new FieldResponse
                {
                    Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                    Name = "Test Field",
                    Size = "7",
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

                Date = new DateOnly(2025, 1, 13),
                StartTime = TimeOnly.Parse("10:00"),
                EndTime = TimeOnly.Parse("11:00"),

                AddPlayerLink = ":///reservation/player-invitation?encryptedId=encriptado",

                Players = new List<PlayersResponse>
                {
                    playerResponse
                }
            };



            //mockeo query de ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(false);

            

            // Act

            // Assert
            await Assert.ThrowsAsync<InvalidReservationException>(
            async () => await reservationService.GetReservationById(IdSearch)
            );
        }






        [Fact]
        public async Task GetReservationById_ShouldReturnReservationResponseEncripted()
        {
            // Arrange
            var mockReservationQuery = new Mock<IReservationQuery>();
            var EncryptionService = new EncryptionService();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();

            var mockHttpService = new Mock<IHttpService>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();



            var reservationService = new ReservationService(
                null,
                mockReservationQuery.Object,
                EncryptionService,
                mapper,
                null,// Dependencias adicionales si son necesarias
                mockHttpService.Object,
                null,
                mockContextAccessor.Object
            );

            var IdSearch = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");


            var fieldResponse = new FieldResponse
            {
                Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                Name = "Test Field",
                Size = "7",
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

            var expectedResult = new ReservationResponse
            {
                ReservationID = IdSearch,
                OwnerUserID = 1,
                Field = new FieldResponse
                {
                    Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                    Name = "Test Field",
                    Size = "7",
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

                Date = new DateOnly(2025, 1, 13),
                StartTime = TimeOnly.Parse("10:00"),
                EndTime = TimeOnly.Parse("11:00"),

                AddPlayerLink = ":///reservation/player-invitation?encryptedId=NDk1NjBkZWYtOWM2Ny00OGI5LWQwMTEtMDhkZDEyNGMzYTJl",

                Players = new List<PlayersResponse>
                {
                    playerResponse
                }
            };



            //mockeo query de ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);

            //mockeo query de GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = IdSearch,
                        FieldID = fieldResponse.Id,
                        OwnerUserID = 1,
                        ReservationStatusID = 1,
                        Date = new DateOnly(2025, 1, 13),
                        StartTime = TimeOnly.Parse("10:00"),
                        EndTime = TimeOnly.Parse("11:00"),
                        MaxJugadores = 22,
                        StatusNavigator = new ReservationStatus
                        {
                            Id = 1,
                            Status = "Reserved"
                        },
                        Players = new List<Players>
                        {
                            new Players
                            {
                                ReservationID = IdSearch,
                                UserID = playerResponse.Id,
                            }
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
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(playerResponse);

            mockHttpService.Setup(x => x.GetAsync<FieldResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(fieldResponse);




            // Act
            var result = await reservationService.GetReservationById(IdSearch);

            // Assert
            Assert.Equal(IdSearch, result.ReservationID);
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

            Assert.Equal(expectedResult.AddPlayerLink, result.AddPlayerLink);

            var playerResult = Assert.Single(result.Players);
            Assert.Equal(playerResponse.Id, playerResult.Id);
            Assert.Equal(playerResponse.Name, playerResult.Name);
            Assert.Equal(playerResponse.Email, playerResult.Email);
        }


    }



}
