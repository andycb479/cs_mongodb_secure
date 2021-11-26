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
          public List<User> Data { get; set; }
          private UsersRepository UsersRepository { get; set; }
          public MainWindow()
          {
               Data = new List<User>();
               UsersRepository = new UsersRepository();
               InitializeComponent();
          }

          private void FetchDataHandler(object sender, RoutedEventArgs e)
          {
               Data = UsersRepository.GetUsersWithEncryptedData();
               RenderTasks();
          }

          private void RenderTasks()
          {
               UserList.Items.Clear();
               foreach (var user in Data)
               {
                    UserList.Items.Add(new TextBlock()
                    {
                         Text =
                              $"Id: {user.Id}, Name: {user.Name}, Email: {user.Email}, Password: {user.Password}, Card Number: {user.CardNumber}"
                    });
               }
          }

          private async void AddTaskHandler(object sender, RoutedEventArgs e)
          {
               await UsersRepository.AddUserAndEncryptData(new User
               {
                    Id = default,
                    Name = "Bill Gates",
                    Password = "gatesMicrosoftTheBest",
                    CardNumber = "9942-3662-2222-0007",
                    Email = "bill.the.best@gmail.com",
               });
               MessageBox.Show("User added.");
          }
     }
}
