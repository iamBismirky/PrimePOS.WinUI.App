namespace PrimePOS.WinUI.Models
{
    public static class TokenStorage
    {
        private static string? _token;

        public static void SetToken(string token)
        {
            _token = token;
        }

        public static string? GetToken()
        {
            return _token;
        }

        public static void Clear()
        {
            _token = null;
        }
    }
}
