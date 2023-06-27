using BackendApi.Models;
using BackEndApi.Services.Contrato;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Services.Implementacion
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly DbempleadoContext _connection;

        public EmpleadoService(DbempleadoContext connection)
        {
            _connection = connection;
        }

        public async Task<List<Empleado>> GetList()
        {
            try
            {
                List<Empleado> empleados = null;
                empleados = await _connection.Empleados.Include(dpt => dpt.IdDepartamentoNavigation).ToListAsync();
                return empleados;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Empleado> Get(int idEmpleado)
        {
            try
            {
                Empleado? encontrado = new Empleado();

                encontrado = await _connection.Empleados.Include(dpt => dpt.IdDepartamentoNavigation).
                    Where(e => e.IdEmpleado == idEmpleado).FirstOrDefaultAsync();

                return encontrado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<Empleado> Add(Empleado modelo)
        {
            try
            {
                _connection.Empleados.Add(modelo);
                await _connection.SaveChangesAsync();
                return modelo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> Update(Empleado modelo)
        {
            try
            {
                _connection.Empleados.Update(modelo);
                await _connection.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> Delete(Empleado modelo)
        {
            try
            {
                _connection.Empleados.Remove(modelo);
                await _connection.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}