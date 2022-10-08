
using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Infrastructure.Data.Base
{
   public class ArandaSoftLabMapper : Core.Domain.Base.IMapper
    {
        private MapperConfigurationExpression _ConfigExpression;

        private MapperConfigurationExpression ConfigExpression
        {
            get
            {
                if (_ConfigExpression == null)
                {
                    _ConfigExpression = new MapperConfigurationExpression();
                }
                return _ConfigExpression;
            }
        }

        private MapperConfiguration _Config { get; set; }


        public void CreateMapperConfiguration()
        {
            _ConfigExpression = new MapperConfigurationExpression();
        }

        public void CreateMap<TSource, TDestination>()
        {
            ConfigExpression.CreateMap<TSource, TDestination>();
        }

        public List<TDestination> Mappear<TSource, TDestination>(List<TSource> obj, List<TDestination> obj2)
        {
            _Config = new MapperConfiguration(_ConfigExpression);
            var mapper = _Config.CreateMapper();
            obj2 = mapper.Map<List<TSource>, List<TDestination>>(obj, obj2);
            return obj2;
        }
        public TDestination Mappear<TSource, TDestination>(TSource obj, TDestination obj2)
        {
            _Config = new MapperConfiguration(_ConfigExpression);
            var mapper = _Config.CreateMapper();
            obj2 = mapper.Map<TSource, TDestination>(obj, obj2);
            return obj2;
        }

        public TDestination Map<TDestination>(object obj)
        {
            return Mapper.Map<TDestination>(obj);
        }

        public List<TDestination> Map<TSource, TDestination>(List<TSource> obj, List<TDestination> obj2)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            }
            );
            var mapper = config.CreateMapper();
            obj2 = mapper.Map<List<TSource>, List<TDestination>>(obj, obj2);
            return obj2;
        }

        public TDestination Map<TSource, TDestination>(TSource obj, TDestination obj2)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = config.CreateMapper();
            TDestination dest = mapper.Map<TSource, TDestination>(obj, obj2);
            return dest;
        }


    }
}
