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
    public class GetReservationsPlayer_UnitTesting
    {

        [Fact]
        public async Task GetReservationsPlayer_ShouldReturnReservationList()
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

            var expectedReservationId = new Guid("49560DEF-9C67-48B9-D011-08DD124C3A2E");

            var token = "test-token";


            var fieldResponse1 = new FieldResponse
            {
                Id = new Guid("11111111-9C67-48B9-D011-08DD124C3A2E"),
                Name = "Test Field 1",
                Size = "11",
                Availabilities = new List<AvailabilityResponse>
                {
                    new AvailabilityResponse
                    {
                        Id = 1,
                        Day = "Monday",
                        OpenHour = TimeSpan.Parse("08:00"),
                        CloseHour = TimeSpan.Parse("22:00") // Convertir string a TimeSpan
                    },

                    new AvailabilityResponse
                    {
                        Id = 2,
                        Day = "Tuesday",
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

            var fieldResponse2 = new FieldResponse
            {
                Id = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                Name = "Test Field 2",
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




            var playerResponse1 = new PlayersResponse
            {
                Id = 1,
                Name = "Ivan",
                Email = "ivan@gmail.com"

            };

            var playerResponse2 = new PlayersResponse
            {
                Id = 2,
                Name = "Marcos",
                Email = "Marcos@gmail.com"

            };

            var reservation1 = new Reservation
            {
                ReservationID = new Guid("12345678-9C67-48B9-D011-08DD124C3A2E"),
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
                        ReservationID = new Guid("12345678-9C67-48B9-D011-08DD124C3A2E"),
                        UserID = 1,
                        },
                        new Players { ReservationID = new Guid("12345678-9C67-48B9-D011-08DD124C3A2E"), UserID = 2, }
                }

            };


            var reservation2 = new Reservation
            {
                ReservationID = new Guid("87654321-9C67-48B9-D011-08DD124C3A2E"),
                FieldID = new Guid("99999999-9C67-48B9-D011-08DD124C3A2E"),
                OwnerUserID = 2,
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
                    new Players { ReservationID = new Guid("87654321-9C67-48B9-D011-08DD124C3A2E"), UserID = 2 },

                    new Players
                    {
                        ReservationID = new Guid("87654321-9C67-48B9-D011-08DD124C3A2E"),
                        UserID = 1,
                    }
                }

            };

            mockReservationQuery.Setup(x => x.GetReservationsPlayer(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>())).ReturnsAsync(new List<Reservation> { reservation1, reservation2 });


            var headers = new HeaderDictionary { { "Authorization", $"Bearer {token}" } };
            var mockHttpContext = new DefaultHttpContext();

            // Agregar el encabezado Authorization directamente
            mockHttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Configurar el mock del HttpContextAccessor
            mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext); mockContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);



            mockHttpService.SetupSequence(x => x.GetAsync<PlayersResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(playerResponse1).ReturnsAsync(playerResponse2)
            .ReturnsAsync(playerResponse2).ReturnsAsync(playerResponse1);

            // Simular la respuesta del microservicio field
            mockHttpService.SetupSequence(x => x.GetAsync<FieldResponse>(
                It.IsAny<string>(), // Puedes especificar la URL exacta si es necesario
                token
            )).ReturnsAsync(fieldResponse1).ReturnsAsync(fieldResponse2);


            MockEncryptionService.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("encriptado");





            var expectedReservation1 = new ReservationResponse
            {
                ReservationID = reservation1.ReservationID,
                OwnerUserID = reservation1.OwnerUserID,
                Field = fieldResponse1,

                Status = new ReservationStatusResponse
                {
                    Id = 1,
                    Status = "Reserved"
                },

                Date = reservation1.Date,
                StartTime = reservation1.StartTime,
                EndTime = reservation1.EndTime,

                AddPlayerLink = ":///reservation/player-invitation?encryptedId=NDk1NjBkZWYtOWM2Ny00OGI5LWQwMTEtMDhkZDEyNGMzYTJl",

                Players = new List<PlayersResponse>
                {
                    playerResponse1, playerResponse2
                }
            };

            var expectedReservation2 = new ReservationResponse
            {
                ReservationID = reservation2.ReservationID,
                OwnerUserID = reservation2.OwnerUserID,
                Field = fieldResponse2,

                Status = new ReservationStatusResponse
                {
                    Id = 1,
                    Status = "Reserved"
                },

                Date = reservation2.Date,
                StartTime = reservation2.StartTime,
                EndTime = reservation2.EndTime,

                AddPlayerLink = ":///reservation/player-invitation?encryptedId=NDk1NjBkZWYtOWM2Ny00OGI5LWQwMTEtMDhkZDEyNGMzYTJl",

                Players = new List<PlayersResponse>
                {
                    playerResponse2, playerResponse1
                }
            };

            var expectedResult = new List<ReservationResponse> { expectedReservation1, expectedReservation2 };
            var fields = new List<FieldResponse> { fieldResponse1, fieldResponse2 };

            // Act
            //var result = await reservationService.GetAll(null, null, null, null, "2024-01-13");
            var result = await reservationService.GetReservationsPlayer(null, null, 1);

            // Assert
            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedResult[i].ReservationID, result[i].ReservationID);
                Assert.Equal(expectedResult[i].OwnerUserID, result[i].OwnerUserID);

                Assert.Equal(fields[i].Id, result[i].Field.Id);
                Assert.Equal(fields[i].Name, result[i].Field.Name);
                Assert.Equal(fields[i].Size, result[i].Field.Size);
                Assert.Equal(fields[i].FieldType.Id, result[i].Field.FieldType.Id);
                Assert.Equal(fields[i].FieldType.Description, result[i].Field.FieldType.Description);

                Assert.Equal(expectedResult[i].Status.Id, result[i].Status.Id);
                Assert.Equal(expectedResult[i].Status.Status, result[i].Status.Status);

                Assert.Equal(expectedResult[i].Date, result[i].Date);
                Assert.Equal(expectedResult[i].StartTime, result[i].StartTime);
                Assert.Equal(expectedResult[i].EndTime, result[i].EndTime);


                for (var j = 0; j < result[i].Players.Count; j++)
                {
                    var expectedPlayer = expectedResult[i].Players.ToList()[j];
                    var actualPlayer = result[i].Players.ToList()[j];

                    Assert.Equal(expectedPlayer.Id, actualPlayer.Id);
                    Assert.Equal(expectedPlayer.Name, actualPlayer.Name);
                    Assert.Equal(expectedPlayer.Email, actualPlayer.Email);
                }


            }

        }
    }
}
