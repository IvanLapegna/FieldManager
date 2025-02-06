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
    public class GenerateReservation_UnitTesting
    {


        [Fact]
        public async Task GenerateReservation_ShouldReturnReservationResponse()
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

            var expectedReservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");

            var token = "test-token";
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

            var request = new ReservationCreateRequest
            {
                FieldID = fieldResponse.Id,
                UserID = 1,
                Day = 13,
                Month = 1,
                Year = 2025,
                StartHour = 10,
                EndHour = 11,
                MaxJugadores = 22
            };

            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            

            // Simular la respuesta del microservicio field
            mockHttpService.Setup(x => x.GetAsync<FieldResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(fieldResponse);


            



            //mockeo mapper
            //mockMapper.Setup(x => x.Map<Reservation>(It.IsAny<ReservationCreateRequest>()))
            //.Returns(new Reservation
            //{
            //    ReservationID = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E"), // Asegúrate de incluir un ID válido
            //    ReservationStatusID = 0,       // Estado inicial antes de asignarlo a 1
            //    // Puedes agregar otras propiedades según sea necesario
            //});


            mockHttpService.Setup(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(playerResponse);

            mockReservationCommand.Setup(x => x.InsertReservation(It.IsAny<Reservation>()))
            .Callback<Reservation>(reservation =>
            {
            reservation.ReservationID = expectedReservationId; // Asignar un GUID nuevo
            });
            //suponiendo que el mapeo funcione normalmente esto esta comentado
            //mockMapper.Setup(x => x.Map<ReservationResponse>(It.IsAny<Reservation>()))
            //.Returns(new ReservationResponse
            //{
            //ReservationID = Guid.NewGuid(),
            //Players = new List<PlayersResponse>
            //{
            //    new PlayersResponse { Id = 1, ReservationID = Guid.NewGuid() }
            //},
            //Field = new FieldResponse
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Test Field"
            //},
            //AddPlayerLink = "https://test.invitation.link",
            //Status = new ReservationStatusResponse
            //{
            //    Id = 1,
            //    Status = "Confirmed"
            //},
            //StartTime = TimeOnly.Parse("8"),
            //EndTime = TimeOnly.Parse("10"),
            //OwnerUserID = 1
            //});




            //mockeo query de ReservationExists
            mockReservationQuery.Setup(x => x.ReservationExists(It.IsAny<Guid>())).ReturnsAsync(true);


            //mockeo query de GetReservationById
            mockReservationQuery.Setup(x => x.GetReservationById(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new Reservation
                    {
                        ReservationID = expectedReservationId,
                        FieldID = fieldResponse.Id,
                        OwnerUserID = request.UserID,
                        ReservationStatusID = 1,
                        Date = new DateOnly(2025, 1, 13),
                        StartTime = TimeOnly.Parse("10:00"),
                        EndTime = TimeOnly.Parse("11:00"),
                        MaxJugadores = request.MaxJugadores,
                        StatusNavigator = new ReservationStatus
                        {
                            Id = 1,
                            Status = "Reserved"
                        },
                        Players = new List<Players>
                        {
                            new Players
                            {
                                ReservationID = expectedReservationId,
                                UserID = request.UserID,
                            }
                        }
                    }

                );

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
            var result = await reservationService.GenerateReservation(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReservationId, result.ReservationID);
            Assert.Equal(request.UserID, result.OwnerUserID);
            
            Assert.Equal(fieldResponse.Id, result.Field.Id);
            Assert.Equal(fieldResponse.Name, result.Field.Name);
            Assert.Equal(fieldResponse.Size, result.Field.Size);
            Assert.Equal(fieldResponse.FieldType.Id, result.Field.FieldType.Id);
            Assert.Equal(fieldResponse.FieldType.Description, result.Field.FieldType.Description);

            Assert.Equal(1, result.Status.Id);
            Assert.Equal("Reserved", result.Status.Status);

            Assert.Equal(DateOnly.Parse("13/01/2025"), result.Date);
            Assert.Equal(TimeOnly.Parse("10:00"), result.StartTime);
            Assert.Equal(TimeOnly.Parse("11:00"), result.EndTime);


            var playerResult = Assert.Single(result.Players);
            Assert.Equal(playerResponse.Id, playerResult.Id);
            Assert.Equal(playerResponse.Name, playerResult.Name);
            Assert.Equal(playerResponse.Email, playerResult.Email);

            //mockHttpService.Verify(x => x.GetAsync<FieldResponse>(
            //    It.Is<string>(url => url.Contains($"Field/{request.FieldID}")), // Asegúrate de que la URL es correcta
            //    token
            //), Times.Once);
        }







        [Fact]
        public async Task GenerateReservation_NoDayAvailabilityException()
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

            var expectedReservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");

            var token = "test-token";
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
                        CloseHour = TimeSpan.Parse("22:00") 
                    }
                },
                FieldType = new FieldTypeResponse
                {
                    Id = 1,
                    Description = "cesped"
                }

            };


            var request = new ReservationCreateRequest
            {
                FieldID = fieldResponse.Id,
                UserID = 1,
                Day = 14,
                Month = 1,
                Year = 2025,
                StartHour = 10,
                EndHour = 11,
                MaxJugadores = 22
            };

            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);



            // Simular la respuesta del microservicio field
            mockHttpService.Setup(x => x.GetAsync<FieldResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(fieldResponse);





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

            // Assert
            await Assert.ThrowsAsync<NoDayAvailabilityException>(
            async () => await reservationService.GenerateReservation(request)
            );

        }


        [Fact]
        public async Task GenerateReservation_OutOfTimeRangeException()
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

            var expectedReservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");

            var token = "test-token";
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
                        CloseHour = TimeSpan.Parse("22:00")
                    }
                },
                FieldType = new FieldTypeResponse
                {
                    Id = 1,
                    Description = "cesped"
                }

            };


            var request = new ReservationCreateRequest
            {
                FieldID = fieldResponse.Id,
                UserID = 1,
                Day = 13,
                Month = 1,
                Year = 2025,
                StartHour = 22,
                EndHour = 23,
                MaxJugadores = 22
            };

            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);



            // Simular la respuesta del microservicio field
            mockHttpService.Setup(x => x.GetAsync<FieldResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(fieldResponse);





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

            // Assert
            await Assert.ThrowsAsync<OutOfTimeRangeException>(
            async () => await reservationService.GenerateReservation(request)
            );

        }



        [Fact]
        public async Task GenerateReservation_TimeSlotUnavailableException()
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

            var expectedReservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");

            var token = "test-token";
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
                        CloseHour = TimeSpan.Parse("22:00")
                    }
                },
                FieldType = new FieldTypeResponse
                {
                    Id = 1,
                    Description = "cesped"
                }

            };


            var request = new ReservationCreateRequest
            {
                FieldID = fieldResponse.Id,
                UserID = 1,
                Day = 13,
                Month = 1,
                Year = 2025,
                StartHour = 8,
                EndHour = 9,
                MaxJugadores = 22
            };

            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);



            // Simular la respuesta del microservicio field
            mockHttpService.Setup(x => x.GetAsync<FieldResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(fieldResponse);


            mockReservationQuery.Setup(x => x.AlreadyExists(It.IsAny<ReservationCreateRequest>())).ReturnsAsync(true);



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

            // Assert
            await Assert.ThrowsAsync<TimeSlotUnavailableException>(
            async () => await reservationService.GenerateReservation(request)
            );

        }


    }




}
