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
    public class AgregarProductoInteractor : IRequestHandler<AgregarProductoRequest, AgregarProductoResponse>, IInteractor
    {
        public string Module => ModulesEnumeration.ArandaSoftLab_Products.Value;
        public string Name => GetType().Name;

        private readonly AgregarProductoRequestValidator _validator;
        public readonly IMapper _mappeer;


        private AgregarProductoRequest _request;
        public AgregarProductoInteractor(IValidator<AgregarProductoRequest> validator, IMapper mappeer)
        {
            _mappeer = mappeer;
            _validator = validator as AgregarProductoRequestValidator;
        }

        public Task<AgregarProductoResponse> Handle(AgregarProductoRequest request, CancellationToken cancellationToken)
        {
            _request = request;
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid) return Task.FromResult(new AgregarProductoResponse(validationResult));

            var prodcuto = _validator.UnitOfWork.ProductoRepository.Add(new Producto
            {
                Nombre = _request.Nombre,
                Descripcion = _request.Descripcion,
                Imagen = _request.Imagen,
                CategoriaId = _request.CategoriaId.Value,
            });

            _validator.UnitOfWork.Commit(this);

            var prodcutoDto = new ProductoDto();
            _mappeer.Map(prodcuto, prodcutoDto);

            return Task.FromResult(new AgregarProductoResponse(validationResult, prodcutoDto));
        }
    }

    public class AgregarProductoRequest : IRequest<AgregarProductoResponse>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? CategoriaId { get; set; }
        public string Imagen { get; set; }
    }

    public class AgregarProductoResponse
    {
        public ProductoDto ProductoDto { get; set; }
        private ValidationResult ValidationResult { get; }
        public string Mensaje { get; set; }
        public bool IsValid => ValidationResult.IsValid;
        public AgregarProductoResponse(ValidationResult validationResult, ProductoDto productoDto = null)
        {
            ValidationResult = validationResult;
            ProductoDto = productoDto;
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

    public class AgregarProductoRequestValidator : AbstractValidator<AgregarProductoRequest>
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public AgregarProductoRequestValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            RuleFor(t => t.Nombre).NotNull().NotEmpty().WithMessage("Debe enviar el nombre del producto a registrar!");
            RuleFor(t => t.Descripcion).NotNull().NotEmpty().WithMessage("Debe enviar el nombre del producto a registrar!");


            When(t => t.CategoriaId.HasValue, () =>
            {
                RuleFor(r => r).Must(DebeExistirCategoria).WithMessage(t => $"No existe la categoria del producto a registrar [{t.CategoriaId}]");
            });
        }

        private bool DebeExistirCategoria(AgregarProductoRequest request)
        {
            var existeCategoria = UnitOfWork.CategoriaRepository.Any(t => t.Id == request.CategoriaId);
            return existeCategoria;
        }



    }
}
