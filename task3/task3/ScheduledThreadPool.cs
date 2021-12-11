using System;

namespace task3
{
    public class ScheduledThreadPool
    {
        // инициализация
        public ScheduledThreadPool() { }

        // выполнение задачи без условий
        public void SubmitCommon(Action<object?> task, object? arg) { }

        // выполнение задачи после по истечению времени delay
        public void SubmitDelayed(Action<object?> task, object? arg, TimeSpan delay) { }

        // выполнение задачи через каждое period количество времени
        public void SubmitPeriodic(Action<object?> task, object? arg, TimeSpan period) { }
    
        // выполнение задачи через интервал interval
        public void SubmitInterval(Action<object?> task, object? arg, TimeSpan interval) { }

    }
}