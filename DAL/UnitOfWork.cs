using System;

namespace DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new();
        
        private GenericRepository<Favorite> ? _favoritesRepository;
        private GenericRepository<FilmViewLater> ?_filmViewLaterRepository;


        public GenericRepository<Favorite> FavoritesRepository
        {
            get
            {
                if (_favoritesRepository == null)
                    _favoritesRepository = new GenericRepository<Favorite>(_context);
                return _favoritesRepository;
            }
        }

        public GenericRepository<FilmViewLater> FilmViewLaterRepository
        {
            get
            {
                if (_filmViewLaterRepository == null)
                    _filmViewLaterRepository = new GenericRepository<FilmViewLater>(_context);
                return _filmViewLaterRepository;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing) 
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}