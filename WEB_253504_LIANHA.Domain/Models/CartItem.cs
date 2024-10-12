using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_LIANHA.Domain.Entities;

namespace WEB_253504_LIANHA.Domain.Models
{
	public class CartItem
	{
		public Automobile Automobile { get; set; } = default!;
		public int Count { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj is not CartItem) return false;
			return (obj as CartItem)!.Automobile == Automobile;
		}

		public override int GetHashCode()
		{
			return Automobile.GetHashCode() ^ Count.GetHashCode();
		}
	}
}
