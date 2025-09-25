using TestManager.Domain.DTO;
using System.Linq.Expressions;

namespace TestManager.DataAccess.Sort
{
    public static class AppointmentSortMappings
    {
        public static readonly Dictionary<AppointmentSortField, Expression<Func<AppointmentDto, object>>> RadiologyMap =
            new()
            {
                [AppointmentSortField.PatientLastName] = x => x.PatientLastName,
                [AppointmentSortField.DoctorFullName] = x => x.DoctorFullName,
                [AppointmentSortField.Date] = x => x.Date,
                [AppointmentSortField.AppointmentType] = x => x.AppointmentType,
                [AppointmentSortField.StatusId] = x => x.StatusId,
            };

        public static readonly Dictionary<AppointmentSortField, Expression<Func<UploaderAppointmentDto, object>>> UploaderMap =
            new()
            {
                [AppointmentSortField.PatientLastName] = x => x.PatientLastName,
                [AppointmentSortField.Date] = x => x.Date,
                [AppointmentSortField.AppointmentType] = x => x.AppointmentType,
        };
    }
}
