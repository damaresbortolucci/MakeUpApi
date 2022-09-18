using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MakeupAPI.Dto;
using MakeupAPI.Interfaces;
using MakeupAPI.Model;
using System.Net.Mime;

namespace MakeupAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("ValidateClaimModule")]
    public class ProductController : ControllerBase, IProductController
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;


        public ProductController(IProductRepository repository, ILogger<ProductController> logger, IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _configuration = configuration;
        }


        [HttpGet]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int page, int maxResults)
        {
            var product = await _repository.Get(page, maxResults);
            return Ok(product);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var product = await _repository.GetByKey(id);

            if (product == null)
                throw new KeyNotFoundException();

            return Ok(product);
        }




        [HttpGet("brand")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] string query)
        {
            var products = await _repository.GetByBrandName(query);

            if (products == null)
                return NotFound(new { message = "Marca não encontrada" });

            return Ok(products);
        }



        [HttpGet("allBrands")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrands()
        {
            var products = await _repository.GetAllBrands();

            //fiz o groupBy no controller porque não consegui retornar um objeto anônimo no método do repository
            var teste = products.GroupBy(x => x.Brand)
                                 .Select(x => new
                                  {
                                    marca = x.Key,
                                    total_Produtos = x.Count()
                                  });
            return Ok(teste);
        }


        
        [HttpGet("type")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] string type, [FromQuery] float minRating = 1)
        {
            var products = await _repository.GetTypeAndRating(type, minRating);

            if (products == null)
                return NotFound(new { message = "Tipo de produto não encontrado" });

            return Ok(products);
        }



        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] ProductDto entity)
        {
            var productToInsert = new Product(id: 0, entity.Brand, entity.Name, entity.Price, entity.Image_Link, entity.Description, entity.Rating,  entity.Category, entity.Product_Type);

            var inserted = await _repository.Insert(productToInsert);
            return Created(string.Empty, inserted);
        }


        [HttpPost("query")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostQuery([FromBody] ProductDto entity)
        {
            var result = _repository.Get(1, 10).Result;
            var filtered = result.Where(item => item.Brand == entity.Brand);
            return Ok(filtered);
        }


        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json, new[] { "application/xml", "text/plain" })]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ProductDto entity)
        {
            var databaseProducts = await _repository.GetByKey(id);

            if (databaseProducts == null)
            {
                var productToInsert = new Product(id: 0, entity.Brand, entity.Name, entity.Price, entity.Image_Link, entity.Description, entity.Rating, entity.Category, entity.Product_Type); 

                var inserted = await _repository.Insert(productToInsert);
                return Created(string.Empty, inserted);
            }
            databaseProducts = UpdateProductModel(databaseProducts, entity);
            var updated = await _repository.Update(databaseProducts);

            return Ok(updated);
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<Product> patchEntity)
        {
            var product = await _repository.GetByKey(id);

            if (product == null)
                throw new KeyNotFoundException();

            patchEntity.ApplyTo(product);
            var updated = await _repository.Update(product);

            return Ok(updated);
        }

 

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var databaseProducts = await _repository.GetByKey(id);

            if (databaseProducts == null)
                throw new KeyNotFoundException();

            var deleted = await _repository.Delete(id);
            return Ok(deleted);
        }




        private Product UpdateProductModel(Product newData, ProductDto entity)
        { 
            newData.Brand = entity.Brand;
            newData.Name = entity.Name;
            newData.Price = entity.Price;
            newData.Image_Link = entity.Image_Link;
            newData.Description = entity.Description;
            newData.Rating = entity.Rating;
            newData.Category = entity.Category;
            newData.Product_Type = entity.Product_Type;
            return newData;
        }
    }
}