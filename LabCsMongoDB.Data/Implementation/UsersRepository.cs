using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using LabCsMongoDB.Data.Encryption;
using LabCsMongoDB.Data.Models;

namespace LabCsMongoDB.Data.Implementation
{
     public class UsersRepository : MongoRepository<User>
     {
          private Encryptor Encryptor { get; set; }
          public UsersRepository() : base()
          {
               var password = GetPassword();
               Encryptor = new Encryptor(password);
          }

          public async Task AddUserAndEncryptData(User user)
          {
               var userWithEncryptedData = new User
               {
                    Id = default,
                    Name = user.Name,
                    Password = Encryptor.Encrypt(user.Password),
                    CardNumber = Encryptor.Encrypt(user.CardNumber),
                    Email = Encryptor.Encrypt(user.Email),
               };

               await InsertOneAsync(userWithEncryptedData);
          }

          public List<User> GetUsersWithEncryptedData()
          {
               var usersEncrypted = AsQueryable().ToList();
               var usersDecrypted = usersEncrypted.Select(x => new User
               {
                    Id = x.Id,
                    Name = x.Name,
                    Password = Encryptor.Decrypt(x.Password),
                    CardNumber = Encryptor.Decrypt(x.CardNumber),
                    Email = Encryptor.Decrypt(x.Email)
               }).ToList();
               return usersDecrypted;
          }

          private static SecureString GetPassword()
          {
               var password = new SecureString();

               password.AppendChar((char)80); // P
               password.AppendChar((char)97); // a
               password.AppendChar((char)115); // s
               password.AppendChar((char)115); // s
               password.AppendChar((char)119); // w
               password.AppendChar((char)111); // o
               password.AppendChar((char)114); // r
               password.AppendChar((char)100); // d
               password.AppendChar((char)49); // 1

               return password;
          }
     }
}
