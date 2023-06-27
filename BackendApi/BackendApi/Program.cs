using AutoMapper;
using BackendApi.Models;
using BackEndApi.DTOs;
using BackEndApi.Services.Contrato;
using BackEndApi.Services.Implementacion;
using BackEndApi.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add ConnectionString to Entity
builder.Services.AddDbContext<DbempleadoContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQL"));
});

//Services implementation
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

//AutoMapperProfile class configuration reference
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//con esto evitamos conflictos cuando la URL de la api y Angular sean distintas
builder.Services.AddCors(options => 
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();

    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region PETICIONES API REST
//Get Departamento
app.MapGet("/Departamento/lista", async (
    IDepartamentoService _departamentoService,
    IMapper _mapper
    ) => {

    var listaDepartameto = await _departamentoService.GetList();
    var listaDepartamentoDTO = _mapper.Map<List<DepartamentoDTO>>(listaDepartameto);

    if (listaDepartamentoDTO.Count > 0)
        return Results.Ok(listaDepartamentoDTO);
    else
        return Results.NotFound("No se encontraron departamentos");

});

//Get Empleado
app.MapGet("/Empleado/lista", async (
    IEmpleadoService _empleadoService,
    IMapper _mapper
    ) => {

    var listaEmpleado = await _empleadoService.GetList();
    var listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

    if (listaEmpleadoDTO.Count > 0)
        return Results.Ok(listaEmpleadoDTO);
    else
        return Results.NotFound("No se encontraron empleados");

});
    

app.MapPost("/Empleado/guardar", async (
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoService,
    IMapper _mapper
    ) => {
    //se convierte a tipo Empleado para poder guardar en BD
    var empleado = _mapper.Map<Empleado>(modelo);
    var empleadoCreado = await _empleadoService.Add(empleado);

    if (empleadoCreado.IdEmpleado != 0)
        //Siempre que retornemos un objeto, en este caso tiene que ser de tipo DTO (por Angular)
        return Results.Created("Empleado creado correctamente", _mapper.Map<EmpleadoDTO>(empleadoCreado));
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    
});


app.MapPut("/Empleado/actualizar/{idEmpleado}", async (
    int idEmpleado,
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoService,
    IMapper _mapper
    ) => {

        var encontrado = await _empleadoService.Get(idEmpleado);

        if (encontrado is null) return Results.NotFound("No existe el empleado");

        var empleado = _mapper.Map<Empleado>(modelo);

        //asignamos lo siguiente para evitar modificar el la propiedad "FechaCreacion"
        encontrado.NombreCompleto = empleado.NombreCompleto;
        encontrado.IdDepartamento = empleado.IdDepartamento;
        encontrado.Sueldo = empleado.Sueldo;
        encontrado.FechaContrato = empleado.FechaContrato;

        var empleadoActualizado = await _empleadoService.Update(encontrado);

        if (empleadoActualizado)
            //Siempre que retornemos un objeto, en este caso tiene que ser de tipo DTO (por Angular)
            return Results.Ok(_mapper.Map<EmpleadoDTO>(encontrado));
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);

    });


app.MapDelete("/Empleado/eliminar/{idEmpleado}", async (
    int idEmpleado,
    IEmpleadoService _empleadoService
    ) => {

        var encontrado = await _empleadoService.Get(idEmpleado);

        if (encontrado is null) return Results.NotFound("No existe el empleado");

        var respuesta = await _empleadoService.Delete(encontrado);

        if (respuesta)
            return Results.Ok("Empleado eliminado correctamente");
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);

    });





#endregion

//indicamos el Cors a utilizar
app.UseCors("NuevaPolitica");

app.Run();
