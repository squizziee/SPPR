﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WEB_253504_LIANHA.Domain.Models;

namespace WEB_253504_LIANHA.Extensions
{
	public static class SessionExtensions
	{
		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		public static Cart? GetCart(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default : JsonSerializer.Deserialize<Cart>(value);
		}
	}
}
