// using VeterinaryClinic.Models;
// using VeterinaryClinic.Data;

// namespace VeterinaryClinic.Repositories
// {
//     public class PatientRepository
//     {
//         // Agregar un nuevo paciente
//         public void Add(Patient patient)
//         {
//             DataStore.Patients.Add(patient);
//         }

//         // Obtener todos los pacientes
//         public List<Patient> GetAll()
//         {
//             return DataStore.Patients;
//         }

//         // Obtener un paciente por ID
//         public Patient? GetById(Guid id)
//         {
//             return DataStore.Patients.FirstOrDefault(p => p.Id == id);
//         }

//         // Eliminar un paciente por ID
//         public bool Delete(Guid id)
//         {
//             var patient = GetById(id);
//             if (patient == null) return false;

//             return DataStore.Patients.Remove(patient);
//         }

//         // Actualizar los datos de un paciente
//         public bool Update(Patient updatedPatient)
//         {
//             var existingPatient = GetById(updatedPatient.Id);
//             if (existingPatient == null) return false;

//             existingPatient.Name = updatedPatient.Name;
//             existingPatient.Age = updatedPatient.Age;
//             existingPatient.Pets = updatedPatient.Pets;

//             return true;
//         }
//     }
// }
