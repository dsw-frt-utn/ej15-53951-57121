using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Interfaces;

    public interface IPersistence
    {
    Task<Doctor?> GetActiveDoctorById(Guid id);
    Task<IEnumerable<Doctor>> GetActiveDoctors();
    Task<Doctor?> GetDoctorById(Guid id);
    Task<Speciality?> GetSpecialityById(Guid id);
    Task SaveDoctor(Doctor doctor);
    Task UpdateDoctor(Doctor doctor);

}

