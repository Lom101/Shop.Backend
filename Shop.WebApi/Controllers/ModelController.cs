using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Model.Request;
using Shop.WebAPI.Dtos.Model.Response;
using Shop.WebAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : ControllerBase
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public ModelController(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModelById(int id)
        {
            var model = await _modelRepository.GetModelByIdAsync(id);
            if (model == null)
            {
                return NotFound("Модель не найдена.");
            }

            var modelResponse = _mapper.Map<GetModelResponse>(model);
            return Ok(modelResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModels()
        {
            var models = await _modelRepository.GetAllModelsAsync();
            var modelResponses = _mapper.Map<IEnumerable<GetModelResponse>>(models);
            return Ok(modelResponses);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateModel(CreateModelRequest request)
        {
            var model = _mapper.Map<Model>(request);
            var newModelId = await _modelRepository.AddModelAsync(model);

            if (newModelId == 0)
            {
                return BadRequest("Не удалось создать модель.");
            }

            return CreatedAtAction(nameof(GetModelById), new { id = newModelId }, request);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateModel(int id, UpdateModelRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("Несоответствие ID модели.");
            }

            var model = _mapper.Map<Model>(request);
            var result = await _modelRepository.UpdateModelAsync(model);

            if (!result)
            {
                return NotFound("Модель не найдена.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var result = await _modelRepository.DeleteModelAsync(id);

            if (!result)
            {
                return NotFound("Модель не найдена.");
            }

            return NoContent();
        }
    }
}
