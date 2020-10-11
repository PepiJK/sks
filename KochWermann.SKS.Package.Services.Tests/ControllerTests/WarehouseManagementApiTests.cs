using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FizzWare.NBuilder;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController _warehouseManagementApiController;
        private Warehouse _testWarehouse;
        private string _testCode = "CODE";

        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //generate test object
            _testWarehouse = Builder<Warehouse>.CreateNew()
                .With(x => x.Level = 0)
                .With(x => x.LocationCoordinates = new GeoCoordinate{Lat = 50, Lon = 50})
                .With(x => x.HopType = "Warehouse")
                .With(x => x.NextHops = new System.Collections.Generic.List<WarehouseNextHops>{
                    Builder<WarehouseNextHops>.CreateNew()
                        .With(x => x.Hop = Builder<Hop>.CreateNew()
                            .With(x => x.LocationCoordinates = new GeoCoordinate{Lat = 100, Lon = 100})
                        .Build())
                    .Build()
                })
            .Build();

            //mock tracking logic
            var mock = new Mock<IWarehouseLogic>();    
            
            mock.Setup(trackingLogic => trackingLogic.ExportWarehouses()).Returns(new BusinessLogic.Entities.Warehouse());     
            
            mock.Setup(trackingLogic => trackingLogic.GetWarehouse(
                It.IsAny<string>()
            )).Returns(new BusinessLogic.Entities.Warehouse());            
            
            mock.Setup(trackingLogic => trackingLogic.ImportWarehouses(
                It.IsAny<BusinessLogic.Entities.Warehouse>()
            ));

            _warehouseManagementApiController = new WarehouseManagementApiController(mapper, mock.Object);
        }

        [Test]
        public void Should_Export_Warehouses()
        {
            var res = _warehouseManagementApiController.ExportWarehouses();
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Warehouse>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Get_Warehouse()
        {
            var res = _warehouseManagementApiController.GetWarehouse(_testCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Warehouse>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Not_Get_Warehouse()
        {
            var res = _warehouseManagementApiController.GetWarehouse(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public void Should_Import_Warehouses()
        {
            var res = _warehouseManagementApiController.ImportWarehouses(_testWarehouse);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkResult>(res);
        }

        [Test]
        public void Should_Not_Import_Warehouses()
        {
            var res = _warehouseManagementApiController.ImportWarehouses(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Error>((res as BadRequestObjectResult).Value);
        }
    }
}