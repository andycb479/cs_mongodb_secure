using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LabCsMongoDB.Data.Implementation;
using LabCsMongoDB.Data.Models;

namespace LabCsMongoDB.UI
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          private UsersRepository UsersRepository { get; set; }
          public MainWindow()
          {
               UsersRepository = new UsersRepository();
               InitializeComponent();
          }

          private async void FetchDataHandler(object sender, RoutedEventArgs e)
          {
               RenderTasks();
          }

          private void RenderTasks()
          {
               TaskList.Items.Clear();
               // foreach (var task in Data)
               // {
               //      TaskList.Items.Add(new TextBlock() { Text = task.Name });
               // }
          }

          private void SendDataToEmail(object sender, RoutedEventArgs e)
          {
               // try
               // {
               //      if (!Data.Any()) return;
               //
               //      MailMessage mail = new MailMessage();
               //      SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
               //
               //      mail.From = new MailAddress("trellosync2@gmail.com");
               //      mail.To.Add("andyciobanuu@gmail.com");
               //      mail.Subject = "Tasks from To Do App";
               //      var tasksAsString = new StringBuilder($"Your tasks for {DateTime.Now.ToShortDateString()}\n");
               //      foreach (var task in Data)
               //      {
               //           tasksAsString.AppendLine("\t" + task.Name);
               //      }
               //      mail.Body = tasksAsString.ToString();
               //
               //      SmtpServer.Port = 587;
               //      SmtpServer.Credentials = new System.Net.NetworkCredential("trellosync2@gmail.com", "trellosync2021");
               //      SmtpServer.EnableSsl = true;
               //
               //      SmtpServer.Send(mail);
               //      MessageBox.Show("mail Send");
               // }
               // catch (Exception ex)
               // {
               //      MessageBox.Show(ex.ToString());
               // }
          }

          private async void AddTaskHandler(object sender, RoutedEventArgs e)
          {
               await UsersRepository.AddUserAndEncryptData(new User
               {
                    Id = default,
                    Name = "Andy",
                    Password = "andy",
                    CardNumber = "1213-1231-1213-1213",
                    Email = "andy.test@gmail.com",
               });
          }
     }
}
