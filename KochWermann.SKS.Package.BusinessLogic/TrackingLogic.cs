using System.Linq;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using System;
using System.Text.RegularExpressions;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _trackingRepository;
        private string _trackingIdPattern = "^[A-Z0-9]{9}$";
        private string _codePattern = "^[A-Z]{4}\\d{1,4}$";

        public TrackingLogic(IMapper mapper, IParcelRepository trackingRepository)
        {
            _trackingRepository = trackingRepository;
            _mapper = mapper;
        }

        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            ValidateParcel(parcel);
            ValidateTrackingId(trackingId);
            return new Parcel();
        }

        public Parcel TrackParcel(string trackingId)
        {
            ValidateTrackingId(trackingId);            
            return new Parcel();
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            ValidateParcel(parcel);
            var dalParcel = _mapper.Map<DataAccess.Entities.Parcel>(parcel);
            _trackingRepository.Create(dalParcel);
            return new Parcel();
        }

        public void ReportParcelDelivery(string trackingId)
        {
            ValidateTrackingId(trackingId);
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            ValidateTrackingId(trackingId);
            ValidateCode(code);
        }

        private void ValidateTrackingId(string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId))
                throw new ArgumentNullException();

            if (!Regex.IsMatch(trackingId, _trackingIdPattern))
                throw new ArgumentException("trackingId does not match pattern.");
        }

        private void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException();

            if (!Regex.IsMatch(code, _codePattern))
                throw new ArgumentException("code does not match pattern.");
        }

        private void ValidateParcel(Parcel parcel)
        {
            if (parcel == null)
                throw new ArgumentNullException();
            
            var validator = new ParcelValidator();
            var validationResult = validator.Validate(parcel);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors); 
        }
    }
}