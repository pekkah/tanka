namespace Web.Infrastructure.Authentication
{
    using Documents;

    public interface ILoginService
    {
        bool Login(string userName, string password, out User user);
    }
}