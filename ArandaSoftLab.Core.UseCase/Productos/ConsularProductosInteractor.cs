using ArandaSoftLab.Core.Domain.Base;
using ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos;
using ArandaSoftLab.Core.Domain.Enumerations;
using ArandaSoftLab.Core.Domain.Productos;
using ArandaSoftLab.Core.UseCase.Dtos;
using ArandaSoftLab.Core.UseCase.Util;
using Core.UseCase.Base;
using Core.UseCase.Util;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArandaSoftLab.Core.UseCase.Productos
{
    public class ConsularProductosInteractor : IRequestHandler<ConsularProductosRequest, ConsularProductosResponse>, IInteractor
    {
        public string Module => ModulesEnumeration.ArandaSoftLab_Products.Value;
        public string Name => GetType().Name;

        private readonly ConsularProductosRequestValidator _validator;
        public readonly IMapper _mappeer;


        public ConsularProductosInteractor(IValidator<ConsularProductosRequest> validator, IMapper mappeer)
        {
            _mappeer = mappeer;
            _validator = validator as ConsularProductosRequestValidator;
        }

        public Task<ConsularProductosResponse> Handle(ConsularProductosRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid) return Task.FromResult(new ConsularProductosResponse(validationResult));

            var filterDto = new ConsularProductosParameterRequest();
            _mappeer.Map(request, filterDto);
            var productos = _validator.UnitOfWork.ProductoRepository.ConsultaParametrizada(filterDto);

            _validator.UnitOfWork.Commit(this);

            var productosDto = new List<ProductoDto>();
            _mappeer.Map(productos, productosDto);

            return Task.FromResult(new ConsularProductosResponse(validationResult, productosDto));
        }
    }

    public class ConsularProductosRequest : IRequest<ConsularProductosResponse>
    {
        public int? CategoriaId { get; set; }
        public string Nombre { get; set; }
        public bool OrdenNombreDesc { get; set; }
        public string Descripcion { get; set; }
        public bool OrdenDescripcionDesc { get; set; }

        public int? Page { get; set; }
        public int? ItemsByPage { get; set; }
    }

    public class ConsularProductosResponse
    {
        public List<ProductoDto> Productos { get; set; }
        private ValidationResult ValidationResult { get; }
        public string Mensaje { get; set; }
        public bool IsValid => ValidationResult.IsValid;
        public ConsularProductosResponse(ValidationResult validationResult, List<ProductoDto> productosDto = null)
        {
            ValidationResult = validationResult;
            Productos = productosDto;
            if (!ValidationResult.IsValid)
            {
                Mensaje = ValidationResult.ToText();
            }
            else
            {
                if (Productos == null) { Productos = new List<ProductoDto>(); }
                Mensaje = $"Operación realizada satisfactoriamente.";
            }
        }


    }

    public class ConsularProductosRequestValidator : AbstractValidator<ConsularProductosRequest>
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public ConsularProductosRequestValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            When(t => t.CategoriaId.HasValue, () =>
            {
                RuleFor(r => r).Must(DebeExistirCategoria).WithMessage(t => $"No existe la categoria del producto a registrar [{t.CategoriaId}]");
            });
        }

        private bool DebeExistirCategoria(ConsularProductosRequest request)
        {
            var existeCategoria = UnitOfWork.CategoriaRepository.Any(t => t.Id == request.CategoriaId);
            return existeCategoria;
        }



    }
}
