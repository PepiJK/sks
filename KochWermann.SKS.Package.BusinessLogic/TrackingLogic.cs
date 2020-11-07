using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using KochWermann.SKS.Package.BusinessLogic.Helpers;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepository;
        private readonly ILogger _logger;
        private readonly ParcelValidator _parcelValidator = new ParcelValidator();
        private readonly TrackingIdValidator _trackingIdValidator = new TrackingIdValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();


        public TrackingLogic(IMapper mapper, IParcelRepository parcelRepository, ILogger<TrackingLogic> logger)
        {
            _parcelRepository = parcelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<Parcel>(parcel, _parcelValidator, _logger);
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                parcel.TrackingId = trackingId;
                SubmitParcel(parcel);
                return parcel;
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }

        public Parcel TrackParcel(string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                var dalParcel = _parcelRepository.GetParcelByTrackingId(trackingId);
                var blParcel = _mapper.Map<BusinessLogic.Entities.Parcel>(dalParcel);

                return blParcel;
            }
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw BusinessLogicHelper.NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }

        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            try
            {
                BusinessLogicHelper.Validate<Parcel>(parcel, _parcelValidator, _logger);

                var dalParcel = _mapper.Map<DataAccess.Entities.Parcel>(parcel);
                _parcelRepository.Create(dalParcel);
                return parcel;
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }

        public void ReportParcelDelivery(string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                var dalParcel = _parcelRepository.GetParcelByTrackingId(trackingId);
                dalParcel.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum;
                _parcelRepository.Update(dalParcel);
            }
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw BusinessLogicHelper.NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);
                BusinessLogicHelper.Validate<string>(code, _codeValidator, _logger);
            }
            /*
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw BusinessLogicHelper.NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            */
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }

        
    }
}