using CleanArchitecture.Application.Common.Models;
using System.Net;

namespace CleanArchitecture.Application.Carts
{
    public static class CartsErrors
    {
        public static Error CartNotFoundError => new(HttpStatusCode.NotFound, "Cart Not Found");
        public static Error CartItemNotFoundError => new(HttpStatusCode.NotFound, "Cart Item Not Found");
        public static Error CartEmptyError => new(HttpStatusCode.NotFound, "Cart Empty");

    }
}
