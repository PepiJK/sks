using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController warehouseManagementApiController;

        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            warehouseManagementApiController = new WarehouseManagementApiController(mapper);
        }

        [Test]
        public void Should_Export_Warehouses()
        {
            var res = warehouseManagementApiController.ExportWarehouses() as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Get_Warehouse()
        {
            var res = warehouseManagementApiController.GetWarehouse("CODE") as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Get_Warehouse()
        {
            var res = warehouseManagementApiController.GetWarehouse(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }

        [Test]
        public void Should_Import_Warehouses()
        {
            var res = warehouseManagementApiController.ImportWarehouses(new Warehouse()) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Import_Warehouses()
        {
            var res = warehouseManagementApiController.ImportWarehouses(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(400, res.StatusCode);
        }
    }
}