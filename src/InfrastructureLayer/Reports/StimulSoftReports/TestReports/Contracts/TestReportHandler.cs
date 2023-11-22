using ServiceLayer.Setups.ServicecInterfaces;

namespace InfrastructureLayer.Reports.StimulSoftReports.TestReports.Contracts
{
    public interface TestReportHandler : IService
    {
        Task<Stream> GetPdf();
    }
}
