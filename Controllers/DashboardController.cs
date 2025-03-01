using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moneygram_api.Services.Interfaces;
using System.Threading.Tasks;

namespace moneygram_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("funds-sent-overview")]
        public async Task<IActionResult> GetFundsSentOverview()
        {
            var data = await _dashboardService.GetFundsSentOverviewAsync();
            return Ok(data);
        }

        [HttpGet("transaction-status")]
        public async Task<IActionResult> GetTransactionStatus()
        {
            var data = await _dashboardService.GetTransactionStatusAsync();
            return Ok(data);
        }

        [HttpGet("top-recipient-countries")]
        public async Task<IActionResult> GetTopRecipientCountries()
        {
            var data = await _dashboardService.GetTopRecipientCountriesAsync();
            return Ok(data);
        }

        [HttpGet("recent-transactions")]
        public async Task<IActionResult> GetRecentTransactions()
        {
            var data = await _dashboardService.GetRecentTransactionsAsync();
            return Ok(data);
        }
    }
}