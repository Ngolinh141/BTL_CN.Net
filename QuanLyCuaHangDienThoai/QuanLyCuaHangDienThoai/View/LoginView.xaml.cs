using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quanlycuahangdienthoai.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public object LoginStatusTextBlock { get; private set; }

        public LoginView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
                string username = txtUser.Text;
                string password = txtPass.Password;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    LoginStatusTextBlock.Text = "Vui lòng nhập tên người dùng và mật khẩu của bạn.";
                    return;
                }

                if (username == "admin" && password == "password")
                {
                    LoginStatusTextBlock.Text = "Đăng nhập thành công.";
                }
                else
                {
                    LoginStatusTextBlock.Text = "Tên người dùng hoặc mật khẩu không hợp lệ.";
                }
            
        }
    }
}