using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudanec_WiFi_Manager_App
{
    internal class OfficeTools
    {
        public static void Export_Excel(DataGridView _ctrl)
        {
            try
            {
                _ctrl.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                _ctrl.SelectAll();
                Clipboard.SetDataObject(_ctrl.GetClipboardContent(), true);

                Microsoft.Office.Interop.Excel.Application xlexcel;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlexcel = new Microsoft.Office.Interop.Excel.Application();
                xlexcel.ScreenUpdating = false;
                xlexcel.Visible = true;
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                xlWorkSheet.Columns[1].Delete();
                xlWorkSheet.Columns.AutoFit();
                xlWorkSheet.get_Range("A1").Select();
                xlexcel.ScreenUpdating = true;

                if (_ctrl.Rows.Count > 0)
                {
                    _ctrl.CurrentCell = _ctrl[1, 0];
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("An error occurred when exporting to MS Excel: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
