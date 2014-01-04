namespace Web.Infrastructure
{
    using Documents;

    public interface ILoginService
    {
        bool Login(string userName, string password, out User user);
    }
}