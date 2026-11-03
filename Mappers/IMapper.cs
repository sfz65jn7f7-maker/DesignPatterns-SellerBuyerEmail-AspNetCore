namespace InscriptionApp.Api.Mappers;

public interface IMapper<TEntity, TRequest, TResponse>
{
    TEntity ToEntity(TRequest request);
    TResponse ToResponse(TEntity entity);
}