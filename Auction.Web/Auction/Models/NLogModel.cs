using NLog;

namespace Auction.Models
{
    class NLogModel
    {
        protected NLogModel()
        {
            Log = LogManager.GetLogger(GetType().FullName);
        }

        protected Logger Log { get; private set; }
    }
}