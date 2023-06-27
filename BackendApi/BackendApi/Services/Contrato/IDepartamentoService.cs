using BackendApi.Models;

namespace BackEndApi.Services.Contrato
{
    public interface IDepartamentoService
    {
        Task<List<Departamento>> GetList();
    }
}
