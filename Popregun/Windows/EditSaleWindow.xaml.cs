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
using Popregun.DataBase;
using Popregun.Pages;

namespace Popregun.Windows
{
	/// <summary>
	/// Interaction logic for EditSaleWindow.xaml
	/// </summary>
	public partial class EditSaleWindow : Window
	{
		public List<Product> Products { get; set; }
		public ProductSale ProductSale { get; set; }
		public EditSaleWindow()
		{
			InitializeComponent(ProductSale sale);
			ProductSale = sale;
			Products = BdConnection.connection.Product.ToList();

			DataContext = this;
		}
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (ProductSale.Product == null)
                stringBuilder.AppendLine("Выберите продукт");
            else if (ProductSale.SaleDate == null)
                stringBuilder.AppendLine("Заполните дату");
            else if (ProductSale.ProductCount == 0)
                stringBuilder.AppendLine("Запишите количество");

            if (stringBuilder.Length == 0)
            {
                if (ProductSale.ID == 0)
                {
                    BdConnection.connection.ProductSale.Add(ProductSale);
                }

                BdConnection.connection.SaveChanges();
                this.DialogResult = true;
            }
            else
                MessageBox.Show(stringBuilder.ToString());
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить агента?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                BdConnection.connection.ProductSale.Remove(ProductSale);
                BdConnection.connection.SaveChanges();
                this.DialogResult = true;
            }
        }
    }
}
