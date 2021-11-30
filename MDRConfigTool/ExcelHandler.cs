using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace MDRConfigTool
{
    class ExcelHandler
    {
        private Excel.Application _app;
        private Excel.Workbooks _workbooks;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _worksheet;

        //base contructor for the Excel file handler, in this scenario no file was selected, so a new worksheet is opened
        //As of May 25th, this contructor should never be called. This is here in case we want to give the option to a customer to fill out the worksheet
        //after opening the Config tool
        public ExcelHandler()
        {
            this._app = new Excel.Application();
            this._workbooks = this._app.Workbooks;
            this._workbook = this._workbooks.Add();
            this._app.Visible = true;
        }//Constructor()

        //This is the constructor that our form calls after the user selects a file.
        //It simply calls this instances version of the OpenFile method, passing the String agrument along.
        public ExcelHandler(string File)
        {
            this.Openfile(File);

        }//Constructor(string)

        public DataTable RetrieveData() {

            
            //get the current worksheet from the workbook, and get its active range.
            //The template provided by Garth Gaddy only has 3 columns, but we will assume the customer does not folow the
            //Template exactly and get the number of columns
            _worksheet = _workbook.ActiveSheet;
            int nColumns = _worksheet.UsedRange.Columns.Count;
            int nRows = _worksheet.UsedRange.Rows.Count;

            //create a new DataTable object. The DT holds data in a similar map as the Spreadsheet, and is easy to translate for the user.
            DataTable DT = new DataTable();

            Excel.Range headerRange = _worksheet.get_Range((Excel.Range)_worksheet.Cells[1, 1], (Excel.Range)_worksheet.Cells[1, nColumns]);
            Excel.Range dataRange = _worksheet.get_Range((Excel.Range)_worksheet.Cells[2,1], (Excel.Range)_worksheet.Cells[nRows,nColumns]);
            
            for (int i = 1; i <= nColumns; i++)
            {
                DT.Columns.Add(headerRange.Columns[i].Value);

            }
            AddData(nColumns, nRows, dataRange, DT);



            return DT;
           
        }//RetriveData();

        public void Openfile(string file)
        {
            this._app = new Excel.Application();
            this._workbooks = this._app.Workbooks;
            if (!string.IsNullOrEmpty(file))
            {
                if (file.Contains(".csv") || file.Contains(".txt"))
                {  this._workbooks.OpenText(file, 2, 1, 1, Excel.XlTextQualifier.xlTextQualifierNone, false, false, false, true);
                    this._workbook = this._app.ActiveWorkbook;
                }
                else if (file.Contains(".xls") || file.Contains(".xlms") || file.Contains(".xlsx"))
                {
                    this._workbook = this._workbooks.Open(file);
                }
            }
        }

        private void AddData(int cCount, int rCount, Excel.Range range, DataTable DT)
        {
            DataRow row;
            object[,] data = range.Value2;
            for(int j = 1; j <= cCount;j++)
            for (int i = 1; i < rCount; i++)
            {
                string CellVal = String.Empty;
               
                    CellVal = (string)data[i, j];
               
                   
                        if (j == 1)
                        {
                            row = DT.NewRow();
                            row[j - 1] = CellVal;
                            DT.Rows.Add(row);
                        }
                        else
                        {
                            row = DT.Rows[i - 1];
                            row[j - 1] = CellVal;
                        }
                    
            }
        }

        public void WritetoExcel(DataTable DT, string FilePath = null)
        {

            int iColumnCount;
            try
            {
                if (DT == null || (iColumnCount = DT.Columns.Count) == 0)
                {
                    throw new Exception("Export to Excel filed: The table is either null or empty!\n");
                }
                //creat an Excel Application instance
                var excelApp = new Excel.Application();

                //add a workbook to the Excel workbook
                excelApp.Workbooks.Add();

                //create a single worksheet
                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
                object[] Header = new object[iColumnCount];

                //create column headings and add them to the worksheet
                iColumnCount = DT.Columns.Count;
                for (int i = 0; i < iColumnCount; i++)
                {
                    Header[i] = DT.Columns[i].ColumnName;
                }
                Excel.Range HeaderRange = workSheet.get_Range((Excel.Range)(workSheet.Cells[1, 1]), (Excel.Range)(workSheet.Cells[1, iColumnCount]));
                HeaderRange.Value = Header;
                HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                HeaderRange.Font.Bold = true;

                //Get Data Cells
                int iRowCount = DT.Rows.Count;
                object[,] Cells = new object[iRowCount, iColumnCount];
                for (int i = 0; i < iRowCount; i++)
                    for (int j = 0; j < iColumnCount; j++)
                        Cells[i, j] = DT.Rows[i][j];

                workSheet.get_Range((Excel.Range)(workSheet.Cells[2, 1]), (Excel.Range)(workSheet.Cells[1 + iRowCount, iColumnCount])).Value = Cells;

                if (FilePath != null && FilePath != "")
                {
                    try
                    {
                        workSheet.SaveAs(FilePath);
                        excelApp.Quit();
                        System.Windows.Forms.MessageBox.Show("Report was saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Export to Excel failed: Excel file could not be saved! Check filepath! \n" +
                        ex.Message);
                    }//try/catch
                }
                else
                {
                    excelApp.Visible = true;
                }

            }
            catch (Exception e)
            {
                throw new Exception("Export to Excel failed: \n" + e.Message);
            }//try/catch
        }
        
        public void closeFile()
        {
            this._app.Quit();
        }
    }//Class
}//namespace
