using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HandBookAppSearch
{
    public partial class SearchForm : Form
    {
        SqlConnection sqlConnection;
        public SearchForm()
        {
            InitializeComponent();
            this.btnExit.Click += new EventHandler(btnExit_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            listSearch.Font = new Font("Bradley Hand ITC", 14);
            listSearch.Items.Clear();
            string connectionString = @"Data Source=localhost;Initial Catalog=Handbook;Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SearchRec_Procedure @firstname, @secondname, @surname, @dateofbirth, @phonenumber, @address ", sqlConnection);

            try
            {
                command.Parameters.Add("@firstname", SqlDbType.NVarChar).Value = txtSearch.Text;
                command.Parameters.Add("@secondname", SqlDbType.NVarChar).Value = txtSearch.Text;
                command.Parameters.Add("@surname", SqlDbType.NVarChar).Value = txtSearch.Text;
                command.Parameters.Add("@dateofbirth", SqlDbType.Char).Value = txtSearch.Text;
                command.Parameters.Add("@phonenumber", SqlDbType.NVarChar).Value = txtSearch.Text;
                command.Parameters.Add("@address", SqlDbType.NVarChar).Value = txtSearch.Text;
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {

                    listSearch.Items.Add(Convert.ToString(sqlReader["FirstName"]) + "   " + Convert.ToString(sqlReader["SecondName"]) + "   " + Convert.ToString(sqlReader["Surname"]) + "  " + Convert.ToString(sqlReader["DateofBirth"]) + "    " + Convert.ToString(sqlReader["Phonenumber"]) + "    " + Convert.ToString(sqlReader["Address"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
                Application.Exit();
        }
    }
}
