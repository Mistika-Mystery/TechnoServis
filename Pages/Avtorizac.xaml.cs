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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TechnoServis.Pages
{
    /// <summary>
    /// Логика взаимодействия для Avtorizac.xaml
    /// </summary>
    public partial class Avtorizac : Page
    {
        public static int FlagRole;
        public static int FlagIdUser;
        public Avtorizac()
        {
            InitializeComponent();
        }

        private void LogBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userLog = AppEntities.GetContext().User.FirstOrDefault(x => x.Login == LogTB.Text && x.Password == PassTB.Password);
                if (userLog == null)
                {
                    MessageBox.Show("что-то пошло не так, попробуйте снова или одратитесь к администратору");

                }
                else
                {
                    Flag.FlagRole = userLog.IdRole;
                    Flag.FlagIdUser = userLog.Id;
                    NavigationService.Navigate(new JornalRequest());
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void GuestBTN_Click(object sender, RoutedEventArgs e)
        {
            Flag.FlagRole = 4;
            NavigationService.Navigate(new AddEdditPage(null));
        }
    }
}
