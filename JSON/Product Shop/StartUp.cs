using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        //private const string DatasetPath = "../../../Datasets";
        //private const string DatasetResultPath = "../../../Datasets/Results";

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var usersJsonAsString = File.ReadAllText("../../../Datasets/users.json");
            Console.WriteLine(ImportUsers(context, usersJsonAsString));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson); //Deserialize 

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}