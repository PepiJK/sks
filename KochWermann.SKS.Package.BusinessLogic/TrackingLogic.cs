using System.Linq;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using System;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId)) throw new NullReferenceException();

            IValidator<Parcel> validator = new ParcelValidator();
            var validationResult = validator.Validate(parcel);

            if(validationResult.IsValid)
            {
                return new Parcel();
            }
            else
            {
                //Unit test sends empty parcel =>
                throw new FluentValidation.ValidationException("parcel " + validationResult.Errors.ToString()); 
            }
        }

        public Parcel TrackParcel(string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId)) throw new NullReferenceException();
            return new Parcel();
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            IValidator<Parcel> validator = new ParcelValidator();
            var validationResult = validator.Validate(parcel);

            if(validationResult.IsValid)
            {
                return new Parcel();
            }
            else
            {
                //Unit test sends empty parcel =>
                throw new FluentValidation.ValidationException("parcel " + validationResult.Errors.ToString()); 
            }
        }

        public void ReportParcelDelivery(string trackingId)
        {

        }

        public void ReportParcelHop(string trackingId, string code)
        {

        }
    }
}