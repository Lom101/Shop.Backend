using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Model.Request;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelController : ControllerBase
{
    private readonly IModelService _modelService;
    
    public ModelController(IModelService modelService)
    {
        _modelService = modelService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetModelById(int id)
    {
        var model = await _modelService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        return Ok(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllModels()
    {
        var models = await _modelService.GetAllModelsAsync();
        return Ok(models);
    }

    [HttpPost]
    public async Task<IActionResult> CreateModel(CreateModelRequest request)
    {
        var newModelId = await _modelService.AddModelAsync(request);
        
        if (newModelId == 0)
        {
            return BadRequest();  // If something goes wrong in creation
        }

        return CreatedAtAction(nameof(GetModelById), new { id = newModelId }, request);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModel(int id, UpdateModelRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("Model ID mismatch");
        }

        var result = await _modelService.UpdateModelAsync(request);
    
        if (!result)
        {
            return NotFound(); // Вернуть 404, если модель не найдена
        }

        return NoContent(); // Вернуть 204, если обновление прошло успешно
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModel(int id)
    {
        var result = await _modelService.DeleteModelAsync(id);
    
        if (!result)
        {
            return NotFound();  // Вернуть 404, если модель не найдена
        }

        return NoContent(); // Вернуть 204, если удаление прошло успешно
    }

}
