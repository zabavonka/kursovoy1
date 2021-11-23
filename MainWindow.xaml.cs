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
using System.Data.SQLite;

namespace kursovoy1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void LogInForm_Closing(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void GetDBName()
        {

        }
        void Registr(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["username"] = Username.Text;
            Application.Current.Properties["dbname"] = Username.Text + ".db";
            var exist = File.Exists(Username.Text + ".db");
            if (exist == true)
            {
                MessageBox.Show("Такая база данных уже существует!");
            }
            else
            {
                SQLiteConnection.CreateFile(App.Current.Properties["dbname"].ToString());
                SQLQuery(App.Current.Properties["dbname"].ToString(), @"CREATE TABLE [products] (
                    [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [name] TEXT NOT NULL,
                    [kkl] integer NOT NULL);
                CREATE TABLE [bludo] (
                    [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [name] TEXT NOT NULL);
                CREATE TABLE [howto] (
                    [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [id_bludo] integer NOT NULL,
                    [text] TEXT NOT NULL,
                    CONSTRAINT [fk_howto_bludo_1] FOREIGN KEY([id_bludo]) REFERENCES [bludo]([id]) ON DELETE CASCADE);
                CREATE TABLE [usedproducts] (
                    [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [id_bludo] integer NOT NULL,
                    [id_product] integer NOT NULL,
                    [gramm] integer NOT NULL,
                    CONSTRAINT [fk_usedproducts_products_1] FOREIGN KEY([id_product]) REFERENCES [products]([id]) ON DELETE RESTRICT,
                    CONSTRAINT [fk_usedproducts_bludo_1] FOREIGN KEY([id_bludo]) REFERENCES [bludo]([id]) ON DELETE CASCADE)");
                MessageBox.Show(string.Format("{0}", App.Current.Properties["dbname"].ToString()));
                
            }
                
        }
        void SQLQuery(string name, string query)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", name));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
        }
        void LogIn(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["username"] = Username.Text;
            Application.Current.Properties["dbname"] = Username.Text + ".db";
            var exist = File.Exists(App.Current.Properties["dbname"].ToString());
            if (exist == true)
            {
                General general = new General();
                general.Show();
            } else
            {
                MessageBox.Show("Такой базы данных нет!");
            }
                

        }
    }
}
