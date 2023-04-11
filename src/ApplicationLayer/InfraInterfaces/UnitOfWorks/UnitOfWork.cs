namespace ApplicationLayer.InfraInterfaces.UnitOfWorks
{
    public interface UnitOfWork
    {
        Task SaveAllChangesAsync();
        void SaveAllChanges();
        Task BeginTransaction();
        Task Commit();
        Task CommitPartial();
    }
}
