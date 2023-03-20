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

namespace Popregun.Windows
{
	/// <summary>
	/// Interaction logic for EditPriorityWindow.xaml
	/// </summary>
	public partial class EditPriorityWindow : Window
	{
		public List<Agent> EditAgent { get; set; }
		public EditPriorityWindow(List<Agent> agents)
		{
			InitializeComponent();
			var maxPriority = agents.Max(x => x.Priority);
			tbPriority.Text = maxPriority.ToString();
			EditAgent = agents;
		}
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(tbPriority.Text))
			{
				foreach (Agent agent in EditAgent)
				{
					agent.Priority = Convert.ToInt32(tbPriority.Text.Trim());
				}

				BdConnection.connection.SaveChanges();
				DialogResult = true;
			}
		}
	}
}
