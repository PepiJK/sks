using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController _warehouseManagementApiController;

        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //moq configuration
            var mock = new Mock<IWarehouseLogic>();
            mock.Setup(trackingLogic => trackingLogic.ExportWarehouses()).Returns(new BusinessLogic.Entities.Warehouse());
            mock.Setup(trackingLogic => trackingLogic.GetWarehouse(It.IsAny<string>())).Returns(new BusinessLogic.Entities.Warehouse());
            mock.Setup(trackingLogic => trackingLogic.ImportWarehouses(new BusinessLogic.Entities.Warehouse()));

            _warehouseManagementApiController = new WarehouseManagementApiController(mapper, mock.Object);
        }

        [Test]
        public void Should_Export_Warehouses()
        {
            var res = _warehouseManagementApiController.ExportWarehouses() as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Get_Warehouse()
        {
            var res = _warehouseManagementApiController.GetWarehouse("CODE") as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Get_Warehouse()
        {
            var res = _warehouseManagementApiController.GetWarehouse(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }

        [Test]
        public void Should_Import_Warehouses()
        {
            var res = _warehouseManagementApiController.ImportWarehouses(new Warehouse()) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Import_Warehouses()
        {
            var res = _warehouseManagementApiController.ImportWarehouses(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(400, res.StatusCode);
        }
    }
}