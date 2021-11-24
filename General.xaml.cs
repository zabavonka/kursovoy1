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
using System.IO;
using System.Data.SQLite;

namespace kursovoy1
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    
    public partial class General : Window
    {
        public General()
        {
            InitializeComponent();
            updateRecipes();
        }
        private void Form_Closing(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btn_save(object sender, RoutedEventArgs e)//TODO
        {
            //string[] defData = { "a", "b" };

            //receptsList.Items.Add(defData[0]);
        }
        private void btn_newRecipe(object sender, RoutedEventArgs e)
        {
            Title.Text = "Введите название блюда";
            HowTo.Text = "Введите ингредиенты и способ приготовления";
        }
        private void list_select(object sender, RoutedEventArgs e)
        {
            //ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            int selectedItem = (sender as ListBox).SelectedIndex;
            Title.Text = (App.Current.Properties["recipes"] as List<List<string>>)[selectedItem][0];
            HowTo.Text = (App.Current.Properties["recipes"] as List<List<string>>)[selectedItem][1];
        }
        /////////////////////////////////////
        private void updateRecipes()
        {
            List<List<string>> sqldata = SQLQueryRows(App.Current.Properties["dbname"].ToString(), @"
                SELECT bludo.name, howto.text
                FROM bludo, howto
                WHERE bludo.id = howto.id_bludo
            ");
            App.Current.Properties["recipes"] = sqldata;
            receptsList.Items.Clear();
            for(int i = 0; i < sqldata.Count; i++)
            {
                receptsList.Items.Add(sqldata[i][0]);
            }
        }
        private List<List<string>> SQLQueryRows(string name, string query)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", name));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = query;
            var reader = command.ExecuteReader();
            List<List<string>> sqldata = new List<List<string>>();
            while (reader.Read())
            {
                List<string> row = new List<string>();
                row.Add(reader.GetString(0));
                row.Add(reader.GetString(1));
                sqldata.Add(row);
            }
            connection.Close();
            return sqldata;
        }
    }
}
