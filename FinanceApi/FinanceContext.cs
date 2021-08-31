using Microsoft.EntityFrameworkCore;

namespace FinanceApi
{
    public class FinanceContext : DbContext
    {
        public FinanceContext(DbContextOptions<FinanceContext> options)
            : base(options)
        {

        }
    }
}
