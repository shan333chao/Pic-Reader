using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
namespace ReadPic
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(txtfolder.Text))
                {
                    btnRead.Text = "正在读取";
                    this.Enabled = false;

                    var files = Directory.EnumerateFiles(txtfolder.Text);
                    foreach (var item in files)
                    {
                        txtResult.AppendText(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss  ") + "\n\n" + item + "\r\n");
                        var list = BuildCorrectData(readImageToString(item), item);
                        GotoSqlite.insertToDb(list);
                    }
                    txtResult.AppendText("读取完成");
                    btnRead.Text = "读取";
                    this.Enabled = true;
                }
                else
                {
                    MessageBox.Show("选择文件夹不对");
                }
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                MessageBox.Show(ex.Message);
            }
        }

        public static List<UserModel> BuildCorrectData(string content, string filePath)
        {
            List<UserModel> list = new List<UserModel>();
            string[] naveValueStr = content.Split('#');
            foreach (var item in naveValueStr)
            {
                string[] itemKv = item.Split(':');
                if (itemKv.Length == 2)
                {
                    list.Add(new UserModel
                    {
                        filePath = filePath,
                        fileTime = DateTime.Now,
                        nickname = itemKv[0],
                        telephone = itemKv[1]
                    });
                }

            }
            return list;
        }


        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string readImageToString(string filePath)
        {
            string result = string.Empty;
            using (var engine = new TesseractEngine(@"./tessdata", "chi_sim", EngineMode.Default))
            {
                using (var imge = new Bitmap(filePath))
                {
                    using (var page = engine.Process(imge, PageSegMode.AutoOsd))
                    {
                        //    page.GetText();

                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();

                            do
                            {
                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                            {
                                                Console.WriteLine("<BLOCK>");
                                            }
                                            string temp = iter.GetText(PageIteratorLevel.Word).Replace(" ", "");
                                            if (temp != "添加")
                                            {
                                                if (temp.IsPhoneNumberMached())
                                                {
                                                    result += ":" + temp + "#";
                                                }
                                                else
                                                {
                                                    result += temp;
                                                }
                                            }


                                            if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                        if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                        {
                                            Console.WriteLine();
                                        }
                                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                            } while (iter.Next(PageIteratorLevel.Block));
                        }
                    }
                }
            }
            return result;
        }

        private void btn_selectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult res = fbd.ShowDialog();
            if (res == DialogResult.OK)
            {
                txtfolder.Text = fbd.SelectedPath;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            List<UserModel> list = GotoSqlite.GetAllUsers();
            if (list.Count == 0)
            {
                MessageBox.Show("暂无数据");
                return;
            }
            string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "ExportFolder\\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "contract.xls");
            ExcelHelper.GridToExcelByNPOI(list, path);
            txtResult.AppendText("\r\n"+path);
            MessageBox.Show("导出数据成功\r\n" + path);
            Process.Start("explorer.exe", "/select, " + path);
        }
    }
}
