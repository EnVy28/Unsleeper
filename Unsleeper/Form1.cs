using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace Unsleeper
{
    public partial class Form1 : Form
    {
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        bool started = false;
        Stopwatch watch = new Stopwatch();
        Thread timer;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SetThreadExecutionState([In] uint esFlags);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (started)
            {
                started = false;
                StartBtn.Text = "Start";
                SetThreadExecutionState(ES_CONTINUOUS);
                watch.Stop();
                timer.Abort();
            }
            else
            {
                started = true;
                StartBtn.Text = "Stop";
                SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED);
                watch.Start();

                timer = new Thread(new ThreadStart(new Action(() => {
                    int sec = 0;
                    int min = 0;
                    int hour = 0;

                    while (true)
                    {
                        Thread.Sleep(1000);
                        if (sec == 59) {
                            sec = 0;
                            if (min == 59)
                            {
                                min = 0;
                                hour++;
                            }
                            else
                            {
                                min++;
                            }
                        }
                        else
                        {

                            sec++;
                        }

                        this.BeginInvoke((MethodInvoker) delegate () {
                            label1.Text = hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
                        });
                    }
                })));
                timer.Start();
            }
        }
    }
}