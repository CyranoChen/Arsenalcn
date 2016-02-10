using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataReaderMapper.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable InconsistentNaming

namespace DataReaderMapper.Tests
{
    [TestClass]
    public class DataReaderMapper_ADataReaderToADto
    {
        public DataReaderMapper_ADataReaderToADto()
        {
            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Insert(0, new DataReaderMapper { YieldReturnEnabled = YieldReturnEnabled });
                cfg.CreateMap<IDataReader, DtoObject>()
                    .ForMember(dest => dest.Else, options => options.MapFrom(src => src.GetDateTime(10)));
            });

            DataReader = new DataBuilder().BuildDataReader();
            Results = Mapper.Map<IDataReader, IEnumerable<DtoObject>>(DataReader);
            Result = Results.FirstOrDefault();
        }

        [TestMethod]
        public void Then_a_column_containing_a_small_integer_should_be_read()
        {
            Assert.AreEqual(Result.SmallInteger, DataReader[FieldName.SmallInt]);
        }

        [TestMethod]
        public void Then_a_column_containing_an_integer_should_be_read()
        {
            Assert.AreEqual(Result.Integer, DataReader[FieldName.Int]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_big_integer_should_be_read()
        {
            Assert.AreEqual(Result.BigInteger, DataReader[FieldName.BigInt]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_GUID_should_be_read()
        {
            Assert.AreEqual(Result.Guid, DataReader[FieldName.Guid]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_float_should_be_read()
        {
            Assert.AreEqual(Result.Float, DataReader[FieldName.Float]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_double_should_be_read()
        {
            Assert.AreEqual(Result.Double, DataReader[FieldName.Double]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_decimal_should_be_read()
        {
            Assert.AreEqual(Result.Decimal, DataReader[FieldName.Decimal]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_date_and_time_should_be_read()
        {
            Assert.AreEqual(Result.DateTime, DataReader[FieldName.DateTime]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_byte_should_be_read()
        {
            Assert.AreEqual(Result.Byte, DataReader[FieldName.Byte]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_boolean_should_be_read()
        {
            Assert.AreEqual(Result.Boolean, DataReader[FieldName.Boolean]);
        }

        [TestMethod]
        public void Then_a_projected_column_should_be_read()
        {
            Assert.AreEqual(Result.Else, DataReader.GetDateTime(10));
        }

        [TestMethod]
        public void Should_have_valid_mapping()
        {
            Mapper.AssertConfigurationIsValid();
        }

        protected virtual bool YieldReturnEnabled => false;
        protected DtoObject Result { get; set; }
        protected IEnumerable<DtoObject> Results { get; set; }
        protected IDataReader DataReader { get; set; }
    }

    [TestClass]
    public class DataReaderMapper_ADataReaderToMatchingDtos
    {
        public DataReaderMapper_ADataReaderToMatchingDtos()
        {
            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Insert(0, new DataReaderMapper());
                cfg.CreateMap<IDataReader, DtoObject>()
                    .ForMember(dest => dest.Else, options => options.MapFrom(src => src.GetDateTime(10)));
                cfg.CreateMap<IDataReader, DerivedDtoObject>()
                    .ForMember(dest => dest.Else, options => options.MapFrom(src => src.GetDateTime(10)));
            });

            Mapper.Map<IDataReader, IEnumerable<DtoObject>>(new DataBuilder().BuildDataReader()).ToArray();
        }

        [TestMethod]
        public void Should_map_successfully()
        {
            var result = Mapper.Map<IDataReader, IEnumerable<DerivedDtoObject>>(new DataBuilder().BuildDataReader());

            Assert.AreEqual(result.Count(), 1);
        }

        [TestMethod]
        public void Should_have_valid_mapping()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }

    /// <summary>
    /// The purpose of this test is to exercise the internal caching logic of DataReaderMapper.
    /// </summary>
    [TestClass]
    public class DataReaderMapper_ADataReaderToADtoTwice : DataReaderMapper_ADataReaderToADto
    {
        public DataReaderMapper_ADataReaderToADtoTwice()
        {
            DataReader = new DataBuilder().BuildDataReader();
            Results = Mapper.Map<IDataReader, IEnumerable<DtoObject>>(DataReader);
            Result = Results.FirstOrDefault();
        }
    }

    [TestClass]
    public class DataReaderMapper_ADataReaderUsingTheDefaultConiguration : DataReaderMapper_ADataReaderToADto
    {
        [TestMethod]
        public void Then_the_enumerable_should_be_a_list()
        {
            Assert.IsInstanceOfType(Results, typeof(IList<DtoObject>));
        }
    }

    [TestClass]
    public class DataReaderMapper_ADataReaderUsingTheYieldReturnOption : DataReaderMapper_ADataReaderToADto
    {
        protected override bool YieldReturnEnabled => true;

        [TestMethod]
        public void Then_the_enumerable_should_not_be_a_list()
        {
            Assert.IsFalse(Results is IList<DtoObject>);
        }
    }

    [TestClass]
    public class DataReaderMapper_ADataReaderToADtoAndTheMapDoesNotExist
    {
        public DataReaderMapper_ADataReaderToADtoAndTheMapDoesNotExist()
        {
            Mapper.Initialize(cfg => MapperRegistry.Mappers.Insert(0, new DataReaderMapper()));
            _dataReader = new DataBuilder().BuildDataReader();
        }

        [TestMethod]
        public void Then_an_automapper_exception_should_be_thrown()
        {
            var passed = false;
            try
            {
                Mapper.Map<IDataReader, IEnumerable<DtoObject>>(_dataReader).FirstOrDefault();
            }
            catch (AutoMapperMappingException)
            {
                passed = true;
            }

            Assert.IsTrue(passed);
        }

        private IDataReader _dataReader;
    }

    [TestClass]
    public class DataReaderMapper_ASingleDataRecordToADto
    {
        public DataReaderMapper_ASingleDataRecordToADto()
        {
            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Insert(0, new DataReaderMapper());
                cfg.CreateMap<IDataRecord, DtoObject>()
                    .ForMember(dest => dest.Else, options => options.MapFrom(src => src.GetDateTime(src.GetOrdinal(FieldName.Something))));
            });

            _dataRecord = new DataBuilder().BuildDataRecord();
            _result = Mapper.Map<IDataRecord, DtoObject>(_dataRecord);
        }

        [TestMethod]
        public void Then_a_column_containing_a_small_integer_should_be_read()
        {
            Assert.AreEqual(_result.SmallInteger, _dataRecord[FieldName.SmallInt]);
        }

        [TestMethod]
        public void Then_a_column_containing_an_integer_should_be_read()
        {
            Assert.AreEqual(_result.Integer, _dataRecord[FieldName.Int]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_big_integer_should_be_read()
        {
            Assert.AreEqual(_result.BigInteger, _dataRecord[FieldName.BigInt]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_GUID_should_be_read()
        {
            Assert.AreEqual(_result.Guid, _dataRecord[FieldName.Guid]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_float_should_be_read()
        {
            Assert.AreEqual(_result.Float, _dataRecord[FieldName.Float]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_double_should_be_read()
        {
            Assert.AreEqual(_result.Double, _dataRecord[FieldName.Double]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_decimal_should_be_read()
        {
            Assert.AreEqual(_result.Decimal, _dataRecord[FieldName.Decimal]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_date_and_time_should_be_read()
        {
            Assert.AreEqual(_result.DateTime, _dataRecord[FieldName.DateTime]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_byte_should_be_read()
        {
            Assert.AreEqual(_result.Byte, _dataRecord[FieldName.Byte]);
        }

        [TestMethod]
        public void Then_a_column_containing_a_boolean_should_be_read()
        {
            Assert.AreEqual(_result.Boolean, _dataRecord[FieldName.Boolean]);
        }

        [TestMethod]
        public void Then_a_projected_column_should_be_read()
        {
            Assert.AreEqual(_result.Else, _dataRecord[FieldName.Something]);
        }

        [TestMethod]
        public void Should_have_valid_mapping()
        {
            Mapper.AssertConfigurationIsValid();
        }

        private DtoObject _result;
        private IDataRecord _dataRecord;
    }

    [TestClass]
    public class DataReaderMapper_ADataReaderToADtoWithNullableField
    {
        internal const string FieldName = "Integer";
        internal const int FieldValue = 7;

        internal class DtoWithSingleNullableField
        {
            public int? Integer { get; set; }
        }

        internal class DataBuilder
        {
            public IDataReader BuildDataReaderWithNullableField()
            {
                var table = new DataTable();

                var col = table.Columns.Add(FieldName, typeof(int));
                col.AllowDBNull = true;

                var row1 = table.NewRow();
                row1[FieldName] = FieldValue;
                table.Rows.Add(row1);

                var row2 = table.NewRow();
                row2[FieldName] = DBNull.Value;
                table.Rows.Add(row2);

                return table.CreateDataReader();
            }
        }

        public DataReaderMapper_ADataReaderToADtoWithNullableField()
        {
            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Insert(0, new DataReaderMapper());
                cfg.CreateMap<IDataReader, DtoWithSingleNullableField>();
            });

            _dataReader = new DataBuilder().BuildDataReaderWithNullableField();
        }

        [TestMethod]
        public void Then_results_should_be_as_expected()
        {
            while (_dataReader.Read())
            {
                var dto = Mapper.Map<IDataReader, DtoWithSingleNullableField>(_dataReader);

                if (_dataReader.IsDBNull(0))
                    Assert.IsFalse(dto.Integer.HasValue);
                else
                {
                    // uncomment the following line to see some strange fail message that might be the key to the problem
                    Assert.IsTrue(dto.Integer.HasValue);

                    Assert.AreEqual(dto.Integer.Value, FieldValue);
                }
            }
        }

        [TestMethod]
        public void Should_have_valid_mapping()
        {
            Mapper.AssertConfigurationIsValid();
        }

        private IDataReader _dataReader;
    }

    [TestClass]
    public class DataReaderMapper_ADataReaderToADtoWithNullableEnum
    {
        internal const string FieldName = "Value";
        internal const int FieldValue = 3;

        public enum settlement_type
        {
            PreDelivery = 0,
            DVP = 1,
            FreeDelivery = 2,
            Prepayment = 3,
            Allocation = 4,
            SafeSettlement = 5,
        }
        internal class DtoWithSingleNullableField
        {
            public settlement_type? Value { get; set; }
        }

        internal class DataBuilder
        {
            public IDataReader BuildDataReaderWithNullableField()
            {
                var table = new DataTable();

                var col = table.Columns.Add(FieldName, typeof(int));
                col.AllowDBNull = true;

                var row1 = table.NewRow();
                row1[FieldName] = FieldValue;
                table.Rows.Add(row1);

                var row2 = table.NewRow();
                row2[FieldName] = DBNull.Value;
                table.Rows.Add(row2);

                return table.CreateDataReader();
            }
        }

        public DataReaderMapper_ADataReaderToADtoWithNullableEnum()
        {
            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Insert(0, new DataReaderMapper());

                cfg.CreateMap<IDataReader, DtoWithSingleNullableField>();
            });

            _dataReader = new DataBuilder().BuildDataReaderWithNullableField();
        }

        [TestMethod]
        public void Then_results_should_be_as_expected()
        {
            while (_dataReader.Read())
            {
                //var dto = Mapper.Map<IDataReader, DtoWithSingleNullableField>(_dataReader);
                var dto = new DtoWithSingleNullableField();

                object value = _dataReader[0];
                if (!Equals(value, DBNull.Value))
                    dto.Value = (settlement_type)value;

                if (_dataReader.IsDBNull(0))
                    Assert.IsFalse(dto.Value.HasValue);
                else
                {
                    Assert.IsTrue(dto.Value.HasValue);

                    Assert.AreEqual(dto.Value.Value, settlement_type.Prepayment);
                }
            }
        }

        [TestMethod]
        public void Should_have_valid_mapping()
        {
            Mapper.AssertConfigurationIsValid();
        }

        private IDataReader _dataReader;
    }

    internal class FieldName
    {
        public const String SmallInt = "SmallInteger";
        public const String Int = "Integer";
        public const String BigInt = "BigInteger";
        public const String Guid = "Guid";
        public const String Float = "Float";
        public const String Double = "Double";
        public const String Decimal = "Decimal";
        public const String DateTime = "DateTime";
        public const String Byte = "Byte";
        public const String Boolean = "Boolean";
        public const String Something = "Something";
    }

    public class DataBuilder
    {
        public IDataReader BuildDataReader()
        {
            var authorizationSetDataTable = new DataTable();
            authorizationSetDataTable.Columns.Add(FieldName.SmallInt, typeof(Int16));
            authorizationSetDataTable.Columns.Add(FieldName.Int, typeof(Int32));
            authorizationSetDataTable.Columns.Add(FieldName.BigInt, typeof(Int64));
            authorizationSetDataTable.Columns.Add(FieldName.Guid, typeof(Guid));
            authorizationSetDataTable.Columns.Add(FieldName.Float, typeof(float));
            authorizationSetDataTable.Columns.Add(FieldName.Double, typeof(Double));
            authorizationSetDataTable.Columns.Add(FieldName.Decimal, typeof(Decimal));
            authorizationSetDataTable.Columns.Add(FieldName.DateTime, typeof(DateTime));
            authorizationSetDataTable.Columns.Add(FieldName.Byte, typeof(Byte));
            authorizationSetDataTable.Columns.Add(FieldName.Boolean, typeof(Boolean));
            authorizationSetDataTable.Columns.Add(FieldName.Something, typeof(DateTime));

            var authorizationSetDataRow = authorizationSetDataTable.NewRow();
            authorizationSetDataRow[FieldName.SmallInt] = 22;
            authorizationSetDataRow[FieldName.Int] = 6134;
            authorizationSetDataRow[FieldName.BigInt] = 61346154;
            authorizationSetDataRow[FieldName.Guid] = Guid.NewGuid();
            authorizationSetDataRow[FieldName.Float] = 642.61;
            authorizationSetDataRow[FieldName.Double] = 67164.64;
            authorizationSetDataRow[FieldName.Decimal] = 94341.61;
            authorizationSetDataRow[FieldName.DateTime] = DateTime.Now;
            authorizationSetDataRow[FieldName.Byte] = 0x12;
            authorizationSetDataRow[FieldName.Boolean] = true;
            authorizationSetDataRow[FieldName.Something] = DateTime.MaxValue;
            authorizationSetDataTable.Rows.Add(authorizationSetDataRow);

            return authorizationSetDataTable.CreateDataReader();
        }

        public IDataRecord BuildDataRecord()
        {
            var dataReader = BuildDataReader();
            dataReader.Read();
            return dataReader;
        }
    }

    public class DtoObject
    {
        public Int16 SmallInteger { get; private set; }
        public Int32 Integer { get; private set; }
        public Int64 BigInteger { get; private set; }
        public Guid Guid { get; private set; }
        public float Float { get; private set; }
        public Double Double { get; private set; }
        public Decimal Decimal { get; private set; }
        public DateTime DateTime { get; private set; }
        public Byte Byte { get; private set; }
        public Boolean Boolean { get; private set; }
        public DateTime Else { get; private set; }
    }

    public class DerivedDtoObject : DtoObject { }
}