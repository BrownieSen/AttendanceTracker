using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AttendanceTracker
{
    public partial class MainForm : Form
    {
        // Employee class to hold attendance data
        public class Employee
        {
            public string EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public string AttendanceStatus { get; set; }
        }

        private List<Employee> employeeList = new List<Employee>();

        public MainForm()
        {
            InitializeComponent();

            // Ensure ComboBox is cleared before adding items to avoid duplicates
            cbAttendanceStatus.Items.Clear();
            cbAttendanceStatus.Items.AddRange(new object[] { "Present", "Absent", "On Leave" });
            cbAttendanceStatus.SelectedIndex = 0;  // Default selection
        }

        // Save data to a CSV file
        private void SaveDataToCSV()
        {
            string filePath = "employee_data.csv";  // The path where the CSV will be saved

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                // Write the header line (optional)
                writer.WriteLine("EmployeeID,EmployeeName,AttendanceStatus");

                // Write each employee record as a line in the CSV
                foreach (var employee in employeeList)
                {
                    writer.WriteLine($"{employee.EmployeeID},{employee.EmployeeName},{employee.AttendanceStatus}");
                }
            }

            MessageBox.Show("Data saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Load data from a CSV file
        private void LoadDataFromCSV()
        {
            string filePath = "employee_data.csv";  // Path to your CSV file

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;

                    // Skip the header row
                    reader.ReadLine();

                    // Clear the existing employee list before loading new data
                    employeeList.Clear();

                    // Read the data from the file line by line
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split each line by commas
                        string[] data = line.Split(',');

                        // Create a new Employee object for each record and add it to the list
                        Employee employee = new Employee
                        {
                            EmployeeID = data[0],
                            EmployeeName = data[1],
                            AttendanceStatus = data[2]
                        };
                        employeeList.Add(employee);
                    }
                }

                // Update the DataGridView with the loaded data
                LoadEmployeeData();

                MessageBox.Show("Data loaded successfully.", "Load", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load employee data into the DataGridView
        private void LoadEmployeeData()
        {
            dgvAttendance.Rows.Clear();  // Clear existing rows in the DataGridView

            // Populate the DataGridView with the updated list of employees
            foreach (var employee in employeeList)
            {
                dgvAttendance.Rows.Add(employee.EmployeeID, employee.EmployeeName, employee.AttendanceStatus);
            }
        }

        // Add employee attendance
        private void btnAddAttendance_Click(object sender, EventArgs e)
        {
            // Trim input to avoid whitespace issues
            string employeeID = txtEmployeeID.Text.Trim();
            string employeeName = txtEmployeeName.Text.Trim();
            string attendanceStatus = cbAttendanceStatus.SelectedItem?.ToString();

            // Validate the input fields
            if (string.IsNullOrEmpty(employeeID) || string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(attendanceStatus))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Employee ID format (only numeric for this example)
            if (!Regex.IsMatch(employeeID, @"^\d+$"))
            {
                MessageBox.Show("Please enter a valid Employee ID (numeric only).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create a new employee object and add it to the list
            Employee newEmployee = new Employee
            {
                EmployeeID = employeeID,
                EmployeeName = employeeName,
                AttendanceStatus = attendanceStatus
            };
            employeeList.Add(newEmployee);

            // Update the DataGridView to display the latest data
            LoadEmployeeData();

            // Clear input fields
            txtEmployeeID.Clear();
            txtEmployeeName.Clear();
            cbAttendanceStatus.SelectedIndex = -1;

            MessageBox.Show("Attendance record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Update employee attendance
        private void btnUpdateAttendance_Click(object sender, EventArgs e)
        {
            string employeeID = txtEmployeeID.Text.Trim();
            string attendanceStatus = cbAttendanceStatus.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(employeeID) || string.IsNullOrWhiteSpace(attendanceStatus))
            {
                MessageBox.Show("Please enter a valid Employee ID and select a status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Employee ID format (only numeric for this example)
            if (!Regex.IsMatch(employeeID, @"^\d+$"))
            {
                MessageBox.Show("Please enter a valid Employee ID (numeric only).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find the employee in the list based on EmployeeID
            var employee = employeeList.Find(E => E.EmployeeID == employeeID);
            if (employee == null)
            {
                MessageBox.Show("Employee ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update the attendance status
            employee.AttendanceStatus = attendanceStatus;

            // Update the DataGridView to reflect the changes
            LoadEmployeeData();

            MessageBox.Show("Attendance record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Save button click event
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            string filePath= @"D:\Gian\Documents\ATDB\attendance.csv";
                using (var writer = new StreamWriter(filePath)) 
            SaveDataToCSV();
        }

        // Load button click event
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadDataFromCSV();
        }
    }
}
