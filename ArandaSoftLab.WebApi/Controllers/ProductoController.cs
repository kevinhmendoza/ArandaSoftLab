using ArandaSoftLab.Core.Domain.Base;
using ArandaSoftLab.Core.UseCase.Productos;
using MediatR;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace ArandaSoftLab.WebApi.Controllers
{
    [RoutePrefix("api/v1/productos")]
    public class ProductoController : ApiController
    {
        private readonly IMediator _mediator = null;
        private readonly IMapper _mapper = null;
        public ProductoController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [SwaggerResponse(HttpStatusCode.OK, "Lista de Productos", typeof(ConsularProductosResponse))]
        public IHttpActionResult Get(bool OrdenDescripcionDes, bool OrdenNombreDes, int? CategoryId = null, string Nombre = null, string Descripcion = null, int? Page = null, int? ItemsByPage = null)
        {
            var response = _mediator.Send(new ConsularProductosRequest()
            {
                CategoriaId = CategoryId,
                Descripcion = Descripcion,
                ItemsByPage = ItemsByPage,
                Page = Page,
                Nombre = Nombre,
                OrdenDescripcionDesc = OrdenDescripcionDes,
                OrdenNombreDesc = OrdenNombreDes
            });
            if (response.Result.IsValid)
            {
                return Ok(response.Result);
            }
            else return BadRequest(response.Result.Mensaje);
        }

        // POST: api/Producto
        [SwaggerResponse(HttpStatusCode.OK, "Agregar de Producto", typeof(AgregarProductoResponse))]
        public IHttpActionResult PostRegistrarProducto(AgregarProductoRequest request)
        {
            var response = _mediator.Send(request);
            if (response.Result.IsValid)
            {
                return Ok(response.Result);
            }
            else return BadRequest(response.Result.Mensaje);
        }

        [SwaggerResponse(HttpStatusCode.OK, "Modificar de Producto", typeof(ModificarProductoResponse))]
        public IHttpActionResult PutModificarProducto(long Id, ModificarProductoControllerRequest request)
        {
            var objRequest = new ModificarProductoRequest();
            _mapper.Map(request, objRequest);
            objRequest.Id = Id;
            var response = _mediator.Send(objRequest);
            if (response.Result.IsValid)
            {
                return Ok(response.Result);
            }
            else return BadRequest(response.Result.Mensaje);
        }

        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Producto", typeof(EliminarProductoResponse))]
        public IHttpActionResult DeleteModificarProducto(long Id)
        {
            var response = _mediator.Send(new EliminarProductoRequest()
            {
                Id= Id
            });
            if (response.Result.IsValid)
            {
                return Ok(response.Result);
            }
            else return BadRequest(response.Result.Mensaje);
        }
    }

    public class ModificarProductoControllerRequest
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? CategoriaId { get; set; }
        public string Imagen { get; set; }
    }
}
