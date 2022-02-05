using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
            var affected =
                    await connection.ExecuteAsync
                            ("INSERT INTO Coupon (ProductName,Description,Amount) VALUES (@ProductName,@Description,@Value)",
                                                  new {ProductName=coupon.ProductName,Description=coupon.Description, Value= coupon.Amount });
            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
            var affected =
                    await connection.ExecuteAsync
                            ("DELETE FROM Coupon WHERE ProductName=@ProductName",
                                                  new { ProductName = productName});
            if (affected == 0)
                return false;
            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
            var coupun = await connection.QueryFirstOrDefaultAsync<Coupon>
                         ("Select * from Coupon WHERE ProductName = @productName",new { ProductName = productName});
            if (coupun == null)
                return new Coupon{ProductName = "No Discount",Amount =0,Description = "No Discount Desc"};
            return coupun;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
            var affected =
                    await connection.ExecuteAsync
                            ("UPDATE Coupon SET ProductName=@ProductName,Description=@Description,Amount=@Value WHERE Id=@Id",
                                                  new { ProductName = coupon.ProductName, Description = coupon.Description, Value = coupon.Amount,Id=coupon.Id });
            if (affected == 0)
                return false;
            return true;
        }
    }
}
