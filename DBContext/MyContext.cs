using Microsoft.EntityFrameworkCore;

namespace MyWebsite
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

    }
}