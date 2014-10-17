using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using System.Data;

namespace Kemel.Orm.QueryDef
{
    public class AggregateCollection: List<Aggregate>
    {

        new public Aggregate Add(Aggregate item)
        {
            base.Add(item);
            return item;
        }

    }

    public class Aggregate
    {

        #region Properties

        public ColumnQuery Column { get; set; }

        public object Parameter { get; set; }

        public Aggregate SubAggregate { get; set; }

        public AggregateFunction Function { get; set; }

        public string Alias { get; set; }

        public string ConvertType { get; set; }

        public TableQuery Parent { get; private set; }

        public DbType TypeToConvert { get; set; }

        #endregion

        #region .ctor

        private Aggregate(object parameter, AggregateFunction function, TableQuery parent)
        {
            this.Parameter = parameter;
            this.Function = function;
            this.Parent = parent;
        }

        private Aggregate(AggregateFunction function, string columnName, TableQuery parent)
        {
            this.Column = new ColumnQuery(columnName, parent);
            this.Function = function;
            this.Parent = parent;
        }

        private Aggregate(AggregateFunction function, ColumnSchema column, TableQuery parent)
        {
            this.Column = new ColumnQuery(column, parent);
            this.Function = function;
            this.Parent = parent;
        }

        private Aggregate(AggregateFunction function, Aggregate subAggregate, TableQuery parent)
        {
            this.SubAggregate = subAggregate;
            this.Function = function;
            this.Parent = parent;
        }

        #endregion

        #region Methods

        public TableQuery As(string alias)
        {
            this.Alias = alias;
            return this.Parent;
        }

        public TableQuery End()
        {
            return this.Parent;
        }

        #endregion

        #region Methods Function Parameter

        public static Aggregate Count(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.Count, parent);
        }

