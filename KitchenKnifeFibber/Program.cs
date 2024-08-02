using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KitchenKnifeFibber 
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (
                args.Length != 2
                || !int.TryParse(args[0], out int startRange)
                || !int.TryParse(args[1], out int endRange)
            )
            {
                Console.WriteLine("Usage: program.exe <startRange> <endRange>");
                return;
            }

            if (startRange < 0 || endRange < 0 || startRange > endRange)
            {
                Console.WriteLine(
                    "Please provide valid non-negative ranges with startRange <= endRange."
                );
                return;
            }

            Console.WriteLine("Generating Fibonacci numbers...");

            List<BigInteger>? fibonacciNumbers = await GenerateFibonacciRangeAsync(
                startRange: startRange,
                endRange: endRange
            );

            StringBuilder output = new StringBuilder();

            Console.WriteLine($"Fibonacci numbers from {startRange} to {endRange}:");

            await Task.Run(() =>
            {
                foreach (var number in fibonacciNumbers)
                {
                    output.AppendLine(number.ToString());
                }
            });

            await Task.Run(() => Console.WriteLine(output.ToString()));
        }

        static async Task<List<BigInteger>> GenerateFibonacciRangeAsync(
            int startRange,
            int endRange
        )
        {
            ConcurrentBag<Task<BigInteger>> tasks = new ConcurrentBag<Task<BigInteger>>();

            Parallel.For(
                startRange,
                endRange + 1,
                i =>
                {
                    tasks.Add(Task.Run(() => Fibonacci(i)));
                }
            );

            BigInteger[] results = await Task.WhenAll(tasks);
            return new List<BigInteger>(results);
        }

        static BigInteger Fibonacci(int n)
        {
            if (n <= 1)
                return n;

            BigInteger a = 0;
            BigInteger b = 1;

            for (int i = 2; i <= n; i++)
            {
                BigInteger c = a + b;
                a = b;
                b = c;
            }

            return b;
        }
    }
}
