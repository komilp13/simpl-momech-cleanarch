using Simpl.Mobile.Mechanic.Core.Domain;

namespace Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories;

public interface IWorkRequestRepository
{
    Task<string> InsertWorkRequestAsync(WorkRequest workRequest);
    Task<WorkRequest?> GetWorkRequestByIdAsync(string workRequestId);
}