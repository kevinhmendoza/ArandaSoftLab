//using ArandaSoftLab.Core.Domain.Enumerations;
//using ArandaSoftLab.Core.Domain.Productos;
//using AutoMapper;
//using Core.UseCase.Base;
//using Core.UseCase.Util;
//using FluentValidation;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ArandaSoftLab.Core.UseCase.Productos
//{
//    public class AgregarProductoInteractor : IRequestHandler<AgregarProductoRequest, AgregarProductoResponse>, IInteractor
//    {
//        public string Module => ModulesEnumeration.ArandaSoftLab_Products.Value;
//        public string Name => GetType().Name;

//        private readonly AgregarProductoRequestValidator _validator;
//        public readonly IMapper _mappeer;


//        private AgregarProductoRequest _request;
//        public AgregarProductoInteractor(AbstractValidator<AgregarProductoRequest> validator, IMapper mappeer)
//        {
//            _mappeer = mappeer;
//            _validator = validator as AgregarProductoRequestValidator;
//        }

//        public Task<AgregarProductoResponse> Handle(AgregarProductoRequest request, CancellationToken cancellationToken)
//        {
//            _request = request;
//            var validationResult = _validator.Validate(request);
//            if (!validationResult.IsValid) return Task.FromResult(new AgregarProductoResponse(validationResult));

//            var pago = _validator.Mesa.NuevoPago(new KMNuevoPagoMesaRequest()
//            {
//                Cita = _validator.Cita,
//                ConsecutivoService = _consecutivoService,
//                CorreoCliente = request.CorreoCliente,
//                Descuento = _validator.Descuento,
//                IdentificacionCliente = request.IdentificacionCliente,
//                MedioPago = request.MedioPago,
//                NombreCliente = request.NombreCliente,
//                Servicio = _validator.Servicio,
//                TelefonoCliente = request.TelefonoCliente,
//                Tercero = _validator.Tercero,
//                UnitOfWork = _validator.UnitOfWork,
//                VigenciaActiva = _validator.VigenciaActiva
//            });

//            _validator.UnitOfWork.CommitAndCloseTransaction(this);//Porque hacemos uso del consecutivo

//            var pagoDto = new KMSucursalMesaPagoDto();
//            _mappeer.Map(pago, pagoDto);

//            return Task.FromResult(new AgregarProductoResponse(validationResult, pagoDto));
//        }
//    }

//    public class AgregarProductoRequest : IRequest<AgregarProductoResponse>
//    {
//        public string Nombre { get; set; }
//        public string Descripcion { get; set; }
//        public int? CategoriaId { get; set; }
//        public string Imagen { get; set; }
//    }

//    public class AgregarProductoResponse
//    {
//        public KMSucursalMesaPagoDto Pago { get; set; }
//        public KMRegistrarPagoResponse(ValidationResult validationResult, KMSucursalMesaPagoDto pago = null) : base(validationResult)
//        {
//            Pago = pago;
//        }
//    }

//    public class AgregarProductoRequestValidator : AbstractValidator<AgregarProductoRequest>
//    {
//        public IUnitOfWork UnitOfWork { get; set; }
//        public Producto Tercero { get; set; }

//        public AgregarProductoRequestValidator(IUnitOfWork unitOfWork)
//        {
//            UnitOfWork = unitOfWork;
//            RuleFor(r => r).Must(DebeExistirCategoria).WithMessage(t => $"No existe vigencia activa del año actual");
//            RuleFor(r => r).Must(DebeExistirMesa).WithMessage(t => $"No existe la mesa [{t.KMSucursalMesaId}]");
//            RuleFor(r => r).Must(DebeExistirServicio).WithMessage(t => $"No existe el servicio [{t.KMSucursalServicioId}]");
//            RuleFor(t => t.MedioPago).NotNull().NotEmpty().WithMessage("Debe enviar el medio de pago!");


//            When(t => t.CategoriaId.HasValue, () =>
//            {
//                RuleFor(r => r).Must(DebeExistirCategoria).WithMessage(t => $"No existe la categoria del producto a registrar [{t.CategoriaId}]");
//            });



//        }

//        private bool DebeExistirCategoria(AgregarProductoRequest request)
//        {
//            VigenciaActiva = UnitOfWork.VigenciaRepository.GetVigenciaActiva();
//            return VigenciaActiva != null && VigenciaActiva.Year == UnitOfWork.Sistema.Now.Year;
//        }



//    }
//}
