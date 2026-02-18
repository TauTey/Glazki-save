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

namespace Козин_Глазки_save
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();
            var currentAgents = KozinEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgents;

            ComboSorting.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;

            UpdateAgents();
        }

        private void UpdateAgents()
        {
            var context = KozinEntities.GetContext();
            var currentAgents = context.Agent.ToList();

            // Поиск по тексту (в названии или телефоне)
            if (!string.IsNullOrWhiteSpace(TBoxSearch.Text))
            {
                var searchText = TBoxSearch.Text.ToLower();
                currentAgents = currentAgents.Where(a =>
                    (a.Title.ToLower().Contains(TBoxSearch.Text.ToLower())) ||
                    (a.Phone.ToLower().Contains(TBoxSearch.Text.ToLower())) ||
                    (a.Email.ToLower().Contains(TBoxSearch.Text.ToLower())) ||
                    (a.currentnumber.ToLower().Contains(TBoxSearch.Text.ToLower()))
                ).ToList();
            }

            // ComboType
            if (ComboType.SelectedIndex > 0) // 0 - "Все типы"
            {
                var selectedType = (ComboType.SelectedItem as TextBlock)?.Text;
                if (!string.IsNullOrEmpty(selectedType))
                {
                    currentAgents = currentAgents.Where(a => a.AgentTypeName == selectedType).ToList();
                }
            }

            // ComboSorting
            switch (ComboSorting.SelectedIndex)
            {
                case 1: // Наименование от А до Я
                    currentAgents = currentAgents.OrderBy(a => a.Title).ToList();
                    break;
                case 2: // Наименование от Я до А
                    currentAgents = currentAgents.OrderByDescending(a => a.Title).ToList();
                    break;
                case 3: // Скидка по возрастанию
                    currentAgents = currentAgents.OrderBy(a => a.Discount).ToList();
                    break;
                case 4: // Скидка по убыванию
                    currentAgents = currentAgents.OrderByDescending(a => a.Discount).ToList();
                    break;
                case 5: // Приоритет по возрастанию
                    currentAgents = currentAgents.OrderBy(a => a.Priority).ToList();
                    break;
                case 6: // Приоритет по убыванию
                    currentAgents = currentAgents.OrderByDescending(a => a.Priority).ToList();
                    break;
            }
            AgentListView.ItemsSource = currentAgents;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
