using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using AutoMapper;
using AutoMapper.Data;
using ClickHouse.Ado;

namespace ClickHouseQueryProvider
{
    /*var command = cnn.CreateCommand(@"create table testTable1
                                    (
                                      id      Int32,
                                      name    String,
                                      surname String
                                    )
                                    ENGINE = Memory
                                    ");*/

    class Program
    {
        private const string ConnectionString =
            "Host=localhost;Port=9000;User=default;Password=;SocketTimeout=600000;Database=test";

        static void Main(string[] args)
        {
            var operation = new SelectOperation();
            operation.Get();
        }

        public class TestTable
        {
            public string some_str { get; set; }
        }

        public abstract class DatabaseOperation
        {
            public abstract string Query { get; }

            public void Get()
            {
                var settings = new ClickHouseConnectionSettings(ConnectionString);
                using (var cnn = new ClickHouseConnection(settings))
                {
                    cnn.Open();
                    using (var reader = cnn.CreateCommand(Query).ExecuteReader())
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                var ssss = reader;
                                var sss = Convert.ToString(reader["String"]);
                                var s = Convert.ToInt64(reader["Int"]);
                                var ss = Convert.ToDateTime(reader["Date"]);
                                var fss = Convert.ToSingle(reader["Float"]);
                            }
                        } while (reader.NextResult());
                    }
                }
            }
        }

        public class InsertOperation : DatabaseOperation
        {
            public override string Query => "insert into TestTable values (1,\"name\",\"surname\")";
        }

        public class SelectOperation : DatabaseOperation
        {
            public override string Query => "select String,Int,Float,Date from TestTable";
        }

        private static void PrintData(IDataReader reader)
        {
            do
            {
                Console.Write("Fields: ");
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write("{0}:{1} ", reader.GetName(i), reader.GetDataTypeName(i));
                }

                Console.WriteLine();
                while (reader.Read())
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var val = reader.GetValue(i);
                        if (val.GetType().IsArray)
                        {
                            Console.Write('[');
                            Console.Write(string.Join(", ", ((IEnumerable) val).Cast<object>()));
                            Console.Write(']');
                        }
                        else
                        {
                            Console.Write(val);
                        }

                        Console.Write(", ");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            } while (reader.NextResult());
        }
    }
}