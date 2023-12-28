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
    /// Логика взаимодействия для AddEdditPage.xaml
    /// </summary>
    public partial class AddEdditPage : Page
    {
        private Request newRequest= new Request();
        public AddEdditPage(Request importReq)
        {
            InitializeComponent();
            if (importReq != null )
            {
                newRequest = importReq;

                bool CanEdit = Flag.FlagRole == 1 || Flag.FlagRole == 2;
                StatusCB.IsEnabled = CanEdit;
                DiscrTB.IsEnabled = CanEdit;

            }
            DataContext= newRequest;
            StatusCB.ItemsSource = AppEntities.GetContext().Status.ToList();

            

            DataCB.Text = DateTime.Now.ToString();




        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder err = new StringBuilder();
                if (string.IsNullOrEmpty(newRequest.Description)) err.AppendLine("Опишите проблему");
                if (err.Length> 0 )
                {
                    MessageBox.Show(err.ToString());
                    return;
                }
                if (newRequest.Id==0)
                {
                    newRequest.IdUser = Flag.FlagIdUser;
                    newRequest.StartDate = DateTime.Now;
                    newRequest.IdStatus = 1;

                }
                AppEntities.GetContext().Request.Add(newRequest);
                AppEntities.GetContext().SaveChanges();
                MessageBox.Show("Успех!");
                if (Flag.FlagRole == 4)
                {
                    NavigationService.Navigate(new AddEdditPage(null));
                }
                else
                {
                    NavigationService.Navigate(new JornalRequest());

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
