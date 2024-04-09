using MessengerDatabaseService.DataContexts;

namespace MessengerDatabaseService.Services
{
    public class DatabaseModelService
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseModelService(DatabaseContext databaseContext) 
        {
            _databaseContext = databaseContext;
        }
    }
}
