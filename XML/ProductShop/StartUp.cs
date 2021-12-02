using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //Initial setup
            var DatasetsPath = "../../../Datasets";

            var db = new ProductShopContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Mapper.Initialize(config =>
            {
                config.AddProfile<ProductShopProfile>();
            });



            //Query 1. Import Users
            //When we import we need to convert xml to string first!!!
            var usersXmlAsString = File.ReadAllText($"{DatasetsPath}/users.xml"); 
            ImportUsers( db, usersXmlAsString);


            //Query 2. Import Products
            var productXmlAsString = File.ReadAllText($"{DatasetsPath}/products.xml");
            ImportProducts( db, productXmlAsString);



            //Query 3. Import Categories
            var categoriesXmlAsString = File.ReadAllText($"{DatasetsPath}/categories.xml");
            Console.WriteLine(ImportCategories(db, categoriesXmlAsString));

        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            //Create new XmlSerializer, set what type to be imported into, create and set new Root Attribute

            var XmlSerializer = new XmlSerializer(typeof(ImportUsersDto[]), new XmlRootAttribute("Users"));

            //1. Use Deserialize method on the Serializer; 2. the input for this method new StringReader from inputXml
            //3. Cast it to the result type;

            var usersDtos = (ImportUsersDto[])XmlSerializer.Deserialize(new StringReader(inputXml));

            //Map the DTO to it's corresponding class. 

            var users = Mapper.Map<User[]>(usersDtos);

            //Add the new range to the DB.

            context.Users.AddRange(users);

            //Save changes in DB.
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productDtos = (ImportProductDto[])serializer.Deserialize(new StringReader(inputXml));

            //var products = Mapper.Map<Product[]>(productDtos);

            var products = new List<Product>();

            foreach (var dto in productDtos)
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    SellerId = dto.SellerId,
                    BuyerId = dto.BuyerId
                };

                products.Add(product);
            }


            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count}"; 
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoriesDto[]), new XmlRootAttribute("Categories"));

            var categoryDtos = (ImportCategoriesDto[])serializer.Deserialize(new StringReader(inputXml));

            //var categories = Mapper.Map<Category[]>(categoryDtos);

            var categories = new List<Category>();

            foreach (var dto in categoryDtos)
            {
                var category = new Category
                {
                    Name = dto.Name
                };

                categories.Add(category);
            }

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

    }
}