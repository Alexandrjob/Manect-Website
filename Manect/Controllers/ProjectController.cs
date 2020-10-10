using Manect.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manect.Controllers
{
    public class ProjectController: Controller
    {
        private readonly IDataRepository _dataRepository;

        public ProjectController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        public async Task<IActionResult> IndexAsync(int projectId)
        {
            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            return View(project);
        }
    }
}
