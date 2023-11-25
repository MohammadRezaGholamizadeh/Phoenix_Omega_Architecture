namespace AccessControlLayer.Infrastructure
{
    public interface AccessControlUnitOfWork
    {
        Task Begin();
        void CommitPartial();
        Task Commit();
        void Rollback();
        Task Complete();
    }
}
