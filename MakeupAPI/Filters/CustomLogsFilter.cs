using Microsoft.AspNetCore.Mvc.Filters;
using MakeupAPI.Interfaces;
using MakeupAPI.Logs;
using MakeupAPI.Model;


namespace MakeupAPI.Filters
{
    public class CustomLogsFilter : IResultFilter, IActionFilter
    {
        private readonly IProductRepository _repository;
        private readonly List<int> _sucessStatusCodes;
        private readonly Dictionary<int, Product> _contextDict;
        

        public CustomLogsFilter(IProductRepository repository)
        {
            _repository = repository;
            _contextDict = new Dictionary<int, Product>();
            _sucessStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created };
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "Product", StringComparison.InvariantCultureIgnoreCase))
            {
                int id = 0;
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out id))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var product = _repository.GetByKey(id).Result;
                        if (product != null)
                        {
                           _contextDict.Add(id, product);
                        }
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/Product", StringComparison.InvariantCulture))
            {
                if (_sucessStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {  
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());
                        var afterUpdate = _repository.GetByKey(id).Result;
                        if (afterUpdate != null)
                        {
                            Product beforeUpdate;
                            if (_contextDict.TryGetValue(id, out beforeUpdate))
                            {
                                CustomLogs.SaveLog(afterUpdate.Id, "Product", afterUpdate.Name, context.HttpContext.Request.Method, beforeUpdate, afterUpdate);
                                _contextDict.Remove(id);
                            }
                        }
                    }
                    else if (context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());
                        Product beforeUpdate;
                        if (_contextDict.TryGetValue(id, out beforeUpdate))
                        {
                            CustomLogs.SaveLog(beforeUpdate.Id, "Product", beforeUpdate.Name, context.HttpContext.Request.Method);
                            _contextDict.Remove(id);
                        }
                    }
                }
            }
        }

        #region Não Utilizados
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

        }
        #endregion
    }
}
