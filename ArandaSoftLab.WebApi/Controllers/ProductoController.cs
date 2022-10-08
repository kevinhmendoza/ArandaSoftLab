using ArandaSoftLab.Core.UseCase.Productos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArandaSoftLab.WebApi.Controllers
{
    [RoutePrefix("api/Producto")]
    public class ProductoController : ApiController
    {
        private readonly IMediator _mediator = null;
        public ProductoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/Producto
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Producto/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Producto
        public IHttpActionResult PostRegistrarProducto(AgregarProductoRequest request)
        {
            var response = _mediator.Send(request);
            if (response.Result.IsValid)
            {
                return Ok(response.Result);
            }
            else return BadRequest(response.Result.Mensaje);
        }

        // PUT: api/Producto/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Producto/5
        public void Delete(int id)
        {
        }
    }
}
