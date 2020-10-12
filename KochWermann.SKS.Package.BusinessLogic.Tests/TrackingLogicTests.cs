using System;
using System.Collections.Generic;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using NUnit.Framework;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class TrackingLogicTests
    {
        private ITrackingLogic _trackingLogic = new TrackingLogic();
        private Parcel _validParcel;
        private string _validTrackingId = "PYJRB4HZ6";
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
                    DateTime = DateTime.Today
                }},
                FutureHops = new List<HopArrival>{new HopArrival{
                    Code = "Code2",
                    Description = "Future hops blabla",
                    DateTime = DateTime.Now
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
        }

        [Test]
        public void Should_Transistion_Valid_Parcel()
        {
            var res = _trackingLogic.TransitionParcel(_validParcel, _validParcel.TrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Of_Parcel_Of_Equal_Sender_Recipient()
        {
            _validParcel.Recipient = _validParcel.Sender;
            Assert.Throws<ValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _validParcel.TrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Of_Parcel_Of_Invalid_PostalCode_Format()
        {
            _validParcel.Recipient.PostalCode = "12345";
            Assert.Throws<ValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _validParcel.TrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Of_Parcel_Of_Invalid_Street_Format()
        {
            _validParcel.Sender.Street = "street";
            Assert.Throws<ValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _validParcel.TrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Of_Parcel_Of_Invalid_TrackingId()
        {
           _validParcel.TrackingId = _invalidTrackingId;
            Assert.Throws<ArgumentException>(() => _trackingLogic.TransitionParcel(_validParcel, _validParcel.TrackingId));
        }

        [Test]
        public void Should_Track_Parcel()
        {
            var res = _trackingLogic.TrackParcel(_validTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Throw_Exception_On_Null_TrackingId_On_Track_Parcel()
        {
            Assert.Throws<ArgumentNullException>(() => _trackingLogic.TrackParcel(null));
        }

        [Test]
        public void Should_Submit_Parcel()
        {
            var res = _trackingLogic.SubmitParcel(_validParcel);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Throw_Exception_On_Submit_Of_Invalid_Parcel_Weight()
        {
            _validParcel.Weight = 0;
            Assert.Throws<ValidationException>(() => _trackingLogic.SubmitParcel(_validParcel));
        }

        [Test]
        public void Should_Report_Parcel_Delivery()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelDelivery(_validTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Report_Parcel_Delivery_Of_Invalid_TrackingId()
        {
            Assert.Throws<ArgumentException>(() => _trackingLogic.ReportParcelDelivery(_invalidTrackingId));
        }

        [Test]
        public void Should_Report_Parcel_Hop()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelHop(_validTrackingId, _validCode));
        }

        [Test]
        public void Should_Throw_Exception_On_Report_Parcel_Hop_Of_Invalid_Code()
        {
            Assert.Throws<ArgumentException>(() => _trackingLogic.ReportParcelHop(_validTrackingId, _invalidCode));
        }
        
        [Test]
        public void Should_Throw_Exception_On_Report_Parcel_Hop_Of_Invalid_TrackingId()
        {
            Assert.Throws<ArgumentException>(() => _trackingLogic.ReportParcelHop(_invalidTrackingId, _validCode));
        }
    }
}