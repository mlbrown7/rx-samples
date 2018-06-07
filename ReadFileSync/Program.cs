using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadFileSync
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource _cts = new CancellationTokenSource();

            var observable = ReadFile(_cts.Token);
            observable.Subscribe(
                onNext: (line) => Console.WriteLine(line),
                onCompleted: _cts.Cancel,
                onError: (ex) => Console.WriteLine(ex.ToString())
            );

            Console.WriteLine("Running, enter 'Q' to quit.");      //does not get here until completed
            var input = Console.ReadKey(false);
            while (input.KeyChar != 'q' && input.KeyChar != 'Q')
            {
                input = Console.ReadKey(false);
            }

            Console.WriteLine("Stopping..");
            _cts.Cancel();
            Console.WriteLine("Stopped!!!");
            Console.ReadLine();
        }


        static IObservable<string> ReadFile(CancellationToken cancellationToken)
        {
            string filename = @"D:\temp\RT Client Test Job 1.csv";

            return Observable.Create<string>(observer =>
            {
                Console.WriteLine("Start read file.");
                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        if (cancellationToken.IsCancellationRequested) { break; }

                        var line = reader.ReadLine();
                        observer.OnNext(line);

                        if (cancellationToken.IsCancellationRequested) { break; }
                    }
                }
                Console.WriteLine("End read file.");

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
    }
}
