using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService_Exercise
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer;
        static int count = 0;
        //private Thread thread1 = new Thread(new ThreadStart(write_Txt));
        //private Thread thread2 = new Thread(new ThreadStart(Empty_RecycleBin));
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Interval = 5000;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(write_Txt);

        }

        protected override void OnStop()
        {
        }

        private void write_Txt(object sender,EventArgs e)
        {
            count++;
            string txt_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/serviceDeneme.txt";
            

            FileStream fs = new FileStream(txt_path, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);
            //Yazma işlemi için bir StreamWriter nesnesi oluşturduk.
            sw.WriteLine(count + ". Çalışma");
            sw.Flush();
            //Veriyi tampon bölgeden dosyaya aktardık.
            sw.Close();
            fs.Close();

           // Delete_Txt();
        }
        private void Delete_Txt()
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/serviceDeneme.txt");
        }

        private void Empty_RecycleBin()
        {
            SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlag.SHERB_NOSOUND | RecycleFlag.SHERB_NOCONFIRMATION);
        }

        [DllImport("Shell32.dll")]

        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);

        enum RecycleFlag : int

        {

            SHERB_NOCONFIRMATION = 0x00000001, // No confirmation, when emptying

            SHERB_NOPROGRESSUI = 0x00000001, // No progress tracking window during the emptying of the recycle bin

            SHERB_NOSOUND = 0x00000004 // No sound when the emptying of the recycle bin is complete

        }


    }
}
