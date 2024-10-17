using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Busca a tarefa no banco de dados pelo ID usando o Entity Framework
            var tarefa = _context.Tarefas.Find(id);

            // Verifica se a tarefa foi encontrada
            if (tarefa == null)
            {
                return NotFound(new { mensagem = "Tarefa não encontrada" });
            }

            // Retorna OK com a tarefa encontrada
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Busca todas as tarefas no banco de dados utilizando o Entity Framework
            var tarefas = _context.Tarefas.ToList();

            // Retorna OK com a lista de tarefas encontradas
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Busca todas as tarefas cujo título contenha o parâmetro recebido
            var tarefas = _context.Tarefas
                                  .Where(t => t.Titulo.Contains(titulo))
                                  .ToList();

            // Verifica se foram encontradas tarefas com o título
            if (tarefas == null || tarefas.Count == 0)
            {
                return NotFound(new { mensagem = "Nenhuma tarefa encontrada com o título informado" });
            }

            // Retorna OK com a lista de tarefas encontradas
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Busca todas as tarefas que tenham o status igual ao parâmetro recebido
            var tarefas = _context.Tarefas
                                  .Where(x => x.Status == status)
                                  .ToList();

            // Verifica se foram encontradas tarefas com o status
            if (tarefas == null || tarefas.Count == 0)
            {
                return NotFound(new { mensagem = "Nenhuma tarefa encontrada com o status informado" });
            }

            // Retorna OK com a lista de tarefas encontradas
            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            // Valida se a data da tarefa foi informada
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adiciona a tarefa ao contexto do EF
            _context.Tarefas.Add(tarefa);

            // Salva as mudanças no banco de dados
            _context.SaveChanges();

            // Retorna o CreatedAtAction com a tarefa criada e seu ID
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }


        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            // Busca a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Verifica se a tarefa foi encontrada
            if (tarefaBanco == null)
                return NotFound(new { Erro = "Tarefa não encontrada" });

            // Valida se a data da tarefa é válida
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualiza as informações da tarefa encontrada com os novos dados
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Salva as mudanças no banco de dados
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            // Retorna OK com a tarefa atualizada
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // Busca a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Verifica se a tarefa foi encontrada
            if (tarefaBanco == null)
                return NotFound(new { Erro = "Tarefa não encontrada" });

            // Remove a tarefa encontrada através do EF
            _context.Tarefas.Remove(tarefaBanco);

            // Salva as mudanças no banco de dados
            _context.SaveChanges();

            // Retorna NoContent para indicar que a remoção foi bem-sucedida
            return NoContent();
        }
    }
}
