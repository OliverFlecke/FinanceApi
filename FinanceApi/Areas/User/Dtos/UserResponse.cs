namespace FinanceApi.Areas.User.Dtos;

public class UserResponse
{
#pragma warning disable CS8618 // Consider declaring the property as nullable.
#pragma warning disable IDE1006
    public string login { get; set; }
    public int id { get; set; }
    public string node_id { get; set; }
    public Uri avatar_url { get; set; }
    public string gravatar_id { get; set; }
    public Uri url { get; set; }
    public Uri html_url { get; set; }
    public Uri followers_url { get; set; }
    public Uri following_url { get; set; }
    public Uri gists_url { get; set; }
    public Uri starred_url { get; set; }
    public Uri subscriptions_url { get; set; }
    public Uri organizations_url { get; set; }
    public Uri repos_url { get; set; }
    public Uri events_url { get; set; }
    public Uri received_events_url { get; set; }
    public string type { get; set; }
    public bool site_admin { get; set; }
    public string name { get; set; }
    public string company { get; set; }
    public Uri blog { get; set; }
    public string location { get; set; }
    public string email { get; set; }
    public bool? hireable { get; set; }
    public string bio { get; set; }
    public string twitter_username { get; set; }
    public int public_repos { get; set; }
    public int public_gists { get; set; }
    public int followers { get; set; }
    public int following { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}