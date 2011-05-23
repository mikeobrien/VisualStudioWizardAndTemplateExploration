using System;

namespace VisualStudio.Data
{
    public static class SqlTypeExtensions
    {
        public const string SqlBigInt = "bigint";
        public const string SqlBinary = "binary";
        public const string SqlBit = "bit";
        public const string SqlChar = "char";
        public const string SqlDate = "date";
        public const string SqlDateTime = "datetime";
        public const string SqlDateTime2 = "datetime2";
        public const string SqlDateTimeOffset = "datetimeoffset";
        public const string SqlDecimal = "decimal";
        public const string SqlFloat = "float";
        public const string SqlImage = "image";
        public const string SqlInt = "int";
        public const string SqlMoney = "money";
        public const string SqlNChar = "nchar";
        public const string SqlNumeric = "numeric";
        public const string SqlNText = "ntext";
        public const string SqlNVarChar = "nvarchar";
        public const string SqlReal = "real";
        public const string SqlRowVersion = "rowversion";
        public const string SqlSmallDateTime = "smalldatetime";
        public const string SqlSmallInt = "smallint";
        public const string SqlSmallMoney = "smallmoney";
        public const string SqlText = "text";
        public const string SqlTime = "time";
        public const string SqlTimestamp = "timestamp";
        public const string SqlTinyInt = "tinyint";
        public const string SqlUniqueidentifier = "uniqueidentifier";
        public const string SqlVarBinary = "varbinary";
        public const string SqlVarChar = "varchar";
        public const string SqlVariant = "sql_variant";
        public const string SqlXml = "xml";

        public static string ToSqlType(this Type type)
        {
            if (type == typeof(DateTime?) || type == typeof(DateTime)) return SqlDateTime;
            if (type == typeof(DateTimeOffset?) || type == typeof(DateTimeOffset)) return SqlDateTimeOffset;
            if (type == typeof(TimeSpan?) || type == typeof(TimeSpan)) return SqlTime;
            if (type == typeof(Boolean?) || type == typeof(Boolean)) return SqlBit;
            if (type == typeof(Byte[])) return SqlVarBinary;
            if (type == typeof(Byte?) || type == typeof(Byte)) return SqlTinyInt;
            if (type == typeof(Int16?) || type == typeof(Int16)) return SqlSmallInt;
            if (type == typeof(Int32?) || type == typeof(Int32)) return SqlInt;
            if (type == typeof(Int64?) || type == typeof(Int64)) return SqlBigInt;
            if (type == typeof(Decimal?) || type == typeof(Decimal)) return SqlDecimal;
            if (type == typeof(Single?) || type == typeof(Single)) return SqlReal;
            if (type == typeof(Double?) || type == typeof(Double)) return SqlFloat;
            if (type == typeof(Char)) return SqlNChar;
            if (type == typeof(String)) return SqlNVarChar;
            if (type == typeof(Object)) return SqlVariant;
            if (type == typeof(Guid?) || type == typeof(Guid)) return SqlUniqueidentifier;
            throw new Exception(string.Format("No SQL data type found to match CLR data type'{0}'.", type.Name));
        }

        public static string ToSqlTypeString(this SqlTypes type)
        {
            switch (type)
            {
                case SqlTypes.Image: return SqlImage;
                case SqlTypes.Text: return SqlText;
                case SqlTypes.Uniqueidentifier: return SqlUniqueidentifier;
                case SqlTypes.Date: return SqlDate;
                case SqlTypes.Time: return SqlTime;
                case SqlTypes.DateTime2: return SqlDateTime2;
                case SqlTypes.DateTimeOffset: return SqlDateTimeOffset;
                case SqlTypes.TinyInt: return SqlTinyInt;
                case SqlTypes.SmallInt: return SqlSmallInt;
                case SqlTypes.Int: return SqlInt;
                case SqlTypes.SmallDateTime: return SqlDateTime;
                case SqlTypes.Real: return SqlReal;
                case SqlTypes.Money: return SqlMoney;
                case SqlTypes.DateTime: return SqlDateTime;
                case SqlTypes.Float: return SqlFloat;
                case SqlTypes.Variant: return SqlVariant;
                case SqlTypes.NText: return SqlNText;
                case SqlTypes.Bit: return SqlBit;
                case SqlTypes.Decimal: return SqlDecimal;
                case SqlTypes.Numeric: return SqlNumeric;
                case SqlTypes.SmallMoney: return SqlSmallMoney;
                case SqlTypes.BigInt: return SqlBigInt;
                case SqlTypes.VarBinary: return SqlVarBinary;
                case SqlTypes.VarChar: return SqlVarChar;
                case SqlTypes.Binary: return SqlBinary;
                case SqlTypes.Char: return SqlChar;
                case SqlTypes.Timestamp: return SqlTimestamp;
                case SqlTypes.NVarChar: return SqlNVarChar;
                case SqlTypes.NChar: return SqlNChar;
                case SqlTypes.Xml: return SqlXml;
                default: return null;
            }            
        }

