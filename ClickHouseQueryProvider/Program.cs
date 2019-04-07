using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using AutoMapper;
using AutoMapper.Data;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using ClickHouse.Ado;
using UnitTests.TestDbContext.AdditionBenchmarks;
using UnitTests.TestDbContext.BenchmarkTests;

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
            "Host=localhost;Port=9000;User=default;Password=;SocketTimeout=600000;Database=datasets";

        static void Main(string[] args)
        {
//            var s = BenchmarkRunner.Run<YandexMetrikaBenchmarkTests>(new AllowNonOptimized());
            var s = BenchmarkRunner.Run<AdditionBenchmarks>(new AllowNonOptimized());
        }

        private class AllowNonOptimized : ManualConfig
        {
            public AllowNonOptimized()
            {
                Add(JitOptimizationsValidator.DontFailOnError); // ALLOW NON-OPTIMIZED DLLS
                Add(DefaultConfig.Instance.GetLoggers().ToArray()); // manual config has no loggers by default
                Add(DefaultConfig.Instance.GetExporters().ToArray()); // manual config has no exporters by default
                Add(DefaultConfig.Instance.GetColumnProviders().ToArray()); // manual config has no columns by default
            }
        }
    }
}