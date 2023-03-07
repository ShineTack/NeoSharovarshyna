using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoSharovarshyna.Services.ProductAPI.Models.Dtos;
using NeoSharovarshyna.Services.ProductAPI.Repositories;

namespace NeoSharovarshyna.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private ResponseDto _responseDto;
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _responseDto.Data = await _repository.GetProducts();
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.Message };
                return StatusCode(500, _responseDto);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest(new ResponseDto() { IsSuccess = false, DisplayMessage = "Id cannot be equals or less then 0" });
            try
            {
                _responseDto.Data = await _repository.GetProductById(id);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.Message };
                return StatusCode(500, _responseDto);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseDto() { IsSuccess = false, DisplayMessage = "Invalid Model" });
            try
            {
                _responseDto.Data = await _repository.CreateUpdateProduct(productDto);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.Message };
                return StatusCode(500, _responseDto);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody]ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseDto() { IsSuccess = false, DisplayMessage = "Invalid Model" });
            try
            {
                _responseDto.Data = await _repository.CreateUpdateProduct(productDto);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.Message };
                return StatusCode(500, _responseDto);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(new ResponseDto() { IsSuccess = false, DisplayMessage = "Id cannot be equals or less then 0" });
            try
            {
                await _repository.DeleteProduct(id);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.Message };
                return StatusCode(500, _responseDto);
            }
        }
    }
}
