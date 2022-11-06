
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

        public void StartTimer()
        {
            
        }
        
        public void Start()
        {
            while (isEnabled)
            {
                ticks++;
                try
                {
                    //if (ticks % 21 == 0)
                    //    Scan();
                    if (ticks % 30 == 0)
                    {
                        isEnabled = false;
                        Watch(1000);
                        Reconnect();
                    }

                    ChoosePvpAndJoin();

                    AvoidAfkAction();

                    Watch(1000);
                }
                catch (Exception e)
                {
                    Watch(2000);
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
        //出门
        private void OutDoor()
        {
            for (int i = 0; i < 10; i++)
            {
                WowProcess.PressKeys(ConsoleKey.W);
                Watch(100);
            }
        }
        //防暂离
        private void AvoidAfkAction()
        {
            if (ticks % 4 == 0) {
                WowProcess.PressKeys(ConsoleKey.F);
                Watch(500);
                WowProcess.PressKeys(ConsoleKey.Spacebar);
                Watch(1000);
                WowProcess.PressKeys(ConsoleKey.Z);
                Watch(500);
            }
            if (ticks % 15 == 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    WowProcess.KeysDown(ConsoleKey.W);
                    Watch(200);
                }
                WowProcess.KeysUp(ConsoleKey.W);
            }
        }
        //重新连接
        private void Reconnect()
        {
            WowProcess.PressKeys(ConsoleKey.Enter);
            Watch(500);
            WowProcess.PressKeys(ConsoleKey.Enter);
            Watch(6000);
            WowProcess.PressKeys(ConsoleKey.Enter);
            WowProcess.PressKeys(ConsoleKey.Enter);
            isEnabled = true;
        }

        public void SetCastKey(ConsoleKey castKey)
        {
            this.castKey = castKey;
        }

        private void Watch(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public void Stop()
        {
            isEnabled = false;
            ticks = 0;
        }

        public void Test()
        {
            Watch(2000);
            WowProcess.LeftClickMousePos(new Point(780,37));
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