using Microsoft.EntityFrameworkCore;
using PosInstaller.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool prova = false;
        bool regolare = true;
        List<Machine> machines = new List<Machine>();

        public MainWindow()
        {
            InitializeComponent();
            using (var dbContext = new PosInstallerContext())
            {
                machines.Clear();
                machines = dbContext.Machines.ToList();
                Nmachines.ItemsSource = machines;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)//tasto salva, prende le informazoni scritte nella form e le inserisce nel database
        {
            try
            {
                using (var dbContext = new PosInstallerContext())
                {
                    Nmachines.Items.Refresh();
                    Nmachines.ItemsSource = machines;
                    regolare = true;
                    string nomemacchina = txtNome_macchina.Text;
                    string username = txtUser.Text;
                    string Password = txtPassword.Text;
                    //string tipo = combobox.SelectionBoxItem.ToString();
                    string tipo = ((ComboBoxItem)comboboxTipo.SelectedItem).Tag.ToString();
                    Machine machineToCreate = new Machine();

                  
                   if (tipo != "" && nomemacchina != "" && username != "" && Password != "")
                   {
                        if (tipo == "cl")
                        {
                            Log.Text = "Client";

                            if (nomemacchina == Environment.MachineName)
                            {
                                Log.Text = "" + "nome macchina corretto";
                            }
                            else
                            {
                                Log.Text = "" + "il nome macchina non corrisponde alla macchina attuale";
                                txtNome_macchina.Clear();
                                regolare = false;
                            }
                        }

                        Machine machinetomodifide = null;
                        machinetomodifide = dbContext.Machines.Where(machine => machine.Type == ((ComboBoxItem)comboboxTipo.SelectedItem).Tag.ToString()).FirstOrDefault();
                        if (machinetomodifide != null && regolare == true)
                        {
                            machinetomodifide.Username = username;
                            machinetomodifide.MachineName = nomemacchina;
                            machinetomodifide.Password = Password;
                            dbContext.SaveChanges();
                            prova = true;
                            machines = dbContext.Machines.ToList();
                            Nmachines.ItemsSource = machines;
                            prova = false;                            
                            Nmachines.Items.Refresh();                                                            
                        }
                        else
                        {
                            if (regolare == true)
                            {
                                prova = false;
                                machineToCreate.Type = tipo;
                                machineToCreate.Username = username;
                                machineToCreate.Password = Password;
                                machineToCreate.MachineName = nomemacchina;

                                dbContext.Add(machineToCreate);
                                dbContext.SaveChanges();
                                txtUser.Clear();
                                txtPassword.Clear();
                                txtNome_macchina.Clear();
                                Log.Text = "aggiunto in modo corretto";
                                machines.Add(machineToCreate);
                                Nmachines.Items.Refresh();                                
                            }
                        }                       
                    }
                    else
                    {
                        Log.Text = "inserire tutti i campi";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Text = ex.Message;
            }
        }
        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void btOks_Click(object sender, RoutedEventArgs e)//tasto per la tastiera a video
        {
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
            try
            {
                processStartInfo.FileName = "C:\\WINDOWS\\system32\\OSK.exe";
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(processStartInfo);
            }
            catch (System.Exception exception)
            {
                Log.Text = exception.Message;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//tasto reset, elimina i campi server e client dal database
        {
            if (MessageBox.Show("resettare il database?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                txtNome_macchina.Clear();
                txtUser.Clear();
                txtPassword.Clear();
                machines.Clear();
                Machine machineServer = new Machine();
                Machine machineclient = new Machine();
                using (var dbContext = new PosInstallerContext())
                {
                    //List<Machine> machines = new List<Machine>();
                    machineclient = dbContext.Machines.Where(machine => machine.Type == "cl").FirstOrDefault();  //assegna a machineclient il machine.type "cl" nonchè la chiave univoca 
                    if (machineclient != null)
                    {
                        dbContext.Machines.Remove(machineclient);
                        dbContext.SaveChanges();
                    }
                    machineServer = dbContext.Machines.Where(machine => machine.Type == "se").FirstOrDefault();
                    if (machineServer != null)
                    {
                        dbContext.Machines.Remove(machineServer);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        private void Nmachines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (prova == false)
            {
                //((Machine)Nmachines.SelectedItem).Type.ToString();
                //string tipo = comboboxTipo.SelectionBoxItem.ToString();
                string tipo = ((Machine)Nmachines.SelectedItem).Type.ToString();
                string nomemacchina = ((Machine)Nmachines.SelectedItem).MachineName.ToString();
                string username = ((Machine)Nmachines.SelectedItem).Username.ToString();
                string password = ((Machine)Nmachines.SelectedItem).Password.ToString();
                using (var dbContext = new PosInstallerContext())

                    if (tipo == "se")
                    {
                        comboboxTipo.SelectedItem = SERVER;
                    }
                else if(tipo == "cl")
                    {
                        comboboxTipo.SelectedItem = CLIENT;
                    }
               
                txtNome_macchina.Text = nomemacchina;
                txtPassword.Text = password;
                txtUser.Text = username;
            }
            
        }

        private void cancella_Click(object sender, RoutedEventArgs e)
        {
            txtNome_macchina.Clear();
            txtUser.Clear();
            txtPassword.Clear();
            machines.Clear();
            comboboxTipo.SelectedItem = null;
        }
        // private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //     using (var dbContext = new PosInstallerContext())
        //     {


        //         var row = ItemsControl.ContainerFromElement((DataGrid)sender,
        //                                 e.OriginalSource as DependencyObject) as DataGridRow;

        //         if (row == null) return;

        //         Machine machinetomodifide = new Machine();
        //         foreach (Machine machin in machines)
        //         {
        //             machinetomodifide = dbContext.Machines.Where(machine => machine.Type == machin.Type).FirstOrDefault();

        //                 txtNome_macchina.Text = machin.MachineName;
        //                 txtPassword.Text = machin.Password;
        //                 txtUser.Text = machin.Username;


        //         }
        //   }

        // }
        //Machine machineclient = new Machine();
        //Machine machineServer = new Machine();

        //machineclient = dbContext.Machines.Where(machine => machine.Type == "cl").FirstOrDefault();
        //machineServer = dbContext.Machines.Where(machine => machine.Type == "se").FirstOrDefault();
        //                    if (machineclient != null)
        //                    {
        //                        machines.Add(new Machine() { Type = machineclient.Type, MachineName = machineclient.MachineName, Username = machineclient.Username, Password = machineclient.Password });
        //                        Nmachines.ItemsSource = machines;
        //                    }
        //                    if (machineServer != null)
        //                    {
        //                        machines.Add(new Machine() { Type = machineServer.Type, MachineName = machineServer.MachineName, Username = machineServer.Username, Password = machineServer.Password });
        //                        Nmachines.ItemsSource = machines;
        //                    }

    }
}

