//using GreetingService.Core;
//using GreetingService.API.Authentication;
//using GreetingService.Core.Entities;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace GreetingService.API.Controllers
//{
//    [Route("api/[controller]")]
//    [BasicAuth]
//    [ApiController]
//    public class GreetingController : ControllerBase
//    {
//        private readonly IGreetingRepository _greetingRepository;
       
//        public GreetingController(IGreetingRepository greetingRepository)
//        {
//            _greetingRepository = greetingRepository;
//        }

//        // GET: api/<Greeting>
//        [HttpGet]
//        public IEnumerable<Greeting> Get()
//        {
//            return _greetingRepository.GetAsync();
//        }

//        // GET api/<Greeting>/5
//        [HttpGet("{id}")]
//        public Greeting Get(Guid id)
//        {
//            return _greetingRepository.Get(id);
//        }

//        // POST api/<Greeting>
//        [HttpPost]
//        public void Post([FromBody] Greeting greeting)
//        {
//            _greetingRepository.Create(greeting);

//        }

//        // PUT api/<Greeting>/5
//        [HttpPut("{id}")]
//        public void Put(Guid id, [FromBody] Greeting greeting)
//        {
//                _greetingRepository.Update(greeting);
//        }

//        // DELETE api/<Greeting>/5
//        [HttpDelete("{id}")]
//        public void Delete(Guid id)
//        {
//            _greetingRepository.DeleteRecord(id);
//        }
//    }
//}
