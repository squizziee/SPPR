using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_LIANHA.Domain.Entities;

namespace WEB_253504_LIANHA.Domain.Models
{
	public class Cart
	{
		public List<CartItem> CartItems { get; set; } = [];

		public virtual void AddToCart(Automobile automobile)
		{
			var newItem = new CartItem { Automobile = automobile, Count = 1 };
			var item = CartItems.Find(ci => ci.Automobile.Id == automobile.Id);
			if (item != null)
			{
				item.Count++;
			}
			else
			{
				CartItems.Add(newItem);
			}
		}

		public virtual void RemoveItems(int id)
		{
			var item = CartItems.Find(ci => ci.Automobile.Id == id);
			if (item == null)
			{
				return;
			}
			if (item!.Count == 1) {
				CartItems.Remove(item);
			}
			else
			{
				item.Count--;
			}
		}

		public virtual void ClearAll()
		{
			CartItems.Clear();
		}

		public int Count { get => CartItems.Sum(ci => ci.Count); }

		public double TotalPrice
		{
			get => CartItems.Sum(ci => ci.Automobile.PriceInUsd * ci.Count);
		}
	}
}
