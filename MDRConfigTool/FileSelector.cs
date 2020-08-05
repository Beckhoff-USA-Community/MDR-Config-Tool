﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDRConfigTool
{
    public partial class FileSelector : Form
    {
        OpenFileDialog ofd;
        ExcelHandler fileReader;
        Timer timer = new Timer();
        SolutionHandler solHandler;
        private  int count = 0;
        private DataTable DT;
        public FileSelector()
        {
            InitializeComponent();

            

        }
        private void btnStart_Click(object sender, EventArgs e)
        {

            pnlWelcome.Visible = false;
            pnlFileSelect.Visible = true;
        }//btnOpenSolution_Click()

        private void FolderBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = fbd.SelectedPath;
            }
            /*
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select I/O Spreadsheet File";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "All Files(*.*)|*.*|Excel Spreadsheets (*.xls,*.xlsm, *.xlsx)|*.xls;*.xlsm;*.xlsx| Text File (*.txt, *.csv)|*.txt;*.csv";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = ofd.FileName;
            }
            */

        }//FolderBrowse_Click()

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            string file;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select I/O Spreadsheet File";
            ofd.InitialDirectory = tbFilePath.Text;
            ofd.Filter = "All Files(*.*)|*.*|Excel Spreadsheets (*.xls,*.xlsm, *.xlsx)|*.xls;*.xlsm;*.xlsx| Text File (*.txt, *.csv)|*.txt;*.csv";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                file = ofd.FileName;
                int idx = file.LastIndexOf('\\');
                file = file.Substring(idx + 1);
                tbFileName.Text = file;
            }


        }//btnBrowseFile_Click()

        private void btnOPenFile_Click(object sender, EventArgs e)
        {
            
            string sFilePath = tbFilePath.Text + @"\" + tbFileName.Text;
            if (!String.IsNullOrEmpty(tbProjectName.Text) && !String.IsNullOrEmpty(tbFileName.Text))
            {

                fileReader = new ExcelHandler(sFilePath);
                DT = fileReader.RetrieveData();
                fileReader.closeFile();

             
                solHandler = new SolutionHandler(tbFilePath.Text, tbProjectName.Text);


                pnlFileSelect.Visible = false; 
                dgvListDisplay.DataSource = DT;
                pnlDataDisplay.Visible = true;
            }
            else { MessageBox.Show("Please Enter a Project Name and/or File Name"); }

        }//btnOpenFile_Click

        private void btnTableOK_Click(object sender, EventArgs e)
        {

            pnlDataDisplay.Visible = false;
            pnlSolutionsettings.Visible = true;

        }//btnTableOK_Click


        private void btnOpenSolution_Click(object sender, EventArgs e)
        {
            
            solHandler.SetNetId(tbAmsNetId.Text);
            //solHandler.ScanDevicesAndBoxes(DT);
            solHandler.PLCdeclarations(DT);
            solHandler.linkVariables(DT);
            //solHandler.ActivateConfiguration();

            pnlSolutionsettings.Visible = false;

        }//btnOpenSolution_Click()

   






    }
}
