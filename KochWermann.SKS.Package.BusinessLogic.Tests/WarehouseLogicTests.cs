using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using FluentValidation;
using System;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseLogicTests
    {
        private IWarehouseLogic _warehouseLogic = new WarehouseLogic();
        private Warehouse _validWarehouse;
        private string _validCode = "CODE123";
        private string _invalidCode = "hi";

        [SetUp]
        public void Setup()
        {
            _validWarehouse = new Warehouse{
                Code = _validCode,
                Description = "This should be a valid description",
                NextHops = new List<WarehouseNextHops>{new WarehouseNextHops{TraveltimeMins = 69}}
            };
        }

        [Test]
        public void Should_Export_Warehouses()
        {
            var res = _warehouseLogic.ExportWarehouses();
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Warehouse>(res);
        }

        [Test]
        public void Should_Import_Warehouses()
        {
            Assert.DoesNotThrow(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Null_Warehouse()
        {
            Assert.Throws<ArgumentNullException>(() => _warehouseLogic.ImportWarehouses(null));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Code()
        {
            _validWarehouse.Code = _invalidCode;
            Assert.Throws<ValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Next_Hop_Truck()
        {
            _validWarehouse.NextHops[0].Hop = new Truck(); 
            Assert.Throws<ValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Next_Hop_Warehouse()
        {
            _validWarehouse.NextHops[0].Hop = new Warehouse(); 
            Assert.Throws<ValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Next_Hop_Transferwarehouse()
        {
            _validWarehouse.NextHops[0].Hop = new TransferWarehouse(); 
            Assert.Throws<ValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Get_Warehouse()
        {
            var res = _warehouseLogic.GetWarehouse(_validCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Warehouse>(res);
        }

        [Test]
        public void Should_Throw_Exception_On_Get_Warehouses_Of_Invalid_Code()
        {
            Assert.Throws<ArgumentException>(() => _warehouseLogic.GetWarehouse(_invalidCode));
        }

        [Test]
        public void Should_Throw_Exception_On_Get_Warehouses_Of_Null_Code()
        {
            Assert.Throws<ArgumentNullException>(() => _warehouseLogic.GetWarehouse(null));
        }
    }
}