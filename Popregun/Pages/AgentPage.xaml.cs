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
using Popregun.DataBase;
using System.IO;
using System.Collections.ObjectModel;
using Popregun.Windows;
using Microsoft.Win32;

namespace Popregun.Pages
{
	/// <summary>
	/// Логика взаимодействия для AgentPage.xaml
	/// </summary>
	public partial class AgentPage : Page
	{
		public Agent Agent { get; set; }
        public List<AgentType> AgentTypes { get; set; }
		public AgentPage(Agent agent, bool isNew = true)
		{
			InitializeComponent();
			Agent = agent;
			AgentTypes = BdConnection.connection.AgentType.ToList();

			if (!isNew)
			{
				btnDel.Visibility = Visibility.Visible;
			}

			DataContext = this;
		}
        private void btnAddLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Agent.Logo = File.ReadAllBytes(openFileDialog.FileName);
                imgLogo.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            if (Agent.Title == null)
                stringBuilder.AppendLine("Не заполнено наименование");
            if (Agent.AgentType == null)
                stringBuilder.AppendLine("Не выбран тип");
            if (Agent.Address == null)
                stringBuilder.AppendLine("Не заполнен адрес");
            if (Agent.INN == null)
                stringBuilder.AppendLine("Не заполнено ИНН");
            if (Agent.KPP == null)
                stringBuilder.AppendLine("Не заполнено КПП");
            if (Agent.DirectorName == null)
                stringBuilder.AppendLine("Не заполнено имя директора");
            if (Agent.Phone == null)
                stringBuilder.AppendLine("Не заполнен телефон");
            if (Agent.Email == null)
                stringBuilder.AppendLine("Не заполнена почта");
            if (Agent.Priority == 0)
                stringBuilder.AppendLine("Не заполнен приоритет");

            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString());
            }
            else
            {
                if (Agent.ID == 0)
                {
                    BdConnection.connection.Agent.Add(Agent);
                }
                BdConnection.connection.SaveChanges();
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить агента?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                if (Agent.ProductSale.Count != 0)
                {
                    BdConnection.connection.Agent.Remove(Agent);
                    BdConnection.connection.SaveChanges();
                    NavigationService.Navigate(new MainPage());
                }
                else
                    MessageBox.Show("Нельзя удалить агента с записями о продажах!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void lvSale_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvSale.SelectedItem != null)
            {
                var selectedProduct = lvSale.SelectedItem as ProductSale;

                if (new EditSaleWindow(selectedProduct).ShowDialog().Value)
                {
                    lvSale.ItemsSource = BdConnection.connection.ProductSale.Where(x => x.Agent.ID == Agent.ID).ToList();
                    lvSale.Items.Refresh();
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProductSale productSale = new ProductSale()
            {
                Agent = Agent,
                SaleDate = DateTime.Now
            };

            if (new EditSaleWindow(productSale).ShowDialog().Value)
            {
                lvSale.ItemsSource = BdConnection.connection.ProductSale.Where(x => x.Agent.ID == Agent.ID).ToList();
                lvSale.Items.Refresh();
            }
        }
    }
}
