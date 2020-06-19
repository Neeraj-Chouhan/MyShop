using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepo;
        public BasketController(IBasketRepository basketRepo)
        {
           _basketRepo = basketRepo;

        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id){
            var basket = await _basketRepo.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
            
        }
        [HttpPost]
         public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket){
           
           return Ok(await _basketRepo.UpdateBasketAsync(basket));
            
        }
        [HttpDelete]
         public async Task DeleteBasket(string id){
           
          await _basketRepo.DeleteBasketAsync(id);
            
        }
    }
}