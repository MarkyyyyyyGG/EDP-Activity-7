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
  
    public partial class EmpSal : Form
    {  
        private DatabaseManager dbManager;
        private string templateFilePath = "";
        private DataTable originalDataTable; 

        public EmpSal()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void EmpSal_Load(object sender, EventArgs e)
        {
            storeIDTbox.Enabled = false;
            agentFnameTbox.Enabled = false;
            agentLnameTbox.Enabled = false;

            BackgroundImageLayout = ImageLayout.Stretch;
            PopulateDataGridView();

            if (dbManager.OpenConnection())
            {
                try
                {
                    PopulateAgentIDComb();

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
            string selectQuery = "SELECT * FROM agent_salary";

            MySqlCommand cmdRetrieveDetails = new MySqlCommand(selectQuery, dbManager.connection);

            originalDataTable = new DataTable(); // Initialize originalDataTable

            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmdRetrieveDetails))
            {
                adapter.Fill(originalDataTable); // Fill originalDataTable with data
            }

            dataGridView1.DataSource = originalDataTable;

            //dataGridView1.Sort(dataGridView1.Columns["date"], ListSortDirection.Descending);
        }

        private void PopulateAgentIDComb()
        {
            try
            {
                // Define the SQL query to fetch store_province
                string selectQuery = "SELECT agent_id FROM agent ORDER BY agent_id ASC";

                // Execute the SQL query
                MySqlCommand cmdRetrieveDetails = new MySqlCommand(selectQuery, dbManager.connection);
                MySqlDataReader reader = cmdRetrieveDetails.ExecuteReader();

                // Check if there are rows to read
                if (reader.HasRows)
                {
                    // Loop through the rows and populate storeProvinceComb
                    while (reader.Read())
                    {
                        agentIdComb.Items.Add(reader.GetInt32("agent_id"));
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

        private void agentIdComb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selectedAgentID = agentIdComb.GetItemText(agentIdComb.SelectedItem);

            if (dbManager.OpenConnection())
            {
                try
                {
                    string selectQuery = "SELECT store_id,agent_fname,agent_lname FROM cardealership.agent WHERE agent_id = @AgentID";
                    MySqlCommand cmdRetrieveCarsName = new MySqlCommand(selectQuery, dbManager.connection);
                    cmdRetrieveCarsName.Parameters.AddWithValue("@AgentID", selectedAgentID);
                    MySqlDataReader reader = cmdRetrieveCarsName.ExecuteReader();

                    while (reader.Read())
                    {
                        int agentStoreID = reader.GetInt32("store_id");
                        string agentFname = reader.GetString("agent_fname");
                        string agentLname = reader.GetString("agent_lname");

                        storeIDTbox.Text = agentStoreID.ToString();
                        agentFnameTbox.Text = agentFname.ToString();
                        agentLnameTbox.Text = agentLname.ToString();
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

        //ADD SALARY RECORD

        private void button2_Click(object sender, EventArgs e)
        {
            
        }


        private void salaryTbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // Allow only one negative sign at the beginning
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                string agentID = agentIdComb.SelectedItem?.ToString();
                int storeID = int.Parse(storeIDTbox.Text);
                string agentFname = agentFnameTbox.Text;
                string agentLname = agentLnameTbox.Text;
                int salary = int.Parse(salaryTbox.Text);
                string selectedDateFormatted = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                // Open the database connection
                if (!dbManager.OpenConnection())
                {
                    MessageBox.Show("Failed to connect to database");
                    return;
                }

                // Define the INSERT query
                string insertQuery = "INSERT INTO cardealership.agent_salary (agent_id, store_id, agent_fname, agent_lname, agent_salary, date) " +
                                     "VALUES (@agentID, @storeID, @agentFname, @agentLname, @salary, @selectedDateFormatted)";

                // Create a MySqlCommand object with the INSERT query and the database connection
                MySqlCommand cmdInsert = new MySqlCommand(insertQuery, dbManager.connection);

                // Add parameters to the INSERT query
                cmdInsert.Parameters.AddWithValue("@agentID", agentID);
                cmdInsert.Parameters.AddWithValue("@storeID", storeID);
                cmdInsert.Parameters.AddWithValue("@agentFname", agentFname);
                cmdInsert.Parameters.AddWithValue("@agentLname", agentLname);
                cmdInsert.Parameters.AddWithValue("@salary", salary);
                cmdInsert.Parameters.AddWithValue("@selectedDateFormatted", selectedDateFormatted);

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
            catch (FormatException ex)
            {
                MessageBox.Show("Error: Invalid input format. Please ensure that the input fields are correctly filled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data to database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Close the database connection
                dbManager.CloseConnection();
            }
        }

        //SEARCH FUNCTION
        private void search_txtbox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void search_txtbox_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                // Get the search keyword from the TextBox
                string keyword = search_txtbox.Text.Trim().ToLower();

                // Clear the current selection to avoid InvalidOperationException
                dataGridView1.ClearSelection();

                // Filter the DataGridView based on the search keyword
                if (!string.IsNullOrEmpty(keyword))
                {
                    // Create a DataView based on the original DataTable
                    DataView dataView = new DataView(originalDataTable);

                    // Apply a RowFilter to dynamically filter the data
                    dataView.RowFilter = $"agent_lname LIKE '%{keyword}%'";

                    // Assign the DataView to the DataGridView
                    dataGridView1.DataSource = dataView.ToTable();
                }
                else
                {
                    // If the search keyword is empty, reset the DataGridView to the original data
                    dataGridView1.DataSource = originalDataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                            worksheet.Cells[row, 2].Value = selectedRow.Cells["agent_fname"].Value.ToString(); // Assuming Column1 is where car_id is stored
                            worksheet.Cells[row, 5].Value = selectedRow.Cells["agent_lname"].Value.ToString(); // Assuming Column2 is where store_id is stored
                            worksheet.Cells[row, 9].Value = selectedRow.Cells["store_id"].Value.ToString(); // Assuming Column3 is where car name is stored
                            worksheet.Cells[row, 11].Value = selectedRow.Cells["agent_id"].Value.ToString(); // Assuming Column4 is where car brand is stored
                            worksheet.Cells[row, 15].Value = selectedRow.Cells["agent_salary"].Value; // Assuming Column5 is where car color is stored
                            worksheet.Cells[row, 18].Value = selectedRow.Cells["date"].Value.ToString(); // Assuming Column6 is where stocks is stored
                            worksheet.Cells[row, 21].Value = selectedRow.Cells["transaction_id"].Value.ToString(); // Assuming Column7 is where car_price is stored

                            row++;
                        }

                        // Add a new worksheet for the chart
                        ExcelWorksheet chartSheet = excelPackage.Workbook.Worksheets.Add("SalaryExpenseChart");

                        var totalSalaryByDate = dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                            .GroupBy(r => r.Cells["date"].Value.ToString())
                            .Select(g => new { Date = g.Key, TotalSalary = g.Sum(r => Convert.ToInt32(r.Cells["agent_salary"].Value)) });

                        int chartRow = 2;
                        foreach (var item in totalSalaryByDate)
                        {
                            chartSheet.Cells[chartRow, 1].Value = item.Date;
                            chartSheet.Cells[chartRow, 2].Value = item.TotalSalary;
                            chartRow++;
                        }

                        // Create a line chart on the new worksheet
                        var chart = chartSheet.Drawings.AddChart("SalaryExpenseChart", eChartType.LineMarkers);
                        chart.SetPosition(0, 0, 0, 0);
                        chart.SetSize(600, 400);
                        chart.Title.Text = "Total Expense on Salary by Date";
                        chart.XAxis.Title.Text = "Date";
                        chart.YAxis.Title.Text = "Total Salary Expense";

                        // Add data series to the chart
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
