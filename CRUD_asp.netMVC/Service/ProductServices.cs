//using CRUD_asp.netMVC.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace CRUD_asp.netMVC.Service
//{
//    public class ProductServices
//    {
//        public readonly List<Products> products;
//        public ProductServices()
//        {
//            products = GetProducts();
//        }

//        public List<Products> GetProducts() => new()
//            {
//                new Products { ID = 2, Name = "Iphone 12",picturePath ="", Price = 200, Description = "Iphone 12 prime 2", Created = new DateTime(2025), CategoryID = "1"},
//                new Products { ID = 1, Name = "Iphone 11",picturePath ="", Price = 100, Description = "Iphone 11 prime 1",Created = new DateTime(2025),  CategoryID = "1"},
//                new Products { ID = 3, Name = "Iphone 13",picturePath ="", Price = 300, Description = "Iphone 13 prime 3",Created = new DateTime(2025),  CategoryID = "1"},
//                new Products { ID = 4, Name = "Iphone 14",picturePath ="", Price = 400, Description = "Iphone 14 prime 4",Created = new DateTime(2025),  CategoryID = "2" },
//                new Products { ID = 5, Name = "Iphone 15",picturePath ="", Price = 500, Description = "Iphone 15 prime 5",Created = new DateTime(2025),  CategoryID = "2" },
//                new Products { ID = 6, Name = "Iphone 16",picturePath ="", Price = 600, Description = "Iphone 16 prime 6",Created = new DateTime(2025),  CategoryID = "2" },
//                new Products { ID = 7, Name = "Iphone 16 plus",picturePath ="", Price = 625, Description = "Iphone 16 prime 7",Created = new DateTime(2025), CategoryID = "3"},
//                new Products { ID = 8, Name = "Iphone 16 promax",picturePath ="", Price = 650, Description = "Iphone 16 prime 8" ,Created = new DateTime(2025), CategoryID = "3"},
//            };

//        public Products? FindPro(int? id) => products.FirstOrDefault(f => f.ID == id);

//        public void AddPro(Products product) => products.Add(product);

//        public void DeletePro(Products product) => products.RemoveAll(f => f.ID == product.ID);

//        public bool UpdatePro(Products product)
//        {
//            var index = products.FindIndex(f => f.ID == product.ID);

//            if (index > -1)
//            {
//                products[index] = product;
//                return true;
//            }
//            return false;
//        }

//    }
//}
