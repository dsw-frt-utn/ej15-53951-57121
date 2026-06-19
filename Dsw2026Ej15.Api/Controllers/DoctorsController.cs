using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers;

public class DoctorsController : AppControllers
{
    private readonly IPersistence _persistence;
    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }



    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if(string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            return BadRequest("Nombre y Matricula requeridos !! ");
        }
        
        var speciality = _persistence.GetSpecialityById(request.SpecialityId);
        if(speciality is null)
        {
            return BadRequest($"No existe la especialidad con ID {request.SpecialityId}");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        _persistence.SaveDoctor(doctor);

        return Created();
    }


       
}

