using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepository;
        private readonly ParcelValidator _parcelValidator = new ParcelValidator();
        private readonly TrackingIdValidator _trackingIdValidator = new TrackingIdValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();
        private readonly ILogger _logger;


        public TrackingLogic(IMapper mapper, IParcelRepository parcelRepository, ILogger<TrackingLogic> logger)
        {
            _parcelRepository = parcelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        private BL_Exception ExceptionHandler(string method, Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new BL_Exception(method, ex);
        }

        private BL_NotFound_Exception NotFound_ExceptionHandler(string method, Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new BL_NotFound_Exception(method, ex);
        }

        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            try
            {
                Validate<Parcel>(parcel, _parcelValidator);
                Validate<string>(trackingId, _trackingIdValidator);

                parcel.TrackingId = trackingId;
                SubmitParcel(parcel);
                return parcel;
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public Parcel TrackParcel(string trackingId)
        {
            try
            {
                Validate<string>(trackingId, _trackingIdValidator);

                var dalParcel = _parcelRepository.GetParcelByTrackingId(trackingId);
                var blParcel = _mapper.Map<BusinessLogic.Entities.Parcel>(dalParcel);

                return blParcel;
            }
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }

        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            try
            {
                Validate<Parcel>(parcel, _parcelValidator);

                var dalParcel = _mapper.Map<DataAccess.Entities.Parcel>(parcel);
                _parcelRepository.Create(dalParcel);
                return new Parcel() { TrackingId = parcel.TrackingId };
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public void ReportParcelDelivery(string trackingId)
        {
            try
            {
                Validate<string>(trackingId, _trackingIdValidator);

                var dalParcel = _parcelRepository.GetParcelByTrackingId(trackingId);
                dalParcel.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum;
                _parcelRepository.Update(dalParcel);
            }
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            try
            {
                Validate<string>(trackingId, _trackingIdValidator);
                Validate<string>(code, _codeValidator);
            }
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        private void Validate<T>(T instanceToValidate, AbstractValidator<T> validator)
        {
            var validationResult = validator.Validate(instanceToValidate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}