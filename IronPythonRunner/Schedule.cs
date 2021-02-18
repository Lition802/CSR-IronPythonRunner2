using System.Threading;

namespace IronPythonRunner
{
    public class Schedule
    {
        private Thread t { get; set; }
        /// <summary>
        /// 获得该scheduleid
        /// </summary>
        public int ScheduleID
        {
            get
            {
                return t.ManagedThreadId;
            }
        }
        /// <summary>
        /// 延时任务
        /// </summary>
        /// <param name="func">py函数</param>
        /// <param name="delay">延时</param>
        /// <param name="cycle">重复次数</param>
        public Schedule(dynamic func, int delay, int cycle)
        {
            int tmp = 0;
            t = new Thread(() =>
            {
                while (tmp != cycle)
                {
                    Thread.Sleep(delay);
                    func();
                    tmp++;
                }
            });
            t.Start();
        }
        /// <summary>
        /// 延时任务，重复次数无限
        /// </summary>
        /// <param name="func">函数</param>
        /// <param name="delay">延时</param>
        public Schedule(dynamic func, int delay)
        {
            t = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(delay);
                    func();
                }
            });
            t.Start();
        }
        public void Stop()
        {
            t.Abort();
        }
    }
}
