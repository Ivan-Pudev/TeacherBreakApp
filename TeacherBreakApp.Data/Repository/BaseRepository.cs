namespace TeacherBreakApp.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BaseRepository : IDisposable
    {
        private bool isDisposed = false;
        private readonly TeacherBreakAppDbContext _dbContext;

        protected BaseRepository(TeacherBreakAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected TeacherBreakAppDbContext DbContext
            => _dbContext;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            isDisposed = true;
        }
        protected async Task<int> SaveChangesAsync()
        {
            return await this.DbContext.SaveChangesAsync();
        }

    }
}
