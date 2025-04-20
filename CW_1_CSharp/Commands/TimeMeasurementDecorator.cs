using System;
using System.Diagnostics;

namespace CW_1_CSharp.Commands
{
    public class TimeMeasurementDecorator : ICommand
    {
        private readonly ICommand _innerCommand;

        public TimeMeasurementDecorator(ICommand innerCommand)
        {
            _innerCommand = innerCommand;
        }

        public void Execute()
        {
            var sw = Stopwatch.StartNew();
            _innerCommand.Execute();
            sw.Stop();
            Console.WriteLine($"Время выполнения: {sw.Elapsed.TotalSeconds:0.000000} сек.");
        }
    }
}