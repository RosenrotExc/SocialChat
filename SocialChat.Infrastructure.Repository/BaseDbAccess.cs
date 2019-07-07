using System.Data;

namespace SocialChat.Infrastructure.Data
{
    public class BaseDbAccess
    {
        public IDbConnection Connection { get; }

        public BaseDbAccess(IDbConnection connect)
        {
            Connection = connect;
        }
    }
}
