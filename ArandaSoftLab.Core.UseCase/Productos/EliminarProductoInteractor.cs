using ArandaSoftLab.Core.Domain.Base;
using ArandaSoftLab.Core.Domain.Enumerations;
using ArandaSoftLab.Core.Domain.Productos;
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
    public class EliminarProductoInteractor : IRequestHandler<EliminarProductoRequest, EliminarProductoResponse>, IInteractor
    {
        public string Module => ModulesEnumeration.ArandaSoftLab_Products.Value;
        public string Name => GetType().Name;

        private readonly EliminarProductoRequestValidator _validator;
        public readonly IMapper _mappeer;


        private EliminarProductoRequest _request;
        public EliminarProductoInteractor(IValidator<EliminarProductoRequest> validator, IMapper mappeer)
        {
            _mappeer = mappeer;
            _validator = validator as EliminarProductoRequestValidator;
        }

        public Task<EliminarProductoResponse> Handle(EliminarProductoRequest request, CancellationToken cancellationToken)
        {
            _request = request;
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid) return Task.FromResult(new EliminarProductoResponse(validationResult));

            _validator.Producto.Inactivar();

            _validator.UnitOfWork.Commit(this);

            return Task.FromResult(new EliminarProductoResponse(validationResult));
        }
    }

    public class EliminarProductoRequest : IRequest<EliminarProductoResponse>
    {
        public long Id { get; set; }
    }

    public class EliminarProductoResponse
    {
        private ValidationResult ValidationResult { get; }
        public string Mensaje { get; set; }
        public bool IsValid => ValidationResult.IsValid;
        public EliminarProductoResponse(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
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

    public class EliminarProductoRequestValidator : AbstractValidator<EliminarProductoRequest>
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public Producto Producto { get; set; }

        public EliminarProductoRequestValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;


            When(t => t.Id > 0, () =>
            {
                RuleFor(r => r).Must(DebeExistirProducto).WithMessage(t => $"No existe el producto [{t.Id}] a eliminar");
            });
        }

        private bool DebeExistirProducto(EliminarProductoRequest request)
        {
            Producto = UnitOfWork.ProductoRepository.Find(request.Id);
            return Producto != null;
        }
    }
}
