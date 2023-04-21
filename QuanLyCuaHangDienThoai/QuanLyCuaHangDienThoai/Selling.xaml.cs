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
using System.Printing;

namespace Quanlycuahangdienthoai
{
    /// <summary>
    /// Interaction logic for Selling.xaml
    /// </summary>
    public partial class Selling : Window
    {
        public Selling()
        {
            InitializeComponent();
        }
        private SqlConnection Con = new SqlConnection(@"link data");
        private void populate()
        {
            Con.Open();
            String query = "select Mbrand, MModel, MPrice from MobileTbl";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            var ds = new DataSet();
            da.Fill(ds);
            MobiDGV.ItemsSource = ds.Tables[0].DefaultView;
            Con.Close();
        }

        private void populateAccess(object sender, EventArgs e)
        {
            Con.Open();
            String query = "select Abrand, AModel, APrice from AccessorieTbl";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            var ds = new DataSet();
            da.Fill(ds);
            AccessoriesDGV.ItemsSource = ds.Tables[0].DefaultView;
            Con.Close();
        }
        private void insertbill()
        {
            if (string.IsNullOrEmpty(BillIdtb.Text) || string.IsNullOrEmpty(ClientNameTb.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                int amount = Convert.ToInt32(Amtlblas.Text);
                try
                {
                    Con.Open();
                    String sql = "insert into BillTbl value(@BillId, @ClientName, @Amount)";
                    SqlCommand cmd = new SqlCommand(sql, Con);
                    cmd.Parameters.AddWithValue("@BillId", BillIdtb.Text);
                    cmd.Parameters.AddWithValue("@ClientName", ClientNameTb.Text);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.ExecuteNonQuery();

                    Con.Close();

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
        private void Selling_Load(object sender, EventArgs e)
        {
            populate();
            populateAccess();
            Sum();
        }

        private void populateAccess()
        {
            throw new NotImplementedException();
        }
        private void Sum()
        {
            string query = "SELECT SUM(Amt) FROM BillTbl";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sellamtlbl.Text = dt.Rows[0][0].ToString();
            }
        }
        private void MobiDGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MobiDGV.SelectedItem != null)
            {
                DataRowView row = (DataRowView)MobiDGV.SelectedItem;
                ProductTb.Text = row["Mbrand"].ToString() + row["MModel"].ToString();
                PriceTb.Text = row["MPrice"].ToString();
            }
        }

        private void AccessoriesDGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccessoriesDGV.SelectedItem != null)
            {
                DataRowView row = (DataRowView)AccessoriesDGV.SelectedItem;
                ProductTb.Text = row["Abrand"].ToString() + row["AModel"].ToString();
                PriceTb.Text = row["APrice"].ToString();
            }
        }
        private int n = 0;
        private int grdtotal = 0;
        int uprice;
        private void addtobill_Click(object sender, RoutedEventArgs e)
        {
            if (QtyTb.Text == "" || PriceTb.Text == "")
            {
                MessageBox.Show("Enter The Quantity");
            }
            else
            {
                int total = int.Parse(QtyTb.Text) * int.Parse(PriceTb.Text);
                BillItem newItem = new BillItem
                {
                    No = n + 1,
                    ProdName = ProductTb.Text,
                    ProdPrice = int.Parse(PriceTb.Text),
                    ProdQty = int.Parse(QtyTb.Text),
                    Total = total
                };
                BillDGV.Items.Add(newItem);
                n++;
                grdtotal += total;
                AmtLbl.Content = grdtotal.ToString();
            }
        }

        int n = 0, Grdtotal = 0;
        private void MobiDGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MobiDGV.SelectedItem != null)
            {
                DataRowView row = (DataRowView)MobiDGV.SelectedItem;
                ProductTb.Text = row["Mbrand"].ToString() + row["MModel"].ToString();
                PriceTb.Text = row["MPrice"].ToString();
            }
        }
        private void AccessoriesDGV_CellContentClick(object sender, SelectionChangedEventArgs e e)
        {
            ProductTb.Text = MobiDGV.SelectedRows[0].Cells[0].Value.ToString() +
            MobiDGV.SelectedRows[0].Cells[1].Value.ToString();
            PriceTb.Text = MobiDGV.SelectedRows[0].Cells[2].Value.ToString();
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;
                printDialog.PrintDocument(printDocument.DocumentPaginator, "MobiSoft Bill");
            }
        }

        private void PrintButton1_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintTicket.PageOrientation = PageOrientation.Portrait;

                FlowDocument doc = new FlowDocument();

                Paragraph title = new Paragraph(new Run("MOBISOFT 1.0"));
                title.FontSize = 12;
                title.FontWeight = FontWeights.Bold;
                title.Foreground = Brushes.Red;
                doc.Blocks.Add(title);

                Paragraph header = new Paragraph(new Run("ID PRODUCT PRICE QUANTITY TOTAL"));
                header.FontSize = 10;
                header.FontWeight = FontWeights.Bold;
                header.Foreground = Brushes.Red;
                doc.Blocks.Add(header);

                foreach (DataRowView row in BillDGV.Items)
                {
                    int prodid = Convert.ToInt32(row["Column1"]);
                    string prodname = "" + row["Column2"];
                    int prodprice = Convert.ToInt32(row["Column3"]);
                    int prodqty = Convert.ToInt32(row["Column4"]);
                    int tottal = Convert.ToInt32(row["Column5"]);

                    Paragraph item = new Paragraph();
                    item.Inlines.Add(new Run(prodid.ToString()) { FontWeight = FontWeights.Bold });
                    item.Inlines.Add(new Run("\t" + prodname) { FontWeight = FontWeights.Bold });
                    item.Inlines.Add(new Run("\t" + prodprice.ToString()) { FontWeight = FontWeights.Bold });
                    item.Inlines.Add(new Run("\t" + prodqty.ToString()) { FontWeight = FontWeights.Bold });
                    item.Inlines.Add(new Run("\t" + tottal.ToString()) { FontWeight = FontWeights.Bold });
                    doc.Blocks.Add(item);

                    Grdtotal += tottal;
                }

                Paragraph total = new Paragraph(new Run("Grand Total: Rs" + Grdtotal));
                total.FontSize = 12;
                total.FontWeight = FontWeights.Bold;
                total.Foreground = Brushes.Crimson;
                doc.Blocks.Add(total);

                Paragraph footer = new Paragraph(new Run("***************MobiSoft***************"));
                footer.FontSize = 12;
                footer.FontWeight = FontWeights.Bold;
                footer.Foreground = Brushes.Crimson;
                doc.Blocks.Add(footer);

                IDocumentPaginatorSource idpSource = doc;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "MobiSoft Bill");
                BillDGV.Items.Clear();
                Grdtotal = 0;
                insertbill();
                Sum();
            }
        }
        private void print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintTicket.PageMediaSize = new PageMediaSize(285, 600);
                printDialog.PrintTicket.PageOrientation = PageOrientation.Portrait;

                FlowDocument document = new FlowDocument();
                // Add content to the FlowDocument here...

                IDocumentPaginatorSource paginator = document;
                printDialog.PrintDocument(paginator.DocumentPaginator, "Print Document");
            }
        }
        private void AccessoriesDGV_CellContentClick(object sender, SelectionChangedEventArgs e e)
        {
            ProductTb.Text = MobiDGV.SelectedRows[0].Cells[0].Value.ToString() +
            MobiDGV.SelectedRows[0].Cells[1].Value.ToString();
            PriceTb.Text = MobiDGV.SelectedRows[0].Cells[2].Value.ToString();
        }
    }
}
