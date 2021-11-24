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
            List<List<string>> sqldata = SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                SELECT bludo.id
                FROM bludo
                WHERE bludo.name = '{Title.Text}'
            ", 1);
            if (sqldata.Count == 1)
            {
                SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                UPDATE howto
                SET text = ('{HowTo.Text}')
                WHERE id_bludo = {sqldata[0][0]};
            ", 0);
            } else
            {
                SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                INSERT INTO bludo (name)
                VALUES ('{Title.Text}')
            ", 0);
                List<List<string>> id = SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                SELECT bludo.id
                FROM bludo
                WHERE bludo.name = '{Title.Text}'
            ", 1);
                SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                INSERT INTO howto (text, id_bludo)
                VALUES ('{HowTo.Text}', {id[0][0]})
            ", 0);
                
            }
            updateRecipes();
        }

        private void btn_delete(object sender, RoutedEventArgs e)
        {
            List<List<string>> id = SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                SELECT bludo.id
                FROM bludo
                WHERE bludo.name = '{Title.Text}'
            ", 1);

            SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                DELETE FROM howto
                WHERE howto.id_bludo = {id[0][0]}
                
            ", 0);
            SQLQueryRows(App.Current.Properties["dbname"].ToString(), $@"
                DELETE FROM bludo
                WHERE bludo.id = {id[0][0]}
                
            ", 0);
            Title.Text = "Введите название блюда";
            HowTo.Text = "Введите ингредиенты и способ приготовления";
            updateRecipes();
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
            if (selectedItem >= 0)
            {
                Title.Text = (App.Current.Properties["recipes"] as List<List<string>>)[selectedItem][0];
               HowTo.Text = (App.Current.Properties["recipes"] as List<List<string>>)[selectedItem][1];
            }
            
        }
        /////////////////////////////////////
        private void updateRecipes()
        {
            List<List<string>> sqldata = SQLQueryRows(App.Current.Properties["dbname"].ToString(), @"
                SELECT bludo.name, howto.text
                FROM bludo, howto
                WHERE bludo.id = howto.id_bludo
            ", 2);
            App.Current.Properties["recipes"] = sqldata;
            receptsList.Items.Clear();
            for(int i = 0; i < sqldata.Count; i++)
            {
                receptsList.Items.Add(sqldata[i][0]);
            }
        }
        private List<List<string>> SQLQueryRows(string name, string query, int colCount)
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
                for (int i = 0; i < colCount; i++)
                {
                    try
                    {
                        row.Add(reader.GetString(i));
                    }
                    catch {
                        row.Add(reader.GetInt32(i).ToString());
                    }
                    
                }
                sqldata.Add(row);
            }
            connection.Close();
            return sqldata;
        }
    }
}
