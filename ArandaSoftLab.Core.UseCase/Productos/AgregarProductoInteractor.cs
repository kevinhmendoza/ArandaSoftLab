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
        /// <summary>
        /// Agregar Imagen en Base64
        /// </summary>
        /// <example>"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII="</example>
        public string Imagen { get; set; }
    }

    public class AgregarProductoResponse
    {
        public ProductoDto Producto { get; set; }
        private ValidationResult ValidationResult { get; }
        public string Mensaje { get; set; }
        public bool IsValid => ValidationResult.IsValid;
        public AgregarProductoResponse(ValidationResult validationResult, ProductoDto productoDto = null)
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
