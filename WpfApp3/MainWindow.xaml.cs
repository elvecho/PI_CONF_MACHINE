using PosInstaller.DataAccess.Models;
using System;
using System.Collections.Generic;
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
 
        bool regolare = true;

        public MainWindow()
        {
            InitializeComponent();       
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                using (var dbContext = new PosInstallerContext())
                {
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
                                Log.Text = "nome macchina corretto";                                  
                            }
                            else
                            {
                                Log.Text = "il nome macchina non corrisponde alla macchina attuale";
                                txtNome_macchina.Clear();
                                regolare = false;
                            }
                        }
                        
                        if (regolare == true)
                        {
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

        private void btOks_Click(object sender, RoutedEventArgs e)
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
    }
    }

