using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Cookies
{
    public class Class
    {
        public static void AddCookies(string key, string value, int minute, HttpContext context)
        {
            CookieOptions options = new CookieOptions()
            {
                MaxAge = TimeSpan.FromMinutes(minute),
            };
            context.Response.Cookies.Append(key, value, options);
        }

        public static Dictionary<int, OrderDetail> GetCartInfo(HttpContext context)
        {
            string cart = context.Request.Cookies["Cart"];
            if (cart == null)
            {
                return new Dictionary<int, OrderDetail>();
            }
            else
            {
                return JsonSerializer.Deserialize<Dictionary<int, OrderDetail>>(cart);
            }
        }

        public static Account GetAccountFromSession(ISession session)
        {
            string accountString = session.GetString("CustSession");
            if (accountString != null)
            {
                return JsonSerializer.Deserialize<Account>(accountString);
            }
            return null;
        }
    }
}
