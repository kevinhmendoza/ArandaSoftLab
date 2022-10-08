using ArandaSoftLab.Core.Domain.Base;
using ArandaSoftLab.Core.Domain.Enumerations;
using ArandaSoftLab.Core.Domain.Productos;
using ArandaSoftLab.Core.UseCase.Dtos;
using ArandaSoftLab.Core.UseCase.Util;
using Core.UseCase.Base;
using Core.UseCase.Util;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ArandaSoftLab.Core.UseCase.Productos
{
    public class ModificarProductoInteractor : IRequestHandler<ModificarProductoRequest, ModificarProductoResponse>, IInteractor
    {
        public string Module => ModulesEnumeration.ArandaSoftLab_Products.Value;
        public string Name => GetType().Name;

        private readonly ModificarProductoRequestValidator _validator;
        public readonly IMapper _mappeer;


        private ModificarProductoRequest _request;
        public ModificarProductoInteractor(IValidator<ModificarProductoRequest> validator, IMapper mappeer)
        {
            _mappeer = mappeer;
            _validator = validator as ModificarProductoRequestValidator;
        }

        public Task<ModificarProductoResponse> Handle(ModificarProductoRequest request, CancellationToken cancellationToken)
        {
            _request = request;
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid) return Task.FromResult(new ModificarProductoResponse(validationResult));

            _mappeer.Map(request, _validator.Producto);

            _validator.UnitOfWork.Commit(this);

            var prodcutoDto = new ProductoDto();
            _mappeer.Map(_validator.Producto, prodcutoDto);

            return Task.FromResult(new ModificarProductoResponse(validationResult, prodcutoDto));
        }
    }

    public class ModificarProductoRequest : IRequest<ModificarProductoResponse>
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? CategoriaId { get; set; }
        public string Imagen { get; set; }
    }

    public class ModificarProductoResponse
    {
        public ProductoDto Producto { get; set; }
        private ValidationResult ValidationResult { get; }
        public string Mensaje { get; set; }
        public bool IsValid => ValidationResult.IsValid;
        public ModificarProductoResponse(ValidationResult validationResult, ProductoDto productoDto = null)
        {
            ValidationResult = validationResult;
            Producto = productoDto;
            if (!ValidationResult.IsValid)
            {
                Mensaje = ValidationResult.ToText();
            }
            else
            {
                Mensaje = $"Operación realizada satisfactoriamente.";
            }
        }


    }

    public class ModificarProductoRequestValidator : AbstractValidator<ModificarProductoRequest>
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public Producto Producto { get; set; }

        public ModificarProductoRequestValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            RuleFor(t => t.Nombre).NotNull().NotEmpty().WithMessage("Debe enviar el nombre del producto a registrar!");
            RuleFor(t => t.Descripcion).NotNull().NotEmpty().WithMessage("Debe enviar el nombre del producto a registrar!");


            When(t => t.CategoriaId.HasValue, () =>
            {
                When(t => t.Id > 0, () =>
                {
                    RuleFor(r => r).Must(DebeExistirProducto).WithMessage(t => $"No existe el producto [{t.Id}] a modificar");
                });
                RuleFor(r => r).Must(DebeExistirCategoria).WithMessage(t => $"No existe la categoria del producto a registrar [{t.CategoriaId}]");
            });
        }

        private bool DebeExistirProducto(ModificarProductoRequest request)
        {
            Producto = UnitOfWork.ProductoRepository.Find(request.Id);
            return Producto != null;
        }

        private bool DebeExistirCategoria(ModificarProductoRequest request)
        {
            var existeCategoria = UnitOfWork.CategoriaRepository.Any(t => t.Id == request.CategoriaId);
            return existeCategoria;
        }



    }
}
