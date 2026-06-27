using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data;

public class PersistenceEf : IPersistence
{
    private readonly Dsw2026Ej15DbContext _context;
    public PersistenceEf(Dsw2026Ej15DbContext context)
    {
        _context = context;

    }


    public async Task<Doctor?> GetActiveDoctorById(Guid id)
    {
       
        return await _context.Doctors
            .Include(d => d.Speciality)
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
    }

    public async Task<IEnumerable<Doctor>> GetActiveDoctors()
    {
       
        return await _context.Doctors
            .Include(d => d.Speciality)
            .Where(d => d.IsActive)
            .ToListAsync();
    }

    public async Task<Doctor?> GetDoctorById(Guid id)
    {
     
        return await _context.Doctors.FindAsync(id);
    }

    public async Task<Speciality?> GetSpecialityById(Guid id)
    {
       
        return await _context.Specials.FindAsync(id);
    }

    public async Task SaveDoctor(Doctor doctor)
    {
        _context.Add(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDoctor(Doctor doctor)
    {
        _context.Update(doctor);
        await _context.SaveChangesAsync();
    }
}

