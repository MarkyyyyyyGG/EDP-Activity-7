using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXmlLic = OfficeOpenXml.LicenseContext;
using OfficeOpenXml.Drawing.Chart; 

namespace EDP_Act4_Fin
{
    public partial class Products : Form
    {
        private DatabaseManager dbManager;
        private string templateFilePath = "";
        public Products()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void ProductManagement_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;


            if (dbManager.OpenConnection())
            {
                try
                {
                    PopulateDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving email from database: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
            dbManager.CloseConnection();
        }

        private void PopulateDataGridView()
        {
            string selectQuery = "SELECT * FROM car";

            MySqlCommand cmdRetrieveDetails = new MySqlCommand(selectQuery, dbManager.connection);

            DataTable dataTable = new DataTable();

            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmdRetrieveDetails))
            {
                adapter.Fill(dataTable);
            }

            dataGridView1.DataSource = dataTable;

            //dataGridView1.Sort(dataGridView1.Columns["date"], ListSortDirection.Descending);
        }

        //CHOOSE TEMPLATE   
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"C:\";

            openFileDialog.Title = "Choose Excel template file";

            openFileDialog.Filter = "Excel Files (*.xlsx, *.xls)|*.xlsx;*.xls";

            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                templateFilePath = openFileDialog.FileName;

                label13.Text = templateFilePath;

                exportBtn.Enabled = true;
            }
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    ExcelPackage.LicenseContext = OfficeOpenXmlLic.NonCommercial;

                    FileInfo templateFile = new FileInfo(templateFilePath);

                    string exportFileName = "ExportedFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    string exportFilePath = Path.Combine(Path.GetDirectoryName(templateFilePath), exportFileName);

                    File.Copy(templateFilePath, exportFilePath);

                    FileInfo exportFile = new FileInfo(exportFilePath);
                    using (ExcelPackage excelPackage = new ExcelPackage(exportFile))
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0]; // Zero-based index

                        int row = 11; // Start row for data entry

                        foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                        {
                            worksheet.Cells[row, 2].Value = selectedRow.Cells["car_id"].Value.ToString(); // Assuming Column1 is where car_id is stored
                            worksheet.Cells[row, 5].Value = selectedRow.Cells["store_id"].Value.ToString(); // Assuming Column2 is where store_id is stored
                            worksheet.Cells[row, 9].Value = selectedRow.Cells["car_name"].Value.ToString(); // Assuming Column3 is where car name is stored
                            worksheet.Cells[row, 11].Value = selectedRow.Cells["car_brand"].Value.ToString(); // Assuming Column4 is where car brand is stored
                            worksheet.Cells[row, 13].Value = selectedRow.Cells["car_color"].Value.ToString(); // Assuming Column5 is where car color is stored
                            worksheet.Cells[row, 15].Value = selectedRow.Cells["stocks"].Value.ToString(); // Assuming Column6 is where stocks is stored
                            worksheet.Cells[row, 18].Value = selectedRow.Cells["car_price"].Value.ToString(); // Assuming Column7 is where car_price is stored

                            row++;
                        }

                        // Add a new worksheet for the graph
                        ExcelWorksheet chartSheet = excelPackage.Workbook.Worksheets.Add("Graph");

                        // Query data from DataGridView
                        var storeStocks = dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                                            .GroupBy(r => r.Cells["store_id"].Value.ToString())
                                            .Select(g => new { StoreID = g.Key, TotalStocks = g.Sum(r => Convert.ToInt32(r.Cells["stocks"].Value)) });

                        // Populate data in the new worksheet
                        int chartRow = 2;
                        foreach (var item in storeStocks)
                        {
                            chartSheet.Cells[chartRow, 1].Value = item.StoreID;
                            chartSheet.Cells[chartRow, 2].Value = item.TotalStocks;
                            chartRow++;
                        }

                        // Create a bar graph based on the data
                        var chart = chartSheet.Drawings.AddChart("StocksChart", eChartType.ColumnClustered);
                        chart.SetPosition(0, 0, 0, 0);
                        chart.SetSize(600, 400);
                        chart.Title.Text = "Stocks by Store";
                        chart.XAxis.Title.Text = "Store ID";
                        chart.YAxis.Title.Text = "Total Stocks";
                        chart.Series.Add(chartSheet.Cells[2, 2, chartRow - 1, 2], chartSheet.Cells[2, 1, chartRow - 1, 1]);

                        excelPackage.Save();

                        DialogResult result = MessageBox.Show("Data exported to Excel successfully in: " + exportFile.FullName + ".\n\n Do you want to open the file?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(exportFile.FullName);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No rows selected to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data to Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"C:\";

            openFileDialog.Title = "Choose Excel template file";

            openFileDialog.Filter = "Excel Files (*.xlsx, *.xls)|*.xlsx;*.xls";

            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                templateFilePath = openFileDialog.FileName;

                label13.Text = templateFilePath;

                exportBtn.Enabled = true;
            }
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            var dashboard = new Dashboard();
            this.Hide();
            dashboard.Show();
        }
    }
}
