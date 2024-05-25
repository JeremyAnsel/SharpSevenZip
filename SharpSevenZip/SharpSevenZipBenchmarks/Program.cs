using BenchmarkDotNet.Running;
using SharpSevenZipBenchmarks;

var summary = BenchmarkRunner.Run<Benchmarks>();

Console.WriteLine("Press any key to end");
Console.ReadKey();