        public static Type ToClrType(this int sqlType) { return ToClrType(sqlType, false); }

        public static Type ToClrType(this int sqlType, bool nullable)
        { return ToClrType((SqlTypes)sqlType, nullable); }

        public static Type ToClrType(this SqlTypes sqlType) { return ToClrType(sqlType, false); }

        public static Type ToClrType(this SqlTypes sqlType, bool nullable)
        {
            switch (sqlType)
            {
                case SqlTypes.BigInt: return nullable ? typeof(Int64?) : typeof(Int64);
                case SqlTypes.Binary: return typeof(Byte[]);
                case SqlTypes.Bit: return nullable ? typeof(Boolean?) : typeof(Boolean);
                case SqlTypes.Char: return typeof(Char);
                case SqlTypes.Date: return nullable ? typeof(DateTime?) : typeof(DateTime);
                case SqlTypes.DateTime: return nullable ? typeof(DateTime?) : typeof(DateTime);
                case SqlTypes.DateTime2: return nullable ? typeof(DateTime?) : typeof(DateTime);
                case SqlTypes.SmallDateTime: return nullable ? typeof(DateTime?) : typeof(DateTime);
                case SqlTypes.DateTimeOffset: return nullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);
                case SqlTypes.Decimal: return nullable ? typeof(Decimal?) : typeof(Decimal);
                case SqlTypes.Float: return nullable ? typeof(Double?) : typeof(Double);
                case SqlTypes.Int: return nullable ? typeof(Int32?) : typeof(Int32);
                case SqlTypes.Money: return nullable ? typeof(Decimal?) : typeof(Decimal);
                case SqlTypes.NChar: return typeof(String);
                case SqlTypes.Numeric: return nullable ? typeof(Decimal?) : typeof(Decimal);
                case SqlTypes.VarChar: return typeof(String);
                case SqlTypes.NVarChar: return typeof(String);
                case SqlTypes.Real: return nullable ? typeof(Single?) : typeof(Single);
                case SqlTypes.SmallInt: return nullable ? typeof(Int16?) : typeof(Int16);
                case SqlTypes.SmallMoney: return nullable ? typeof(Decimal?) : typeof(Decimal);
                case SqlTypes.Variant: return typeof(Object);
                case SqlTypes.Time: return nullable ? typeof(TimeSpan?) : typeof(TimeSpan);
                case SqlTypes.TinyInt: return nullable ? typeof(Byte?) : typeof(Byte);
                case SqlTypes.Uniqueidentifier: return nullable ? typeof(Guid?) : typeof(Guid);
                case SqlTypes.VarBinary: return typeof(Byte[]);
                default: throw new Exception(string.Format("No CLR data type found to match SQL data type'{0}'.", sqlType));
            }
        }

        public static Type ToClrType(this string sqlType) { return ToClrType(sqlType, false); }

        public static Type ToClrType(this string sqlType, bool nullable)
        {
            if (sqlType.Equals(SqlBigInt, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.BigInt, nullable);
            if (sqlType.Equals(SqlBinary, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Binary, nullable);
            if (sqlType.Equals(SqlBit, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Bit, nullable);
            if (sqlType.Equals(SqlChar, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Char, nullable);
            if (sqlType.Equals(SqlDate, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Date, nullable);
            if (sqlType.Equals(SqlDateTime, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.DateTime, nullable);
            if (sqlType.Equals(SqlDateTime2, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.DateTime2, nullable);
            if (sqlType.Equals(SqlSmallDateTime, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.SmallDateTime, nullable);
            if (sqlType.Equals(SqlDateTimeOffset, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.DateTimeOffset, nullable);
            if (sqlType.Equals(SqlDecimal, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Decimal, nullable);
            if (sqlType.Equals(SqlFloat, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Float, nullable);
            if (sqlType.Equals(SqlInt, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Int, nullable);
            if (sqlType.Equals(SqlMoney, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Money, nullable);
            if (sqlType.Equals(SqlNChar, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.NChar, nullable);
            if (sqlType.Equals(SqlNumeric, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Numeric, nullable);
            if (sqlType.Equals(SqlVarChar, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.VarChar, nullable);
            if (sqlType.Equals(SqlNVarChar, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.NVarChar, nullable);
            if (sqlType.Equals(SqlReal, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Real, nullable);
            if (sqlType.Equals(SqlSmallInt, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.SmallInt, nullable);
            if (sqlType.Equals(SqlSmallMoney, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.SmallMoney, nullable);
            if (sqlType.Equals(SqlVariant, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Variant, nullable);
            if (sqlType.Equals(SqlTime, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Time, nullable);
            if (sqlType.Equals(SqlTinyInt, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.TinyInt, nullable);
            if (sqlType.Equals(SqlUniqueidentifier, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.Uniqueidentifier, nullable);
            if (sqlType.Equals(SqlVarBinary, StringComparison.OrdinalIgnoreCase)) return ToClrType(SqlTypes.VarBinary, nullable);
            throw new Exception(string.Format("No CLR data type found to match SQL data type'{0}'.", sqlType));
        }
    }
}
