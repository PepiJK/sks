using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using NUnit.Framework;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Mapper;
using Microsoft.Extensions.Logging;

using BLException = KochWermann.SKS.Package.BusinessLogic.Entities.BLException;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class TrackingLogicTests
    {
        private ITrackingLogic _trackingLogic;
        private Parcel _validParcel;
        private string _validTrackingId = "PYJRB4HZ6";
        private string _notFoundTrackingId = "PYJRB4HZ9";
        private string _invalidTrackingId = "hallo";
        private string _validCode = "TEST1234";
        private string _invalidCode = "hi";

        [SetUp]
        public void Setup()
        {      
            _validParcel = new Parcel{
                VisitedHops = new List<HopArrival>{new HopArrival{
                    Code = "Code1",
                    Description = "Visited hops blabla",
                    DateTime = DateTime.Now.AddDays(-1)
                }},
                FutureHops = new List<HopArrival>{new HopArrival{
                    Code = "Code2",
                    Description = "Future hops blabla",
                    DateTime = DateTime.Now.AddDays(1)
                }},
                Recipient = new Recipient{
                    Country = "Österreich",
                    PostalCode = "A-1120",
                    Street = "Hauptstraße 12/12/12",
                    City = "Wien",
                    Name = "Josef Koch"
                },
                Sender = new Recipient{
                    Country = "Austria",
                    PostalCode = "A-1210",
                    Street = "Landstraße 27a",
                    City = "Wien",
                    Name = "Josef Wermann"
                },
                State = Parcel.StateEnum.InTransportEnum,
                TrackingId = _validTrackingId,
                Weight = 6.9f
            };

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DalMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //mock parcel repository
            var mock = new Mock<IParcelRepository>();
            mock.Setup(parcelRepository => parcelRepository.GetParcelByTrackingId(
                _validTrackingId
            )).Returns(new DataAccess.Entities.Parcel());
            mock.Setup(parcelRepository => parcelRepository.GetParcelByTrackingId(
                _notFoundTrackingId
            )).Throws(new DataAccess.Entities.DALNotFoundException("TrackingId Not Found", new Exception()));
            mock.Setup(parcelRepository => parcelRepository.GetParcelByTrackingId(
                _invalidTrackingId
            )).Throws(new DataAccess.Entities.DALException("Invalid TrackingId", new Exception()));

            mock.Setup(parcelRepository => parcelRepository.Create(
                It.IsAny<DataAccess.Entities.Parcel>()
            )).Returns<DataAccess.Entities.Parcel>(p => p.Id);

            mock.Setup(parcelRepository => parcelRepository.Update(
                It.IsAny<DataAccess.Entities.Parcel>()
            ));
            mock.Setup(parcelRepository => parcelRepository.Update(
                It.Is<DataAccess.Entities.Parcel>(p => p.TrackingId == _notFoundTrackingId)
            )).Throws(new DataAccess.Entities.DALNotFoundException("Parcel Not Found", new Exception()));
            
            var loggerMock = new Mock<ILogger<TrackingLogic>>();

            _trackingLogic = new TrackingLogic(mapper, mock.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Transistion_Valid_Parcel()
        {
            var res = _trackingLogic.TransitionParcel(_validParcel, _validParcel.TrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Equal_Sender_Recipient()
        {
            _validParcel.Recipient = _validParcel.Sender;
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _validTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Invalid_PostalCode_Format()
        {
            _validParcel.Recipient.PostalCode = "12345";
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _validTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Invalid_Street_Format()
        {
            _validParcel.Sender.Street = "street";
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(_validParcel,_validTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Invalid_TrackingId()
        {
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _invalidTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Null_TrackingId()
        {
            Assert.Throws<BLException>(() => _trackingLogic.TransitionParcel(_validParcel, null));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Null_Parcel()
        {
            Assert.Throws<BLException>(() => _trackingLogic.TransitionParcel(null, _validTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Invalid_Parcel_Weight()
        {
            _validParcel.Weight = 0;
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _validTrackingId));
        }

        [Test]
        public void Should_Track_Parcel()
        {
            var res = _trackingLogic.TrackParcel(_validTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_On_Track_Parcel()
        {
            Assert.Throws<BLNotFoundException>(() => _trackingLogic.TrackParcel(_notFoundTrackingId));
        }

        [Test]
        public void Should_Submit_Parcel()
        {
            var res = _trackingLogic.SubmitParcel(_validParcel);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Report_Parcel_Delivery()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelDelivery(_validTrackingId));
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_On_Report_Parcel_Delivery()
        {
            Assert.Throws<BLNotFoundException>(() => _trackingLogic.ReportParcelDelivery(_notFoundTrackingId));
        }

        [Test]
        public void Should_Report_Parcel_Hop()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelHop(_validTrackingId, _validCode));
        }
        [Test]
        public void Should_Throw_Exception_On_Report_Parcel_Hop_Of_Invalid_Code()
        {
            Assert.Throws<BLValidationException>(() => _trackingLogic.ReportParcelHop(_validTrackingId, _invalidCode));
        }

        [Test]
        public void Should_Throw_Exception_On_Report_Parcel_Hop_Of_Null_Code()
        {
            Assert.Throws<BLException>(() => _trackingLogic.ReportParcelHop(_validTrackingId, null));
        }
    }
}