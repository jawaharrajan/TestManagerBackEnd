using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface ISequenceRepository : IGenericRepository<SequenceValue, int>
    {
        Task<int> GetNextValueFromSequence(string sequenceName);
    }
}
