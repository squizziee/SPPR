using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.Extensions;

namespace WEB_253504_LIANHA.Services.SessionService
{
    public class SessionCartService : ISessionCartService
    {
        private Cart _cart;
        private readonly HttpContext? _context;
        public SessionCartService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _context = httpContextAccessor.HttpContext;
            _cart = _context!.Session.GetCart("cart") ?? new();
        }
        public void AddToCart(Automobile automobile)
        {
            _cart.AddToCart(automobile);
            _context!.Session.Set("cart", _cart);
        }

        public void RemoveItems(int id)
        {
            _cart.RemoveItems(id);
            _context!.Session.Set("cart", _cart);
        }

        public void ClearAll()
        {
            _cart.ClearAll();
            _context!.Session.Set("cart", _cart);
        }

        public Cart GetCart()
        {
            return _cart;
        }

        public int Count { get => _cart.Count; }

        public double TotalPrice
        {
            get => _cart.TotalPrice;
        }
    }
}
