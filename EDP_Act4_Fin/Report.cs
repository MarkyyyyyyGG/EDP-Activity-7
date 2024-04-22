using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
    public partial class Report : Form
    {
        private DatabaseManager dbManager;
        private string templateFilePath = "";
        public Report()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        //LOADING car brand combo box
        private void Report_Load(object sender, EventArgs e)
        {
            if (dbManager.OpenConnection())
            {
                try
                {
                    PopulateDataGridView();
                    PopulateStoreProvinceComboBox();
                    PopulateAgentComboBox();
                    string selectQuery = "SELECT * FROM car";
                    MySqlCommand cmdRetrieveDetails = new MySqlCommand(selectQuery, dbManager.connection);
                    MySqlDataReader reader = cmdRetrieveDetails.ExecuteReader();

                    while (reader.Read())
                    {
                        carBrandComb.Items.Add(reader.GetString("car_brand"));
                        //storeProvinceComb.Items.Add(reader.GetString("store_province"));
                    }

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

        private void PopulateStoreProvinceComboBox()
        {
            try
            {
                // Define the SQL query to fetch store_province
                string selectQuery = "SELECT store_province, store_region FROM store";

                // Execute the SQL query
                MySqlCommand cmdRetrieveDetails = new MySqlCommand(selectQuery, dbManager.connection);
                MySqlDataReader reader = cmdRetrieveDetails.ExecuteReader();

                // Check if there are rows to read
                if (reader.HasRows)
                {
                    // Loop through the rows and populate storeProvinceComb
                    while (reader.Read())
                    {
                        storeProvinceComb.Items.Add(reader.GetString("store_province"));
                        storeRegionComb.Items.Add(reader.GetString("store_region"));
                    }
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("No store provinces found in the database");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving store provinces from database: " + ex.Message);
            }
            finally
            {
                // Close the data reader
                //reader.Close();
            }
        }

        private void PopulateAgentComboBox()
        {
            try
            {
                // Define the SQL query to fetch store_province
                string selectQuery = "SELECT agent_lname FROM agent";

                // Execute the SQL query
                MySqlCommand cmdRetrieveDetails = new MySqlCommand(selectQuery, dbManager.connection);
                MySqlDataReader reader = cmdRetrieveDetails.ExecuteReader();

                // Check if there are rows to read
                if (reader.HasRows)
                {
                    // Loop through the rows and populate storeProvinceComb
                    while (reader.Read())
                    {
                        agentLnameComb.Items.Add(reader.GetString("agent_lname"));
                    }
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("No store provinces found in the database");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving store provinces from database: " + ex.Message);
            }
            finally
            {
                // Close the data reader
                //reader.Close();
            }
        }



        private void carBrandComb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selectedCarBrand = carBrandComb.GetItemText(carBrandComb.SelectedItem);
            carNameComb.Items.Clear();

            if (dbManager.OpenConnection())
            {
                try
                {
                    string selectQuery = "SELECT car_name FROM cardealership.car WHERE car_brand = @carBrand";
                    MySqlCommand cmdRetrieveCarsName = new MySqlCommand(selectQuery, dbManager.connection);
                    cmdRetrieveCarsName.Parameters.AddWithValue("@carBrand", selectedCarBrand);
                    MySqlDataReader reader = cmdRetrieveCarsName.ExecuteReader();

                    while (reader.Read())
                    {
      
                        carNameComb.Items.Add(reader.GetString("car_name"));
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving cars from database: " + ex.Message);
                }
                finally
                {
                    dbManager.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
        }

        private void carNameComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            carColorComb.Items.Clear();
            string selectedCarName = carNameComb.GetItemText(carNameComb.SelectedItem);
            if (dbManager.OpenConnection())
            {
                try
                {
                    string selectQuery = "SELECT car_price, car_color FROM cardealership.car WHERE car_name = @carName";
                    MySqlCommand cmdRetrievePrice = new MySqlCommand(selectQuery, dbManager.connection);
                    cmdRetrievePrice.Parameters.AddWithValue("@carName", selectedCarName);
                    MySqlDataReader reader = cmdRetrievePrice.ExecuteReader();

                    if (reader.Read())
                    {
                        double carPrice = reader.GetDouble("car_price");
                        carPriceTbox.Text = carPrice.ToString();
                        carPriceTbox.Enabled = false;
                        carColorComb.Items.Add(reader.GetString("car_color"));

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving cars from database: " + ex.Message);
                }
                finally
                {
                    dbManager.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
        }

        //SORTING DATAGRID BY DATE
        private void PopulateDataGridView()
        {
            string selectQuery = "SELECT * FROM transaction_2";

            MySqlCommand cmdRetrieveDetails = new MySqlCommand(selectQuery, dbManager.connection);

            DataTable dataTable = new DataTable();

            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmdRetrieveDetails))
            {
                adapter.Fill(dataTable);
            }

            dataGridView1.DataSource = dataTable;

            dataGridView1.Sort(dataGridView1.Columns["date"], ListSortDirection.Descending);
        }

        //ADD RECORD TO THE DATABASE
        private void button2_Click(object sender, EventArgs e)
        {
            string customerFname = cusfname.Text;
            string customerLname = cuslname.Text;
            string selectedDateFormatted = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string selectedCarBrand = carBrandComb.SelectedItem?.ToString();
            string selectedCarName = carNameComb.SelectedItem?.ToString();
            string selectedCarColor = carColorComb.SelectedItem?.ToString();
            int selectedCarPrice = int.Parse(carPriceTbox.Text);
            string selectedStoreRegion = storeRegionComb.SelectedItem?.ToString();
            string selectedStoreProvince = storeProvinceComb.SelectedItem?.ToString();
            string selectedAgent = agentLnameComb.SelectedItem?.ToString();
            int quantity = int.Parse(qtyTbox.Text);

            int total = selectedCarPrice * quantity;

            // Open the database connection
            if (!dbManager.OpenConnection())
            {
                MessageBox.Show("Failed to connect to database");
                return;
            }

            try
            {
                // Define the INSERT query
                string insertQuery = "INSERT INTO cardealership.transaction_2 (cus_fname, date, cus_lname, car_brand, car_name, car_color, car_price, quantity, store_region, store_province, agent_lname, total_cost) " +
                                     "VALUES (@customerFname, @transactionDate, @customerLname, @carBrand, @carName, @carColor, @carPrice, @quantity, @storeRegion, @storeProvince, @agentLname, @total)";

                // Create a MySqlCommand object with the INSERT query and the database connection
                MySqlCommand cmdInsert = new MySqlCommand(insertQuery, dbManager.connection);

                // Add parameters to the INSERT query
                cmdInsert.Parameters.AddWithValue("@customerFname", customerFname);
                cmdInsert.Parameters.AddWithValue("@customerLname", customerLname);
                cmdInsert.Parameters.AddWithValue("@carBrand", selectedCarBrand);
                cmdInsert.Parameters.AddWithValue("@carName", selectedCarName);
                cmdInsert.Parameters.AddWithValue("@carColor", selectedCarColor);
                cmdInsert.Parameters.AddWithValue("@carPrice", selectedCarPrice);
                cmdInsert.Parameters.AddWithValue("@storeRegion", selectedStoreRegion);
                cmdInsert.Parameters.AddWithValue("@storeProvince", selectedStoreProvince);
                cmdInsert.Parameters.AddWithValue("@agentLname", selectedAgent);
                cmdInsert.Parameters.AddWithValue("@transactionDate", selectedDateFormatted);
                cmdInsert.Parameters.AddWithValue("@quantity", quantity);
                cmdInsert.Parameters.AddWithValue("@total", total);
                // Execute the INSERT query
                int rowsAffected = cmdInsert.ExecuteNonQuery();

                // Check if the INSERT operation was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Data saved to database successfully");
                    PopulateDataGridView();
                }
                else
                {
                    MessageBox.Show("Failed to save data to database");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data to database: " + ex.Message);
            }
            finally
            {
                // Close the database connection
                dbManager.CloseConnection();
            }
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
                            worksheet.Cells[row, 2].Value = selectedRow.Cells["cus_fname"].Value.ToString(); // Assuming Column1 is where customer firstname is stored
                            worksheet.Cells[row, 5].Value = selectedRow.Cells["cus_lname"].Value.ToString(); // Assuming Column2 is where customer lastname is stored
                            worksheet.Cells[row, 9].Value = selectedRow.Cells["car_name"].Value.ToString(); // Assuming Column3 is where car name is stored
                            worksheet.Cells[row, 11].Value = selectedRow.Cells["car_brand"].Value.ToString(); // Assuming Column4 is where car brand is stored
                            worksheet.Cells[row, 13].Value = selectedRow.Cells["car_color"].Value.ToString(); // Assuming Column5 is where car color is stored
                            worksheet.Cells[row, 15].Value = selectedRow.Cells["quantity"].Value.ToString(); // Assuming Column6 is where quantity is stored
                            worksheet.Cells[row, 16].Value = selectedRow.Cells["car_price"].Value.ToString(); // Assuming Column7 is where total is stored
                            worksheet.Cells[row, 18].Value = selectedRow.Cells["total_cost"].Value.ToString(); // Assuming Column7 is where total is stored
                            worksheet.Cells[row, 21].Value = selectedRow.Cells["date"].Value.ToString(); // Assuming Column7 is where total is stored
                            worksheet.Cells[row, 23].Value = selectedRow.Cells["transaction_id"].Value.ToString(); // Assuming Column7 is where total is stored

                            row++;
                        }

                        ExcelWorksheet chartSheet = excelPackage.Workbook.Worksheets.Add("Chart");

                        var salesByDate = dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                                            .GroupBy(r => r.Cells["date"].Value.ToString())
                                            .Select(g => new { Date = g.Key, TotalSales = g.Sum(r => Convert.ToInt32(r.Cells["total_cost"].Value)) });

                        int chartRow = 2;
                        foreach (var item in salesByDate)
                        {
                            chartSheet.Cells[chartRow, 1].Value = item.Date;
                            chartSheet.Cells[chartRow, 2].Value = item.TotalSales;
                            chartRow++;
                        }

                        var chart = chartSheet.Drawings.AddChart("SalesChart", eChartType.LineMarkers);
                        chart.SetPosition(0, 0, 0, 0);
                        chart.SetSize(600, 400);
                        chart.Title.Text = "Total Sales by Date";
                        chart.XAxis.Title.Text = "Date";
                        chart.YAxis.Title.Text = "Total Sales";
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

        private void back_btn_Click(object sender, EventArgs e)
        {
            var dashboard = new Dashboard();
            this.Hide();
            dashboard.Show();
        }
    }
}
