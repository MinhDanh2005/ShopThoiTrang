using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopThoiTrang.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Xử lý vòng lặp
                MaxDepth = 128 // Tăng độ sâu tối đa nếu cần
            };
            session.SetString(key, JsonSerializer.Serialize(value, options));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null) return default;

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return JsonSerializer.Deserialize<T>(value, options);
        }
    }
}