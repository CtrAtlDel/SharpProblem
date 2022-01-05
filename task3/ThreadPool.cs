using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace task4
{
    public class ThreadPool
    {
        private ConcurrentQueue<(Action<object?> Action, object? Value)> _queue;
        private List<Thread> _threads;
        private CountdownEvent _waitGroup;
        private bool _isAlive;

        public ThreadPool(int size) // количество потоков
        {
            this._queue = new ConcurrentQueue<(Action<object?> Action, object? Value)>();
            this._threads = Enumerable.Repeat(0, size).Select(
                _ => new Thread(this.Working)
                {
                    IsBackground = true
                }
            ).ToList();

            this._waitGroup = new CountdownEvent(size);
            this._isAlive = true;
            foreach (var thread in this._threads)
            {
                thread.Start(); // запускаем потоки
            }
        }

        public void Submit(Action action)
        {
            this._queue.Enqueue((_ => action.Invoke(), null));
        }

        public void Submit(Action<object?> action, object? value)
        {
            this._queue.Enqueue((action, value));
        }

        private void Working()
        {
            while (this._isAlive)
            {
                var ok = this._queue.TryDequeue(out var entry);
                if (ok)
                {
                    entry.Action.Invoke(entry.Value);
                }
            }

            this._waitGroup.Signal();
        }

        public void Kill() //переводим потоки в спящий режим
        {
            this._isAlive = false;
            this._waitGroup.Wait();
        }
    }
}