using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Collections;
using System.Reflection;

namespace User_Management_System
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=DESKTOP-ENIT3AG\SQLEXPRESS;Initial Catalog=user_management_system;Integrated Security=True;TrustServerCertificate=True";
        private readonly UserRepository userRepository;
        
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            userRepository = new UserRepository(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadUsers();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Width = 200;
        }

        private void loadUsers()
        {
            DataTable dt = userRepository.GetUsers();
            dataGridView1.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string userName = txtName.Text;
            string userAddress = txtAddress.Text;
            string userEmail = txtEmail.Text;

            if (ValidateForm())
            {
                try
                {
                    int rowsAffected = userRepository.AddUser(userName, userAddress, userEmail);
                    ShowResultMessage(rowsAffected, "User Added Successfullly");
                }
                catch(Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            Console.WriteLine(selectedRow);
            txtId.Text = selectedRow.Cells[0].Value.ToString();
            txtName.Text = selectedRow.Cells[1].Value.ToString();
            txtAddress.Text = selectedRow.Cells[2].Value.ToString();
            txtEmail.Text = selectedRow.Cells[3].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string userId = txtId.Text;
            string userName = txtName.Text;
            string userAddress = txtAddress.Text;
            string userEmail = txtEmail.Text;

            if (userId.Equals(""))
            {
                MessageBox.Show("Please select row that you need to update", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (ValidateForm())
                {
                    try
                    {
                        int rowsAffected = userRepository.UpdateUser(userId, userName, userAddress, userEmail);
                        ShowResultMessage(rowsAffected, "User Updated Successfullly");
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            resetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure to delete this user?","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) {
                    string userId = txtId.Text;
                    int rowAffected = userRepository.DeleteUser(userId);

                    ShowResultMessage(rowAffected, "User Deleted Successfully");
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message); 
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            string searchTerm = txtSearch.Text;
            DataTable dt = userRepository.SearchUsers(searchTerm);
            dataGridView1.DataSource = dt;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required", "Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required", "Address", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required", "Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void resetForm()
        {
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtId.Text = string.Empty;
        }

        private void ShowResultMessage(int rows,string message)
        {
            if(rows == 1)
            {
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadUsers();
                resetForm();
            }
            else
            {
                MessageBox.Show("Something went wrong! Try again..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleException(Exception ex)
        {
            Console.WriteLine("Invalid operation: " + ex.Message);
            MessageBox.Show("Something went wrong! Try again..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
