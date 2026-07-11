namespace NotificationService.InterFaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
}
