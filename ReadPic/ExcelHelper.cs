using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadPic
{
    public class ExcelHelper
    {
        //Datatable导出Excel
        public static void GridToExcelByNPOI(List<UserModel> list, string strExcelFileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            //ICellStyle HeadercellStyle = workbook.CreateCellStyle();
            //HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            //HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            //HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            //HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            ////字体
            //NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
            //headerfont.Boldweight = (short)FontBoldWeight.Bold;
            //HeadercellStyle.SetFont(headerfont);


            ////用column name 作为列名
            //int icolIndex = 0;
            //IRow headerRow = sheet.CreateRow(0);
            //foreach (DataColumn item in dt.Columns)
            //{
            //    ICell cell = headerRow.CreateCell(icolIndex);
            //    cell.SetCellValue(item.ColumnName);
            //    cell.CellStyle = HeadercellStyle;
            //    icolIndex++;
            //}

            ICellStyle cellStyle = workbook.CreateCellStyle();

            //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;


            NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
            cellfont.Boldweight = (short)FontBoldWeight.Normal;
            cellStyle.SetFont(cellfont);

            for (int i = 0; i < list.Count; i++)
            {
                IRow DataRow = sheet.CreateRow(i);
                ICell cell = DataRow.CreateCell(0);
                cell.SetCellValue(list[i].nickname);
                cell.CellStyle = cellStyle;
                ICell cell2 = DataRow.CreateCell(1);
                cell2.SetCellValue(list[i].telephone);
                cell2.CellStyle = cellStyle;
            }
 

            //写Excel
            using (FileStream file = new FileStream(strExcelFileName, FileMode.OpenOrCreate))
            {
                workbook.Write(file);
                file.Flush();
                file.Close();
            };


        }
    }
}
