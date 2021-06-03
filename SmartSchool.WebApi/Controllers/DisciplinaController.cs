using Microsoft.AspNetCore.Mvc;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.interfaces.IServices;
using System;
using System.Linq;

namespace SmartSchool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinaController : ControllerBase
    {
        private readonly IBaseService _baseService;

        public DisciplinaController(IBaseService baseService)
        {
            _baseService = baseService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_baseService.BuscarTodosPorQuery<Disciplina>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            try
            {
                return Ok(_baseService.BuscarTodosPorId<Disciplina>(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Disciplina entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _baseService.Adicionar(entidade);
                    return Created($"api/{RouteData.Values.First().Value}", resultado);
                }

                return BadRequest("Classe inválida");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Disciplina entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_baseService.BuscarTodosPorId<Disciplina>(id) == null)
                        return NotFound();

                    _baseService.Atualizar(id, entidade);

                    return Ok(_baseService.BuscarTodosPorId<Disciplina>(id));
                }

                return BadRequest("Classe inválida");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_baseService.BuscarTodosPorId<Disciplina>(id) == null)
                    return NotFound();

                _baseService.Excluir<Disciplina>(id);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
