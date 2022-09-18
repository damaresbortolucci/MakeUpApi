using MakeupAPI.Dto;
using MakeupAPI.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace MakeupAPI.Interfaces
{
    public interface IProductController
    {
        Task<IActionResult> Get(int page, int maxResults);

        Task<IActionResult> Get(int key);

        Task<IActionResult> Get(string brand);

        Task<IActionResult> Post(ProductDto entity);

        Task<IActionResult> Put(int key, ProductDto entity);

        Task<IActionResult> Patch(int key, JsonPatchDocument<Product> entity);

        Task<IActionResult> Delete(int key);
    }
}
