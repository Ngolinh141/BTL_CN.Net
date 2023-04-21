using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Reflection;

namespace Quanlycuahangdienthoai
{
    /// <summary>
    /// Interaction logic for Mobi.xaml
    /// </summary>
    public partial class Mobi : Window
    {
        public Mobi()
        {
            InitializeComponent();
        }
        private SqlConnection Con = new SqlConnection(@"link data");

        
        private void label1_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
                if (string.IsNullOrEmpty(Mobidtb.Text) || string.IsNullOrEmpty(brandtb.Text) || string.IsNullOrEmpty(modeltb.Text) ||
                    string.IsNullOrEmpty(pricetb.Text) || string.IsNullOrEmpty(stocktb.Text) || string.IsNullOrEmpty(cameratb.Text))
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "INSERT INTO MobileTbl VALUES (@Mobid, @Brand, @Model, @Price, @Stock, @Ram, @Rom, @Camera)";
                            SqlCommand command = new SqlCommand(sql, connection);
                            command.Parameters.AddWithValue("@Mobid", Mobidtb.Text);
                            command.Parameters.AddWithValue("@Brand", brandtb.Text);
                            command.Parameters.AddWithValue("@Model", modeltb.Text);
                            command.Parameters.AddWithValue("@Price", pricetb.Text);
                            command.Parameters.AddWithValue("@Stock", stocktb.Text);
                            command.Parameters.AddWithValue("@Ram", ramcb.SelectedItem.ToString());
                            command.Parameters.AddWithValue("@Rom", romcb.SelectedItem.ToString());
                            command.Parameters.AddWithValue("@Camera", cameratb.Text);
                            command.ExecuteNonQuery();
                        }
                        MessageBox.Show("Mobile Added Successfully");
                        populate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            populate();
        }

        private void populate()
        {
                try
                {
                    Con.Open();
                    string query = "select * Mobitb";
                    SqlDataAdapter da = new SqlDataAdapter(query, Con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    MobiDGV.ItemsSource = dt.DefaultView;
                    Con.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
        }

        private void MobiDGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MobiDGV.SelectedItem != null)
            {
                Mobi mobile = (Mobi)MobiDGV.SelectedItem;
                Mobidtb.Text = mobile.Mobidtb.ToString();
                brandtb.Text = mobile.brandtb;
                modeltb.Text = mobile.modeltb;
                pricetb.Text = mobile.pricetb.ToString();
                stocktb.Text = mobile.stocktb.ToString();
                ramcb.SelectedItem = mobile.ramcb;
                romcb.SelectedItem = mobile.romcb;
                cameratb.Text = mobile.cameratb;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Mobidtb.Text = "";
            brandtb.Text = "";
            modeltb.Text = "";
            pricetb.Text = "";
            stocktb.Text = "";
            cameratb.Text = "";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Mobidtb.Text == "")
            {
                MessageBox.Show("Enter The mobile to Be Deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from MobileTbl where MobId=" + Mobidtb.Text + "";
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (Mobidtb.Text == "" || brandtb.Text == "" || modeltb.Text == "" ||
        pricetb.Text == "" || stocktb.Text == "" || cameratb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string sql = "update MobileTbl set Mbrand='" + brandtb.Text + "', MModel='" + modeltb.Text + "', MPrice=" + pricetb.Text + ", Mstock=" + stocktb.Text + ", MRam='" + ramcb.SelectedItem.ToString() + "', MRom='" + romcb.SelectedItem.ToString() + "', MCam='" + cameratb.Text + "' where MobId=" + Mobidtb.Text + ";";
                    SqlCommand cmd = new SqlCommand(sql, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Mobile Updated Successfully");
                    Con.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        
    } 
}

