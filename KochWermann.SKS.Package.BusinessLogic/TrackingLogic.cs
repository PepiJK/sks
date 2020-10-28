using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepository;
        private readonly ParcelValidator _parcelValidator = new ParcelValidator();
        private readonly TrackingIdValidator _trackingIdValidator = new TrackingIdValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();

        public TrackingLogic(IMapper mapper, IParcelRepository parcelRepository)
        {
            _parcelRepository = parcelRepository;
            _mapper = mapper;
        }

        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            Validate<Parcel>(parcel, _parcelValidator);
            Validate<string>(trackingId, _trackingIdValidator);

            return new Parcel();
        }

        public Parcel TrackParcel(string trackingId)
        {
            Validate<string>(trackingId, _trackingIdValidator);

            var dalParcel = _parcelRepository.GetParcelByTrackingId(trackingId);
            var blParcel = _mapper.Map<BusinessLogic.Entities.Parcel>(dalParcel);

            return blParcel;
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            Validate<Parcel>(parcel, _parcelValidator);

            var dalParcel = _mapper.Map<DataAccess.Entities.Parcel>(parcel);
            _parcelRepository.Create(dalParcel);
            
            return parcel;
        }

        public void ReportParcelDelivery(string trackingId)
        { 
            Validate<string>(trackingId, _trackingIdValidator);
            
            var dalParcel = _parcelRepository.GetParcelByTrackingId(trackingId);
            dalParcel.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum;
            _parcelRepository.Update(dalParcel);
            
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            Validate<string>(trackingId, _trackingIdValidator);
            Validate<string>(code, _codeValidator);
        }

      
        private void Validate<T>(T instanceToValidate, AbstractValidator<T> validator)
        {
            var validationResult = validator.Validate(instanceToValidate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors); 
        }
    }
}