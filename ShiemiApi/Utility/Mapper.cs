using AutoMapper;

namespace ShiemiApi.Utility;

public class MapperUtility
{
    public Mapper Get<TSource, TDest>()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<TSource, TDest>(),
            new LoggerFactory()
        );

        return new Mapper(config);
    }
}
