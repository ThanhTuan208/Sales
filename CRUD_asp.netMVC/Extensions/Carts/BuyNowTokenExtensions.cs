using CRUD_asp.netMVC.DTO.Cart;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CRUD_asp.netMVC.Extensions.Carts
{
    public class BuyNowTokenExtensions
    {
        private readonly IConfiguration _config;

        public BuyNowTokenExtensions(IConfiguration config) => _config = config;

        public string SecretKey
        {
            get
            {
                string? key = _config["BuyNowToken:SecretKey"];

                if(string.IsNullOrEmpty(key))
                {
                    throw new InvalidOperationException("BuyNowToken:SecretKey is not configured in appsettings.json");
                }

                return key!;
            }
        }

        // Tao token tu object
        public string GenerateToken(int productId, string color, string size, int qty)
        {
            var expires = DateTime.UtcNow.AddMinutes(6);

            var data = new BuyNowData
            {
                ProductId = productId,
                Color = color,
                Size = size,
                Quantity = qty,
                Expired = expires
            };

            var jsonData = JsonSerializer.Serialize(data);
            var signature = ComputeHmacSignature(jsonData);

            string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData))
                                        .Replace('+', '-')
                                        .Replace('/', '_')
                                        .TrimEnd('=');

            return $"{signature}.{base64Data}";
        }

        // Kiem tra token
        public BuyNowData ValidateToken(string token)
        {
            if(string.IsNullOrEmpty(token) || !token.Contains('.'))
            {
                throw new InvalidOperationException("Invalid token format");
            }

            try
            {
                var parts = token.Split('.', 2);
                var oldSignature = parts[0];
                var encodedData  = parts[1];

                string decodeJsonData = Encoding.UTF8.GetString(
                Convert.FromBase64String(encodedData.Replace('-', '+').Replace('_', '/').PadRight(encodedData.Length + (4 - encodedData.Length % 4) % 4, '=')));

                var newSignature = ComputeHmacSignature(decodeJsonData);

                if (newSignature != oldSignature)
                    return null!;

                var data = JsonSerializer.Deserialize<BuyNowData>(decodeJsonData);

                if(data == null || data.Expired < DateTime.UtcNow)
                    return null!;

                return data;
            }
            catch
            {
                return null!;
            }
        }


        // Tao signature tu jsonData
        private string ComputeHmacSignature(string jsonData)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(jsonData));

            return Convert.ToBase64String(hash)
                          .Replace('+', '-')
                          .Replace('/', '_')
                          .TrimEnd('=');

        }
    }
}
