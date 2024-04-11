using Microsoft.AspNetCore.Mvc;
using TheBookOfAnswerAPI.Models;
using TheBookOfAnswerAPI.Service;

namespace TheBookOfAnswerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController : ControllerBase{
        public AnswerController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<Answer>>?> GetAll(){
            List<Answer>? result = await AnswerService.GetAll();

            if(result is null){
                return null;
            }

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetById(int id){
            Answer? answer =  await AnswerService.GetById(id);

            if(answer == null){
                return NotFound();
            }

            return answer;
        }

        [HttpGet]
        [Route("Random")]
        public async Task<ActionResult<Answer>> RandomAnswer(){
            Random rnd = new Random();
            int randomId = rnd.Next(1, 175);

            return  await GetById(randomId);
        }
    }
}