using System;
using System.Collections.Generic;
using System.Linq;
using KochWermann.SKS.Package.DataAccess.Entities;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.DataAccess.Tests
{
    public class ParcelRepositoryTests
    {
        private IParcelRepository _parcelRepository;
        private List<Parcel> _parcels;

        [SetUp]
        public void Setup()
        {
            _parcels = new List<Parcel>{new Parcel{
                Id = 1,
                HopArrivals = new List<HopArrival>{new HopArrival{
                    Id = 1,
                    Code = "Code1",
                    Description = "Visited hops blabla",
                    DateTime = DateTime.Today
                }},
                Recipient = new Recipient{
                    Id = 1,
                    Country = "Österreich",
                    PostalCode = "A-1120",
                    Street = "Hauptstraße 12/12/12",
                    City = "Wien",
                    Name = "Josef Koch"
                },
                Sender = new Recipient{
                    Id = 2,
                    Country = "Austria",
                    PostalCode = "A-1210",
                    Street = "Landstraße 27a",
                    City = "Wien",
                    Name = "Josef Wermann"
                },
                State = Parcel.StateEnum.InTransportEnum,
                TrackingId = "PYJRB4HZ6",
                Weight = 6.9f
            }};

            var loggerMock = new Mock<ILogger<SqlParcelRepository>>();

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(p => p.Parcels).Returns(DbContextMock.GetQueryableMockDbSet<Parcel>(_parcels));
            mockContext.Setup(p => p.SaveChanges()).Returns(1);

            _parcelRepository = new SqlParcelRepository(mockContext.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Create()
        {
            var newParcel = new Parcel();

            _parcelRepository.Create(newParcel);

            Assert.AreEqual(2, _parcels.Count);
            Assert.AreEqual(newParcel, _parcels[1]);
        }

        [Test]
        public void Should_Delete()
        {
            _parcelRepository.Delete(1);

            Assert.AreEqual(0, _parcels.Count);
        }

        [Test]
        public void Should_Throw_Not_Found_On_Delete()
        {
            Assert.Throws<DataAccess.Entities.DAL_NotFound_Exception>(() => _parcelRepository.Delete(2));
        }

        [Test]
        public void Should_Get_Parcel_By_Id()
        {
            var parcel = _parcelRepository.GetParcelById(1);

            Assert.AreEqual(_parcels[0], parcel);
        }

        [Test]
        public void Should_Throw_Not_Found_On_Get_Parcel_By_Id()
        {
            Assert.Throws<DataAccess.Entities.DAL_NotFound_Exception>(() => _parcelRepository.GetParcelById(2));
        }

        [Test]
        public void Should_Get_Parcel_By_Recipient()
        {
            var recipient = new Recipient
            {
                Id = 1,
                Country = "Österreich",
                PostalCode = "A-1120",
                Street = "Hauptstraße 12/12/12",
                City = "Wien",
                Name = "Josef Koch"
            };

            var parcels = _parcelRepository.GetParcelByRecipient(recipient);

            Assert.AreEqual(1, parcels.Count());
        }

        [Test]
        public void Should_Not_Get_Parcel_By_Recipient()
        {
            var parcels = _parcelRepository.GetParcelByRecipient(new Recipient());

            Assert.AreEqual(0, parcels.Count());
        }

        [Test]
        public void Should_Get_Parcel_By_TrackingId()
        {
            var parcel = _parcelRepository.GetParcelByTrackingId("PYJRB4HZ6");

            Assert.AreEqual(_parcels[0], parcel);
        }

        [Test]
        public void Should_Throw_Not_Found_On_Get_Parcel_By_TrackingId()
        {
            Assert.Throws<DataAccess.Entities.DAL_NotFound_Exception>(() => _parcelRepository.GetParcelByTrackingId(""));
        }

        [Test]
        public void Should_Get_All_Parcels()
        {
            var parcels = _parcelRepository.GetAllParcels();

            Assert.AreEqual(_parcels, parcels.ToList());
        }
    }
}