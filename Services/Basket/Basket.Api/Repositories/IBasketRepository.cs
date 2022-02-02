using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string usesrName);
    }

    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _rediCache;
        public BasketRepository(IDistributedCache distributedCache)
        {
            this._rediCache = distributedCache;
        }
        public async Task DeleteBasket(string usesrName)
        {
            await _rediCache.RemoveAsync(usesrName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var item = await _rediCache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(item))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(item);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _rediCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }
    }
}
