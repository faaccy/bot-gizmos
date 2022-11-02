
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace Bot.Core
{
    public class PvpBot
    {
        ConsoleKey castKey = ConsoleKey.Enter;
        private Stopwatch stopwatch = new Stopwatch();
        private static Random random = new Random();
        private bool isEnabled = true;
        private int ticks = 0;
        public PvpBot()
        {
        }
        public void Start()
        {
            isEnabled = true;
            while (isEnabled)
            {
                ticks++;
                try
                {
                    ChoosePvpAndJoin();
                    AvoidAfkAction();
                    if (ticks % 10 == 0)
                        Scan();
                    if (ticks % 45 == 0)
                        Reconnect();
                    Watch(1000);
                }
                catch (Exception e)
                {
                    Sleep(2000);
                }
            }

        }
        private void ChoosePvpAndJoin()
        {
            WowProcess.PressKeys(ConsoleKey.T);
        }
        //获取鼠标相对窗体的位置
        private Point GetCurrentPoint()
        {
            var oldPosition = System.Windows.Forms.Cursor.Position;
            return oldPosition;
        }
    
        private void Scan()
        {
            //扇形扫描目标敌人
            WowProcess.PressKeys(ConsoleKey.A);
        }
        //防暂离
        private void AvoidAfkAction()
        {
            WowProcess.PressKeys(ConsoleKey.F);
            Watch(50);
            WowProcess.PressKeys(ConsoleKey.Spacebar);
            Watch(500);
        }
        //重新连接
        private void Reconnect()
        {
            WowProcess.PressKeys(ConsoleKey.Enter);
            Watch(5);
            WowProcess.PressKeys(ConsoleKey.Enter);
            Watch(1000);
            WowProcess.PressKeys(ConsoleKey.Enter);
            WowProcess.PressKeys(ConsoleKey.Enter);
        }

        public void SetCastKey(ConsoleKey castKey)
        {
            this.castKey = castKey;
        }

        private void Watch(int milliseconds)
        {
           
            stopwatch.Reset();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < milliseconds)
            {
                Console.WriteLine("53");
            }
            stopwatch.Stop();
        }

        public void Stop()
        {
            isEnabled = false;
        }

        private DateTime StartTime = DateTime.Now;
        public static void Sleep(int ms)
        {
            ms+=random.Next(0, 225);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed.TotalMilliseconds < ms)
            {
                Thread.Sleep(100);
            }
        }
    }
}