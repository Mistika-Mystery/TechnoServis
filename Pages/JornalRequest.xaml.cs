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
    /// Логика взаимодействия для JornalRequest.xaml
    /// </summary>
    public partial class JornalRequest : Page
    {
        public JornalRequest()
        {
            InitializeComponent();
            var allStatus = AppEntities.GetContext().Status.ToList();
            allStatus.Insert(0, new Status
            {
                Name = "Без фильтрации"
            });
            FiltrCB.ItemsSource = allStatus;
            if (Flag.FlagRole != 1)
            {
                DelBTN.Visibility= Visibility.Collapsed;
            }
            SerchRequest();
        }
        private void SerchRequest()
        {
            var SearchAll = AppEntities.GetContext().Request.ToList();
           
            
             if (Flag.FlagRole != 1 && Flag.FlagRole != 2)
            {
                SearchAll = SearchAll.Where(x => x.IdUser == Flag.FlagIdUser).ToList();
            }

            SearchAll = SearchAll.Where( x=> x.User.Surname.ToLower().Contains(SearchTB.Text.ToLower())
            || (x.User.Name ?? "").ToLower().Contains(SearchTB.Text.ToLower())
            || (x.User.FhaterName ?? "").ToLower().Contains(SearchTB.Text.ToLower())).ToList();

            if (FiltrCB.SelectedIndex != 0)
            {
                SearchAll = SearchAll.Where( x=> x.Status == FiltrCB.SelectedValue).ToList();
            }
            switch (SortCB.SelectedIndex)
            {
                case 1:
                    SearchAll = SearchAll.OrderBy(x => x.StartDate).ToList();
                    break;
                case 2:
                    SearchAll = SearchAll.OrderByDescending(x => x.StartDate).ToList();
                    break;
            }
            

            JornalDG.ItemsSource= SearchAll;
        }
        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SerchRequest();
        }

        private void FiltrCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SerchRequest();
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SerchRequest();
        }

        private void EditBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEdditPage((sender as Button).DataContext as Request));
        }

        private void DelBTN_Click(object sender, RoutedEventArgs e)
        {
            var delReq = JornalDG.SelectedItems.Cast<Request>().ToList();
            if (delReq.Any(x => x.IdStatus != 3))
            {
                MessageBox.Show("Нельзя удалять не завершенные заявки");
                return;
            }

            if (MessageBox.Show("Вы уверены? удаление без востановления!", "ВНИМАНИЕ", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                

                try
                {
                    AppEntities.GetContext().Request.RemoveRange(delReq);
                    AppEntities.GetContext().SaveChanges();
                    SerchRequest();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void NewBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEdditPage(null));
        }

        private void ExBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Avtorizac());
        }
    }
}
