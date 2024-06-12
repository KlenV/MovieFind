

namespace DAL
{
    public interface IUnitOfWork
    {
        GenericRepository<Favorite> FavoritesRepository { get; }
        GenericRepository<FilmViewLater> FilmViewLaterRepository { get; }

        void SaveChanges();
    }
}
