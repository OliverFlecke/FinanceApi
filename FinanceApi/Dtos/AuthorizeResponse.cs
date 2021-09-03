namespace FinanceApi.Dtos
{
    public class AuthorizeResponse
    {
#pragma warning disable CS8618 // Consider declaring the property as nullable.
#pragma warning disable IDE1006 // These words must begin with upper case characters
        public string access_token { get; set; }
        public string scopes { get; set; }
        public string token_type { get; set; }
    }
}
