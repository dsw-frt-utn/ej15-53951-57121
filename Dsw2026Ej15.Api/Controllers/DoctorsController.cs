using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
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
    public IActionResult CreateDoctor(DoctorModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException("El nombre es requerido.");
        }

        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("La matricula es requerida.");
        }

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality is null)
        {
            throw new ValidationException($"No existe la especialidad con ID {request.SpecialityId}.");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        _persistence.SaveDoctor(doctor);

        return CreatedAtAction(
            nameof(GetActiveDoctorById),
            new { id = doctor.Id },
            ToResponse(doctor));
    }

    [HttpGet("doctors")]
    public IActionResult GetActiveDoctors()
    {
        var doctors = _persistence
            .GetActiveDoctors()
            .Select(ToResponse);

        return Ok(doctors);
    }

    [HttpGet("doctors/{id:guid}")]
    public IActionResult GetActiveDoctorById(Guid id)
    {
        var doctor = _persistence.GetActiveDoctorById(id);
        if (doctor is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(doctor));
    }

    [HttpDelete("doctors/{id:guid}")]
    public IActionResult DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetActiveDoctorById(id);
        if (doctor is null)
        {
            return NotFound();
        }

        _persistence.DeactivateDoctor(doctor);

        return NoContent();
    }

    private static DoctorModel.Response ToResponse(Doctor doctor)
    {
        return new DoctorModel.Response(
            doctor.Id,
            doctor.Name,
            doctor.LicenseNumber,
            doctor.Speciality?.Name ?? string.Empty);
    }
}
