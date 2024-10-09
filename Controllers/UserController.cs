using ElasticAPI.Models;
using ElasticAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IElasticSearchService<UserModel> _elasticService;

        public UserController(IElasticSearchService<UserModel> elasticService) { _elasticService = elasticService; }


        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetUserID(string id)
        {
            var result = await _elasticService.GetDocument(id);

            if (!result.IsValid) { return NotFound(); }

            return Ok(new { id = result.Id, obj = result.Source });
        }

        [HttpGet("{page}/{pageSize}")]
        public async Task<IActionResult> SearchAllWithPagination(int page, int pageSize)
        {
            var documents = await _elasticService.SearchAllWithPagination(page, pageSize);


            return Ok(documents);
        }

        [HttpPost]
        public async Task<IActionResult> IndexUser([FromBody] UserModel user)
        {
            var response = await _elasticService.IndexDocument(user);

            if (!response.IsValid) { return NotFound(); };

            return Ok(new { Id = response.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel model, string id)
        {
            var response = await _elasticService.Update("user", model, id);

            if (!response.IsValid) { return NotFound(); }

            return Ok(new {Message = "Usuário atualizado com sucesso", Id = id });
        }

    }
}
