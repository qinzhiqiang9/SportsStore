using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";
        public object BindModel(
            ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if (controllerContext.RequestContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.RequestContext.HttpContext.Session[sessionKey];
                if (cart == null)
                {
                    cart = new Cart();
                    controllerContext.RequestContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}