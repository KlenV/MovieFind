using DAL;
using Ninject;

namespace BLL
{
    public class BusinessLogic : IBLogic
    {
        readonly IKernel ikernel = new StandardKernel(new IoCCon());
        //Favorite
        public void AddFavorite(int MovieId, string UserId)
        {
            var DB = ikernel.Get<IUnitOfWork>();

            Favorite NewFavorite = new()
            {
                MovieID = MovieId,
                UserID = UserId
            };

            try
            {
                DB.FavoritesRepository.Add(NewFavorite);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void DeleteFavorite(int MovieId, string UserId)
        {
            var DB = ikernel.Get<IUnitOfWork>();
            var favorites = DB.FavoritesRepository.GetAll();

            foreach (Favorite p in favorites)
            {
                if (p.MovieID == MovieId)
                {
                    DB.FavoritesRepository.Delete(p);
                }
            }
            try
            {
                DB.SaveChanges();
            }
            catch
            {
            }
        }

        public IEnumerable<Favorite> GetAllFavorites()
        {
            var DB = ikernel.Get<IUnitOfWork>();
            var favorites = DB.FavoritesRepository.GetAll();
            return favorites;
        }

        // FilmViewLater
    }
}