        public static Aggregate Sum(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.Sum, parent);
        }

        public static Aggregate Avg(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.Avg, parent);
        }

        public static Aggregate Min(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.Min, parent);
        }

        public static Aggregate Max(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.Max, parent);
        }

        public static Aggregate StDev(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.StDev, parent);
        }

        public static Aggregate StDevP(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.StDevP, parent);
        }

        public static Aggregate Var(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.Var, parent);
        }

        public static Aggregate VarP(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.VarP, parent);
        }

        public static Aggregate Convert(object parameter, TableQuery parent)
        {
            return new Aggregate(parameter, AggregateFunction.Convert, parent);
        }

        #endregion

        #region Methods Function Column Name

        public static Aggregate CountColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Count, columnName, parent);
        }

        public static Aggregate SumColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Sum, columnName, parent);
        }

        public static Aggregate AvgColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Avg, columnName, parent);
        }

        public static Aggregate MinColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Min, columnName, parent);
        }

        public static Aggregate MaxColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Max, columnName, parent);
        }

        public static Aggregate StDevColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.StDev, columnName, parent);
        }

        public static Aggregate StDevPColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.StDevP, columnName, parent);
        }

        public static Aggregate VarColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Var, columnName, parent);
        }

        public static Aggregate VarPColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.VarP, columnName, parent);
        }

        public static Aggregate ConvertColumn(string columnName, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Convert, columnName, parent);
        }

        #endregion

        #region Methods Function Column

        public static Aggregate CountColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Count, column, parent);
        }

        public static Aggregate SumColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Sum, column, parent);
        }

        public static Aggregate AvgColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Avg, column, parent);
        }

        public static Aggregate MinColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Min, column, parent);
        }

        public static Aggregate MaxColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Max, column, parent);
        }

        public static Aggregate StDevColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.StDev, column, parent);
        }

        public static Aggregate StDevPColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.StDevP, column, parent);
        }

        public static Aggregate VarColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Var, column, parent);
        }

        public static Aggregate VarPColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.VarP, column, parent);
        }

        public static Aggregate ConvertColumn(ColumnSchema column, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Convert, column, parent);
        }

        #endregion

        #region Methods Function Sub-Aggregate

        public static Aggregate Count(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Count, subAggregate, parent);
        }

        public static Aggregate Sum(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Sum, subAggregate, parent);
        }

        public static Aggregate Avg(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Avg, subAggregate, parent);
        }

        public static Aggregate Min(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Min, subAggregate, parent);
        }

        public static Aggregate Max(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Max, subAggregate, parent);
        }

        public static Aggregate StDev(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.StDev, subAggregate, parent);
        }

        public static Aggregate StDevP(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.StDevP, subAggregate, parent);
        }

        public static Aggregate Var(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Var, subAggregate, parent);
        }

        public static Aggregate VarP(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.VarP, subAggregate, parent);
        }

        public static Aggregate Convert(Aggregate subAggregate, TableQuery parent)
        {
            return new Aggregate(AggregateFunction.Convert, subAggregate, parent);
        }

        #endregion

        #region Convert To

        public Aggregate ToVarchar()
        {
            this.TypeToConvert = DbType.AnsiString;
            return this;
        }

        public Aggregate ToVarchar(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.AnsiString;
            return this;
        }

        public Aggregate ToChar()
        {
            this.TypeToConvert = DbType.AnsiStringFixedLength;
            return this;
        }

        public Aggregate ToChar(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.AnsiStringFixedLength;
            return this;
        }

        public Aggregate ToNVarchar()
        {
            this.TypeToConvert = DbType.String;
            return this;
        }

        public Aggregate ToNVarchar(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.String;
            return this;
        }

        public Aggregate ToNChar()
        {
            this.TypeToConvert = DbType.StringFixedLength;
            return this;
        }

        public Aggregate ToNChar(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.StringFixedLength;
            return this;
        }



        public Aggregate ToBinary()
        {
            this.TypeToConvert = DbType.Binary;
            return this;
        }

        public Aggregate ToBinary(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Binary;
            return this;
        }

        public Aggregate ToBoolean()
        {
            this.TypeToConvert = DbType.Boolean;
            return this;
        }

        public Aggregate ToBoolean(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Boolean;
            return this;
        }

        public Aggregate ToByte()
        {
            this.TypeToConvert = DbType.Byte;
            return this;
        }

        public Aggregate ToByte(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Byte;
            return this;
        }

        public Aggregate ToCurrency()
        {
            this.TypeToConvert = DbType.Currency;
            return this;
        }

        public Aggregate ToCurrency(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Currency;
            return this;
        }

        public Aggregate ToDate()
        {
            this.TypeToConvert = DbType.Date;
            return this;
        }

        public Aggregate ToDate(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Date;
            return this;
        }

        public Aggregate ToDateTime()
        {
            this.TypeToConvert = DbType.DateTime;
            return this;
        }

        public Aggregate ToDateTime(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.DateTime;
            return this;
        }

        public Aggregate ToDateTime2()
        {
            this.TypeToConvert = DbType.DateTime2;
            return this;
        }

        public Aggregate ToDateTime2(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.DateTime2;
            return this;
        }

        public Aggregate ToDateTimeOffset()
        {
            this.TypeToConvert = DbType.DateTimeOffset;
            return this;
        }

        public Aggregate ToDateTimeOffset(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.DateTimeOffset;
            return this;
        }

        public Aggregate ToDecimal()
        {
            this.TypeToConvert = DbType.Decimal;
            return this;
        }

        public Aggregate ToDecimal(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Decimal;
            return this;
        }

        public Aggregate ToDouble()
        {
            this.TypeToConvert = DbType.Double;
            return this;
        }

        public Aggregate ToDouble(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Double;
            return this;
        }

        public Aggregate ToGuid()
        {
            this.TypeToConvert = DbType.Guid;
            return this;
        }

        public Aggregate ToGuid(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Guid;
            return this;
        }

        public Aggregate ToSmallInt()
        {
            this.TypeToConvert = DbType.Int16;
            return this;
        }

        public Aggregate ToSmallInt(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Int16;
            return this;
        }

        public Aggregate ToInt()
        {
            this.TypeToConvert = DbType.Int32;
            return this;
        }

        public Aggregate ToInt(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Int32;
            return this;
        }

        public Aggregate ToLong()
        {
            this.TypeToConvert = DbType.Int64;
            return this;
        }

        public Aggregate ToLong(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Int64;
            return this;
        }

        public Aggregate ToObject()
        {
            this.TypeToConvert = DbType.Object;
            return this;
        }

        public Aggregate ToObject(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Object;
            return this;
        }

        public Aggregate ToSByte()
        {
            this.TypeToConvert = DbType.SByte;
            return this;
        }

        public Aggregate ToSByte(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.SByte;
            return this;
        }

        public Aggregate ToFloat()
        {
            this.TypeToConvert = DbType.Single;
            return this;
        }

        public Aggregate ToFloat(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Single;
            return this;
        }

        public Aggregate ToTime()
        {
            this.TypeToConvert = DbType.Time;
            return this;
        }

        public Aggregate ToTime(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Time;
            return this;
        }

        public Aggregate ToUInt16()
        {
            this.TypeToConvert = DbType.UInt16;
            return this;
        }

        public Aggregate ToUInt16(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.UInt16;
            return this;
        }

        public Aggregate ToUInt32()
        {
            this.TypeToConvert = DbType.UInt32;
            return this;
        }

        public Aggregate ToUInt32(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.UInt32;
            return this;
        }

        public Aggregate ToUInt64()
        {
            this.TypeToConvert = DbType.UInt64;
            return this;
        }

        public Aggregate ToUInt64(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.UInt64;
            return this;
        }

        public Aggregate ToVarNumeric()
        {
            this.TypeToConvert = DbType.VarNumeric;
            return this;
        }

        public Aggregate ToVarNumeric(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.VarNumeric;
            return this;
        }

        public Aggregate ToXml()
        {
            this.TypeToConvert = DbType.Xml;
            return this;
        }

        public Aggregate ToXml(string type)
        {
            this.ConvertType = type;
            this.TypeToConvert = DbType.Xml;
            return this;
        }

        #endregion

        public static class ConvertTypes
        {
            ///<sumary>
            ///Padrão:EUA
            ///Entrada/Saída:mm/dd/aaaa
            ///</sumary>
            public const int N_1 = 1;

            ///<sumary>
            ///Padrão:ANSI
            ///Entrada/Saída:aa.mm.dd
            ///</sumary>
            public const int N_2 = 2;

            ///<sumary>
            ///Padrão:Britânico/francês
            ///Entrada/Saída:dd/mm/aaaa
            ///</sumary>
            public const int N_3 = 3;

            ///<sumary>
            ///Padrão:Alemão
            ///Entrada/Saída:dd.mm.aa
            ///</sumary>
            public const int N_4 = 4;

            ///<sumary>
            ///Padrão:Italiano
            ///Entrada/Saída:dd-mm-aa
            ///</sumary>
            public const int N_5 = 5;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:dd mês aa
            ///</sumary>
            public const int N_6 = 6;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:Mês dd, aa
            ///</sumary>
            public const int N_7 = 7;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:hh:mi:ss
            ///</sumary>
            public const int N_8 = 8;

            ///<sumary>
            ///Padrão:EUA
            ///Entrada/Saída:mm-dd-aa
            ///</sumary>
            public const int N_10 = 10;

            ///<sumary>
            ///Padrão:JAPÃO
            ///Entrada/Saída:aa/mm/dd
            ///</sumary>
            public const int N_11 = 11;

            ///<sumary>
            ///Padrão:ISO
            ///Entrada/Saída:aammdd
            ///</sumary>
            public const int N_12 = 12;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:hh:mi:ss:mmm(24h)
            ///</sumary>
            public const int N_14 = 14;

            ///<sumary>
            ///Padrão:Padrão
            ///Entrada/Saída:mês dd aaaa hh:miAM (ou PM)
            ///</sumary>
            public const int N_100 = 100;

            ///<sumary>
            ///Padrão:EUA
            ///Entrada/Saída:mm/dd/aaaa
            ///</sumary>
            public const int N_101 = 101;

            ///<sumary>
            ///Padrão:ANSI
            ///Entrada/Saída:aa.mm.dd
            ///</sumary>
            public const int N_102 = 102;

            ///<sumary>
            ///Padrão:Britânico/francês
            ///Entrada/Saída:dd/mm/aaaa
            ///</sumary>
            public const int N_103 = 103;

            ///<sumary>
            ///Padrão:Alemão
            ///Entrada/Saída:dd.mm.aa
            ///</sumary>
            public const int N_104 = 104;

            ///<sumary>
            ///Padrão:Italiano
            ///Entrada/Saída:dd-mm-aa
            ///</sumary>
            public const int N_105 = 105;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:dd mês aa
            ///</sumary>
            public const int N_106 = 106;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:Mês dd, aa
            ///</sumary>
            public const int N_107 = 107;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:hh:mi:ss
            ///</sumary>
            public const int N_108 = 108;

            ///<sumary>
            ///Padrão:Padrão + milissegundos
            ///Entrada/Saída:mês dd aaaa hh:mi:ss:mmmAM (ou PM)
            ///</sumary>
            public const int N_109 = 109;

            ///<sumary>
            ///Padrão:EUA
            ///Entrada/Saída:mm-dd-aa
            ///</sumary>
            public const int N_110 = 110;

            ///<sumary>
            ///Padrão:JAPÃO
            ///Entrada/Saída:aa/mm/dd
            ///</sumary>
            public const int N_111 = 111;

            ///<sumary>
            ///Padrão:ISO
            ///Entrada/Saída:aaaammdd
            ///</sumary>
            public const int N_112 = 112;

            ///<sumary>
            ///Padrão:Padrão Europa + milissegundos
            ///Entrada/Saída:dd mês aaaa hh:mi:ss:mmm (24h)
            ///</sumary>
            public const int N_113 = 113;

            ///<sumary>
            ///Padrão:-
            ///Entrada/Saída:hh:mi:ss:mmm(24h)
            ///</sumary>
            public const int N_114 = 114;

            ///<sumary>
            ///Padrão:ODBC canônico
            ///Entrada/Saída:aaaa-mm-dd hh:mi:ss(24h)
            ///</sumary>
            public const int N_120 = 120;

            ///<sumary>
            ///Padrão:ODBC canônico (com milissegundos)
            ///Entrada/Saída:aaaa-mm-dd hh:mi:ss.mmm(24h)
            ///</sumary>
            public const int N_121 = 121;

            ///<sumary>
            ///Padrão:ISO8601
            ///Entrada/Saída:aaaa-mm-ddThh:mi:ss.mmm (sem espaços)
            ///</sumary>
            public const int N_126 = 126;

            ///<sumary>
            ///Padrão:ISO8601 com fuso horário Z.
            ///Entrada/Saída:aaaa-mm-ddThh:mi:ss.mmmZ (sem espaços)
            ///</sumary>
            public const int N_127 = 127;

            ///<sumary>
            ///Padrão:Islâmico (5)
            ///Entrada/Saída:dd mmm aaaa hh:mi:ss:mmmAM
            ///</sumary>
            public const int N_130 = 130;

            ///<sumary>
            ///Padrão:Islâmico (5)
            ///Entrada/Saída:dd/mm/aa hh:mi:ss:mmmAM
            ///</sumary>
            public const int N_131 = 131;

        }
    }
}
