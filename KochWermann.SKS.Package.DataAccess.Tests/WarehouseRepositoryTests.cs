using System.Collections.Generic;
using System.Linq;
using KochWermann.SKS.Package.DataAccess.Entities;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Sql;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.DataAccess.Tests
{
    public class WarehouseRepositoryTests
    {
        private IWarehouseRepository _warehouseRepository;
        private List<Hop> _hops;
        private List<Warehouse> _warehouses;
        private List<WarehouseNextHops> _warehouseNextHops;
        private List<Truck> _trucks;
        private List<TransferWarehouse> _transferWarehouses;

        [SetUp]
        public void Setup()
        {
            _hops = new List<Hop>{
                new Warehouse{
                    Level = 0,
                    Code = "CODE1234",
                    HopType = "Warehouse",
                    NextHops = new List<WarehouseNextHops>{
                        new WarehouseNextHops{
                            Hop = new TransferWarehouse{
                                Code = "TRAN1234",
                                HopType = "TransferWarehouse",
                                RegionGeometry = new NetTopologySuite.Geometries.Point(2, 2),
                                LocationCoordinates = new NetTopologySuite.Geometries.Point(2, 2),
                            }
                        }
                    }
                },
                new Truck{
                    Code = "TRUC1234",
                    HopType = "Truck",
                    LocationCoordinates = new NetTopologySuite.Geometries.Point(1, 1),
                    RegionGeometry = new NetTopologySuite.Geometries.Point(1, 1)
                }
            };

            _warehouses = new List<Warehouse>{
                _hops[0] as Warehouse
            };
            _warehouses[0].IsRootWarehouse = true;

            _warehouseNextHops = new List<WarehouseNextHops>{
                (_hops[0] as Warehouse).NextHops[0]
            };

            _trucks = new List<Truck>{
                _hops[1] as Truck
            };

            _transferWarehouses = new List<TransferWarehouse>{
                (_hops[0] as Warehouse).NextHops[0].Hop as TransferWarehouse
            };

            var loggerMock = new Mock<ILogger<SqlWarehouseRepository>>();

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(p => p.Hops).Returns(DbContextMock.GetQueryableMockDbSet<Hop>(_hops));
            mockContext.Setup(p => p.Warehouses).Returns(DbContextMock.GetQueryableMockDbSet<Warehouse>(_warehouses));
            mockContext.Setup(p => p.WarehouseNextHops).Returns(DbContextMock.GetQueryableMockDbSet<WarehouseNextHops>(_warehouseNextHops));
            mockContext.Setup(p => p.Trucks).Returns(DbContextMock.GetQueryableMockDbSet<Truck>(_trucks));
            mockContext.Setup(p => p.TransferWarehouses).Returns(DbContextMock.GetQueryableMockDbSet<TransferWarehouse>(_transferWarehouses));
            mockContext.Setup(p => p.HopArrivals).Returns(DbContextMock.GetQueryableMockDbSet<HopArrival>(new List<HopArrival>()));
            mockContext.Setup(p => p.Parcels).Returns(DbContextMock.GetQueryableMockDbSet<Parcel>(new List<Parcel>()));
            mockContext.Setup(p => p.Recipients).Returns(DbContextMock.GetQueryableMockDbSet<Recipient>(new List<Recipient>()));

            mockContext.Setup(p => p.SaveChanges()).Returns(1);

            _warehouseRepository = new SqlWarehouseRepository(mockContext.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Create_Hop()
        {
            var newHop = new Hop();

            _warehouseRepository.Create(newHop);

            Assert.AreEqual(3, _hops.Count);
            Assert.AreEqual(newHop, _hops[2]);
        }

        [Test]
        public void Should_Update()
        {
            Assert.DoesNotThrow(() => _warehouseRepository.Update(_hops[0]));
        }

        [Test]
        public void Should_Delete()
        {
            _warehouseRepository.Delete("CODE1234");

            Assert.AreEqual(1, _hops.Count);
        }

        [Test]
        public void Should_Throw_Not_Found_On_Delete()
        {
            Assert.Throws<DALNotFoundException>(() => _warehouseRepository.Delete("CODE4321"));
        }

        [Test]
        public void Should_Get_Warehouse_By_Code()
        {
            var warehouse = _warehouseRepository.GetWarehouseByCode("CODE1234");

            Assert.AreEqual(_warehouses[0], warehouse);
        }

        [Test]
        public void Should_Throw_Not_Found_On_Get_Warehouse_By_Code()
        {
            Assert.Throws<DALNotFoundException>(() => _warehouseRepository.GetWarehouseByCode("CODE4321"));
        }

        [Test]
        public void Should_Get_Root_Warehouse()
        {
            var warehouse = _warehouseRepository.GetRootWarehouse();

            Assert.AreEqual(_hops[0].Code, warehouse.Code);
            Assert.AreEqual(true, warehouse.IsRootWarehouse);
        }

        [Test]
        public void Should_Get_Hop_By_Code()
        {
            var hop = _warehouseRepository.GetHopByCode("CODE1234");

            Assert.AreEqual(_hops[0], hop);
        }

        [Test]
        public void Should_Throw_Not_Found_On_Get_Hop_By_Code()
        {
            Assert.Throws<DALNotFoundException>(() => _warehouseRepository.GetHopByCode("CODE4321"));
        }

        [Test]
        public void Should_Get_All_Trucks()
        {
            var trucks = _warehouseRepository.GetAllTrucks();

            Assert.AreEqual(_trucks, trucks.ToList());
        }

        [Test]
        public void Should_Get_All_Warehouses()
        {
            var warehouses = _warehouseRepository.GetAllWarehouses();

            Assert.AreEqual(_warehouses, warehouses.ToList());
        }

        [Test]
        public void Should_Get_Truck_By_Coordinates()
        {
            var hop = _warehouseRepository.GetHopByCoordinates(1, 1);

            Assert.AreEqual(_trucks[0], hop as Truck);
        }

        [Test]
        public void Should_Get_TransferWarehouse_By_Coordinates()
        {
            var hop = _warehouseRepository.GetHopByCoordinates(2,2);

            Assert.AreEqual(_transferWarehouses[0], hop as TransferWarehouse);
        }

        [Test]
        public void Should_Not_Get_Hop_By_Coordinates()
        {
            var hop = _warehouseRepository.GetHopByCoordinates(3, 1);

            Assert.AreEqual(null, hop);
        }

        [Test]
        public void Should_Clear_All_Tables()
        {
            Assert.DoesNotThrow(() => _warehouseRepository.ClearAllTables());
        }
    }
}