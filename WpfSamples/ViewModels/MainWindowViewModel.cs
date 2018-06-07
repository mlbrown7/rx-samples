using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive;
using ReactiveUI;
using WpfSamples.Views;

namespace WpfSamples.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private CancellationTokenSource _cts;

        public MainWindowViewModel()
        {
            Rows = new ReactiveList<string>();
            _cts = new CancellationTokenSource();
            System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] UI.");

            GridViewSamplesCommand = ReactiveCommand.Create(() =>
            {
                var window = new GridEditSample();
                window.DataContext = new GridEditSampleViewModel();
                window.ShowDialog();
            });

            ReadFileCommand = ReactiveCommand.CreateFromObservable(() => ReadFile(FileName, _cts.Token).SubscribeOn(RxApp.TaskpoolScheduler));  //this runs the observable code on the thread pool

            System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] ctor()");

            StopCommand = ReactiveCommand.Create(() =>
            {
                _cts.Cancel();
            });

            //ObserveOnDispatcher means the subscription code will run on the ui thread
            ReadFileCommand.ObserveOnDispatcher().Subscribe(
                onNext: (line) =>
                {
                    _rows.Insert(0, line);
                    System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] Update UI");
                },
                onCompleted: () =>
                {
                    System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] Completed");
                });

            ReadFileCommand.Finally(() =>
            {
                System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] Finally");
            });

            //var fileObservable = Observable.Create<string>((observer) => ReadyFileAsync(observer, FileName, _cts.Token)).SubscribeOn(RxApp.TaskpoolScheduler);

            //ReadFileCommand = ReactiveCommand.CreateFromObservable(() => fileObservable);
            //ReadFileCommand.ObserveOnDispatcher().Subscribe((line) =>
            //{
            //    _rows.Insert(0, line);
            //    System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] {line}");
            //});

            //ReadFileCommand = ReactiveCommand.CreateFromObservable(() => ReadFile(FileName, _cts.Token)  .SubscribeOn(RxApp.TaskpoolScheduler));
            //ReadFileCommand.ObserveOnDispatcher().Subscribe((line) =>
            //    {
            //        _rows.Insert(0, line);
            //        System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] {line}");

            //    });

            //ReadFileCommand.Subscribe(
            //    onNext: line =>
            //    {
            //        try
            //        {
            //                System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] Rows Collection Insert.");
            //                _rows.Insert(0, line);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.ToString());
            //        }
            //    },
            //        onError: ex =>
            //        {
            //            MessageBox.Show(ex.ToString());
            //        },
            //    onCompleted: () => MessageBox.Show("Read File Completed")
            //    );


            FileName = @"d:\temp\RT Client Test Job 1.csv";
        }

        private string _fileNmme;
        public string FileName { get => _fileNmme; set => this.RaiseAndSetIfChanged(ref _fileNmme, value); }


        private ReactiveList<string> _rows;
        public ReactiveList<string> Rows { get => _rows; set => this.RaiseAndSetIfChanged(ref _rows, value); }


        //public ReactiveCommand ReadFileCommand { get; set; }
        public ReactiveCommand<Unit, string> ReadFileCommand { get; set; }

        public ReactiveCommand StopCommand { get; set; }

        public ReactiveCommand GridViewSamplesCommand { get; set; }


        //private Task ReadyFileAsync(IObserver<string> observer, string filename, CancellationToken cancellationToken)
        //{
        //    return Task.Run(() =>
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] Read file.");
        //        using (var reader = new StreamReader(filename))
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                if (cancellationToken.IsCancellationRequested) { break; }

        //                var line = reader.ReadLine();
        //                observer.OnNext(line);
        //                System.Threading.Thread.Sleep(1000);

        //                if (cancellationToken.IsCancellationRequested) { break; }
        //            }
        //        }
        //        observer.OnCompleted();
        //    });
        //}


        private IObservable<string> ReadFile(string filename, CancellationToken cancellationToken)
        {
            return Observable.Create<string>(observer =>
            {
                System.Diagnostics.Debug.WriteLine($"Thread[{System.Threading.Thread.CurrentThread.ManagedThreadId}] Read file.");
                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        if (cancellationToken.IsCancellationRequested) { break; }

                        var line = reader.ReadLine();
                        observer.OnNext(line);
                        System.Threading.Thread.Sleep(1000);

                        if (cancellationToken.IsCancellationRequested) { break; }
                    }
                }

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
    }
}
