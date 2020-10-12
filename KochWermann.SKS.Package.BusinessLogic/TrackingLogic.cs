using System.Linq;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using System;
using System.Text.RegularExpressions;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private string _trackingIdPattern = "^[A-Z0-9]{9}$";
        private string _codePattern = "^[A-Z]{4}\\d{1,4}$";

        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId) || parcel == null)
                throw new ArgumentNullException();
            
            if (!Regex.IsMatch(trackingId, _trackingIdPattern))
                throw new ArgumentException("trackingId does not match pattern.");

            IValidator<Parcel> validator = new ParcelValidator();
            var validationResult = validator.Validate(parcel);

            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors); 
            
            return new Parcel();
        }

        public Parcel TrackParcel(string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId)) throw new ArgumentNullException();
            if (!Regex.IsMatch(trackingId, _trackingIdPattern)) throw new ArgumentException("trackingId does not match pattern.");
            
            return new Parcel();
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            if (parcel == null)
                throw new ArgumentNullException();

            IValidator<Parcel> validator = new ParcelValidator();
            var validationResult = validator.Validate(parcel);

            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors); 
            
            return new Parcel();
        }

        public void ReportParcelDelivery(string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId))
                throw new ArgumentNullException();
            
            if (!Regex.IsMatch(trackingId, _trackingIdPattern))
                throw new ArgumentException("trackingId does not match pattern.");
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            if (string.IsNullOrWhiteSpace(trackingId) || string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException();
            
            if (!Regex.IsMatch(trackingId, _trackingIdPattern))
                throw new ArgumentException("trackingId does not match pattern.");
            
            if (!Regex.IsMatch(code, _codePattern))
                throw new ArgumentException("code does not match pattern.");
        }
    }
}