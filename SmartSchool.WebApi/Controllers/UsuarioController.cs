using Microsoft.AspNetCore.Mvc;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.interfaces.IServices;
using System;
using System.Linq;

namespace SmartSchool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IBaseService _baseService;

        public UsuarioController(IBaseService baseService)
        {
            _baseService = baseService;
        }

        /// <summary>
        /// Método responsável por obter todos os usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_baseService.BuscarTodosPorQuery<Usuario>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Método responsável por obter apenas 1 usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            try
            {
                return Ok(_baseService.BuscarTodosPorId<Usuario>(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Método resposável por adicionar a entidade usuário
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Usuario entidade)
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
        /// <summary>
        /// Método responsável por Atualizar a entidade usuário
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entidade"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Usuario entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_baseService.BuscarTodosPorId<Usuario>(id) == null)
                        return NotFound();

                    _baseService.Atualizar(id, entidade);

                    return Ok(_baseService.BuscarTodosPorId<Usuario>(id));
                }

                return BadRequest("Classe inválida");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Método responsável por Excluir a entidade usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_baseService.BuscarTodosPorId<Usuario>(id) == null)
                    return NotFound();

                _baseService.Excluir<Usuario>(id);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
