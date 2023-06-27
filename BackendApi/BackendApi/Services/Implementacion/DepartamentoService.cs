using BackendApi.Models;
using BackEndApi.Services.Contrato;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Services.Implementacion
{
    public class DepartamentoService : IDepartamentoService
    {
        private DbempleadoContext _connection;
        public DepartamentoService(DbempleadoContext connection)
        {
            _connection = connection;
        }

        public async Task<List<Departamento>> GetList()
        {
            try
            {
                var lista = new List<Departamento>();
                lista = await _connection.Departamentos.ToListAsync();

                return lista;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
