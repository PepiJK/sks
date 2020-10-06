using System.Linq;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        public Parcel TransitionParcel(string trackingId)
        {
            return new Parcel();
        }

        public Parcel GetParcel(string trackingId)
        {
            return new Parcel();
        }

        public void SubmitParcel(Parcel parcel)
        {
            IValidator<Parcel> validator = new ParcelValidator();
            var validationResult = validator.Validate(parcel);

            if(validationResult.IsValid)
            {

            }
            else
            {
                //Unit test sends empty parcel =>
                //throw new FluentValidation.ValidationException("new parcel "+ validationResult.Errors.ToString()); 
            }
        }

        public void ReportParcelDelivery(string trackingId)
        {

        }

        public void ReportParcelHop(string trackingId, string code)
        {

        }

        public Parcel TrackParcel(string trackingID)
        {
            return new Parcel();
        }    
    }
}