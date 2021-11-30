using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using TCatSysManagerLib;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using TwinCAT.Ads;

namespace MDRConfigTool
{
    public class Program
    {
        private int iNumber;

        [STAThread]
        public static void Main(string[] args)
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FileSelector fSelect = new FileSelector();
            Application.Run(fSelect);

            /*----------------------------------------------
             * This is used for testing.
             *
             * ---------------------------------------------*/
            //SolutionHandler solH = new SolutionHandler();

        }

        public void helloWorld()
        {
            MessageBox.Show("Hello World");
        }



       
    }//class
}
