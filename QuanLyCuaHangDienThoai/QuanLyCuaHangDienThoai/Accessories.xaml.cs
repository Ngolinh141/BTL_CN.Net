using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Quanlycuahangdienthoai
{
    /// <summary>
    /// Interaction logic for Accessories.xaml
    /// </summary>
    public partial class Accessories : Window
    {
        public Accessories()
        {
            InitializeComponent();
        }
        private SqlConnection Con = new SqlConnection(@"link data");
        private void populate()
        {
            try
            {
                Con.Open();
                string query = "select * from AccessoriesTbl";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                AccessorieDGV.ItemsSource = dt.DefaultView;
                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (AidTb.Text == "" || AbrandTb.Text == "" || ApriceTb.Text == "" || AmodelTb.Text == "" || AstockTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string sql = "insert into AccessoriesTbl values(" + AidTb.Text + ", '" + AbrandTb.Text + "', '" + AmodelTb.Text + "', " + AStockTb.Text + ", " + ApriceTb.Text + ")";
                    SqlCommand cmd = new SqlCommand(sql, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Accessories Added Successfully");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void Accessoires_Loaded(object sender, RoutedEventArgs e)
        {
            populate();
        }
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            AidTb.Text = "";
            AbrandTb.Text = "";
            AmodelTb.Text = "";
            ApriceTb.Text = "";
            AstockTb.Text = "";
        }

        private void Accessorise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccessoriesDGV.SelectedItem != null)
            {
                DataRowView row = AccessoriesDGV.SelectedItem as DataRowView;
                AidTb.Text = row["Id"].ToString();
                AbrandTb.Text = row["Brand"].ToString();
                AmodelTb.Text = row["Model"].ToString();
                AstockTb.Text = row["Stock"].ToString();
                ApriceTb.Text = row["Price"].ToString();
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (AidTb.Text == "")
            {
                MessageBox.Show("Enter The Accessorie to Be Deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from AccessorieTbl where AId=" + AidTb.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Mobile Deleted");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AidTb.Text) || string.IsNullOrEmpty(AbrandTb.Text) ||
        string.IsNullOrEmpty(AmodelTb.Text) || string.IsNullOrEmpty(AStockTb.Text) ||
        string.IsNullOrEmpty(ApriceTb.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string sql = "UPDATE AccessoriesTbl SET Abrand=@Abrand, Amodel=@Amodel, Astock=@Astock, Aprice=@Aprice WHERE Aid=@Aid";
                    SqlCommand cmd = new SqlCommand(sql, Con);
                    cmd.Parameters.AddWithValue("@Aid", AidTb.Text);
                    cmd.Parameters.AddWithValue("@Abrand", AbrandTb.Text);
                    cmd.Parameters.AddWithValue("@Amodel", AmodelTb.Text);
                    cmd.Parameters.AddWithValue("@Astock", AstockTb.Text);
                    cmd.Parameters.AddWithValue("@Aprice", ApriceTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Accessorie Updated Successfully");
                    Con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AccessoriesDGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
