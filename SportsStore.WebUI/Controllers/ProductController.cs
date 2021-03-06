﻿using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository repository;

        public int pagesize = 4;

        public ProductController(IProductsRepository repository) {
            this.repository = repository;
        }
        // GET: Product
        public ViewResult List(string category,int page = 1)
        {
            //return View(
            //     repository.Products.OrderBy(p => p.ProductID)
            //    .Skip(pagesize * (page - 1))
            //    .Take(pagesize));

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                            .Where(p=> String.IsNullOrEmpty(category) || p.Category == category)
                            .OrderBy(p => p.ProductID)
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pagesize,
                    TotalItems = String.IsNullOrEmpty(category) ?
                            repository.Products.Count() :
                            repository.Products.Where(e => e.Category == category).Count()

                },
                CurrentCategory = category
            };

            return View(model);

        }

        public FileContentResult GetImage(int productID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            if (product != null) {
                return File(product.ImageData, product.ImageMimeType);
            }
            return null;
        }
    }
}