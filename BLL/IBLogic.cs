using DAL;

namespace BLL
{
    public interface IBLogic
    {
        void AddFavorite(int MovieId, string UserId);
        void DeleteFavorite(int MovieId, string UserId);
        IEnumerable<Favorite> GetAllFavorites();

        //void AddFilmViewLater(int MovieId);
        //void DeleteFilmViewLater(int MovieId);
        //IEnumerable<FilmViewLater> GetAllFilmViewLater();
    }
}
