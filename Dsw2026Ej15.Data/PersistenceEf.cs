using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data;

public class PersistenceEf : IPersistence
{
    private readonly DoctorsDbContext _context;

    public PersistenceEf(DoctorsDbContext context)
    {
        _context = context;
    }

    public Speciality? GetSpecialityById(Guid id)
    {
        return _context.Specialities.SingleOrDefault(s => s.Id == id);
    }

    public IEnumerable<Doctor> GetActiveDoctors()
    {
        return _context.Doctors
            .AsNoTracking()
            .Include(d => d.Speciality)
            .Where(d => d.IsActive)
            .ToList();
    }

    public Doctor? GetActiveDoctorById(Guid id)
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .SingleOrDefault(d => d.Id == id && d.IsActive);
    }

    public void SaveDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        _context.SaveChanges();
    }

    public void DeactivateDoctor(Doctor doctor)
    {
        doctor.Deactivate();
        _context.SaveChanges();
    }
}
