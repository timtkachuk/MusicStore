using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        public void Add(CartItem cartItem)
        {
            var item = Items.SingleOrDefault(p => p.AlbumId == cartItem.AlbumId);
            if (item != null)
                item.Quantity++;
            else
                Items.Add(cartItem);
        }

        public void Add(int AlbumId) => Items.Single(p => p.AlbumId == AlbumId).Quantity++;

        public void DecreaseQuantity(int AlbumId)
        {
            var item = Items.Single(p => p.AlbumId == AlbumId);
            item.Quantity--;
            if (item.Quantity == 0)
                Items.Remove(item);
        }

        public void Remove(int AlbumId) => Items.Remove(Items.Single(p => p.AlbumId == AlbumId));
        public void Clear() => Items = new List<CartItem>();

        public decimal Total
        {
            get
            {
                return Items.Sum(p => p.Amount);
            }
        }

        public decimal Discount
        {
            get
            {
                return Total > 100 ? Total * 0.1m : 0;
            }
        }
        public decimal GrandTotal
        {
            get
            {
                return Total - Discount;
            }
        }
    }
}