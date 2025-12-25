using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public interface IReportService
{
    public Task<MemoryStream> GetCollects(CollectFilterViewModel filters);
}