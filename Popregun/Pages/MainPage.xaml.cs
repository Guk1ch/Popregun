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
using System.IO;
using Popregun.DataBase;

namespace Popregun.Pages
{
	/// <summary>
	/// Логика взаимодействия для MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{
		public List<Agent> agents { get; set; }
		public List<Agent> sagents { get; set; }
		public List<AgentType> agentTypes { get; set; }
		public int page = 0;
		public MainPage()
		{
			InitializeComponent();
			agents = BdConnection.connection.Agent.ToList();
			agentTypes = BdConnection.connection.AgentType.ToList();
			agentTypes.Insert(0, new AgentType() { ID = 0, Title = "Все типы" });
			List<string> Sorting = new List<string>()
			{
				"Все",
               "Наименование по возрастанию",
               "Наименование по убыванию",
               "Размер скидки по возрастанию",
               "Размер скидки по убыванию",
               "Приоритет по возрастанию",
               "Приоритет по убыванию",
            };
			cbFiltr.ItemsSource= Sorting;
			cbSort.ItemsSource = agentTypes;
			cbSort.DisplayMemberPath = "Title";
			if (lvMain.Items.Count > 10)
			{
                SetPageNumbers();
            }
			DataContext = this;
		}
		public void SetPageNumbers()
		{
			var pageCount = Math.Round((double)(sagents.Count / 10), MidpointRounding.AwayFromZero);

			spPagination.Children.Clear();

			spPagination.Children.Add(new TextBlock { Text = "<" });
			for (int i = 0; i < pageCount; i++)
			{
				spPagination.Children.Add(new TextBlock { Text = $"{i + 1}" });
			}

			spPagination.Children.Add(new TextBlock { Text = ">" });

			foreach (var child in spPagination.Children)
			{
				(child as UIElement).MouseDown += AgentsListPage_MouseDown;
			}
		}
		private void AgentsListPage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			var text = (sender as TextBlock).Text;
			var pageCount = Math.Round((double)(sagents.Count / 10), MidpointRounding.AwayFromZero);

			if (text == "<")
			{
				if (page > 0)
					page--;
			}
			else if (text == ">")
			{
				if (page < pageCount - 1)
					page++;
			}
			else
			{
				page = int.Parse(text) - 1;
			}

			Filter(false);
		}
		private void cbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            Filter();
        }

		private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            Filter();
        }

		private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
		{
            Filter();
        }

		private void lvMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(lvMain.SelectedItem != null)
			{
				var selItem = lvMain.SelectedItem as Agent;
				NavigationService.Navigate(new AgentPage(selItem, false)); 
			}
		}
		private void Filter(bool isFilterChanged = true)
		{
			if (isFilterChanged) { page = 0; }
			sagents = BdConnection.connection.Agent.ToList();
			if(cbSort.SelectedItem != null)
			{
				var selType = cbSort.SelectedItem as AgentType;
				if(selType.ID != 0)
				{
					sagents = sagents.Where(x => x.AgentType== selType).ToList();
				}
			}

			if(cbFiltr.SelectedItem != null)
			{
				if(cbFiltr.SelectedIndex == 1)
				{
					sagents = sagents.OrderBy(x=> x.Title).ToList();
				}
				else if (cbFiltr.SelectedIndex == 2)
				{
					sagents = sagents.OrderByDescending(x=> x.Title).ToList();
				}
				else if(cbFiltr.SelectedIndex==3)
				{
					sagents = sagents.OrderBy(x=> x.ProductSale).ToList();
				}
				else if(cbFiltr.SelectedIndex == 4)
				{
					sagents =sagents.OrderByDescending(x=> x.ProductSale).ToList();
				}
				else if(cbFiltr.SelectedIndex == 5)
				{
					sagents = sagents.OrderBy(x => x.Priority).ToList();
				}
				else if (cbFiltr.SelectedIndex == 6)
				{
					sagents =sagents.OrderByDescending(x => x.Priority).ToList();
                }
			}

			if(tbSearch.Text.Trim().Length != 0)
			{
				sagents = sagents.Where(x => x.Title.Contains(tbSearch.Text.Trim()) || x.Phone.Contains(tbSearch.Text.Trim()) || x.Email.Contains(tbSearch.Text.Trim())).ToList();
			}

			lvMain.ItemsSource = sagents.Skip(page * 10).Take(10);
			SetPageNumbers();
		}

		private void btnAddAgent_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnEditPriority_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
