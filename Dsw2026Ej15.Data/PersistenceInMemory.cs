using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data;


public class PersistenceInMemory: IPersistence
{
    private List<Speciality> _specialities = [];
    private List<Doctor> _doctors = [];

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }

    public async Task<Speciality?> GetSpecialityById(Guid id)
    {
        return await Task.FromResult(_specialities.SingleOrDefault(e => e.Id == id));
    }

 
    public async Task<IEnumerable<Doctor>> GetActiveDoctors()
    {
        return await Task.FromResult(_doctors.Where(d => d.IsActive));
    }


    public async Task<Doctor?> GetActiveDoctorById(Guid id)
    {
        return await Task.FromResult(_doctors.SingleOrDefault(d => d.Id == id && d.IsActive));
    }

   
    public async Task SaveDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
        await Task.CompletedTask; 
    }


    public async Task UpdateDoctor(Doctor doctor)
    {
       
        await Task.CompletedTask;
    }

  
    public async Task<Doctor?> GetDoctorById(Guid id)
    {
        return await Task.FromResult(_doctors.SingleOrDefault(d => d.Id == id));
    }

    private void LoadSpecialities()
    {
        string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");

        try
        {
            var json = File.ReadAllText(jsonPath);
            var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json, new JsonSerializerOptions(){
                PropertyNameCaseInsensitive = true
            })?? [];
            _specialities = [.. specialities.Select(s => new Speciality(s.Name, s.Description, s.Id))];
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException($"No se pudieron cargar las especialidades desde {jsonPath}.", exception);
        }
    }

}


