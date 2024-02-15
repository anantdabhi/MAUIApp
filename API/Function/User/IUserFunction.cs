
namespace ChatAppApi.Function.User;

public interface IUserFunction
{
    User? Authenticate(string loginId, string password);

    User GetUserById(int id);
}