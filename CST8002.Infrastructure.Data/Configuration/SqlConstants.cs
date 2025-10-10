namespace CST8002.Infrastructure.Data.Configuration
{
    public static class SqlConstants
    {
        public const string Schema = "DABS";

        // Users / Auth
        public const string SpActivateUser = $"{Schema}.sp_ActivateUser";
        public const string SpLogin = $"{Schema}.sp_Login";
        public const string SpRegisterPatient = $"{Schema}.sp_RegisterPatient";
        public const string SpGetUserSaltByEmail = $"{Schema}.[sp_GetUserSaltByEmail]";
        public const string SpGetDoctorIdByUserId = $"{Schema}.sp_GetDoctorIdByUserId";
        public const string SpGetPatientIdByUserId = $"{Schema}.sp_GetPatientIdByUserId";

        // Doctors
        public const string SpCreateDoctor = $"{Schema}.sp_CreateDoctor";
        public const string SpDeleteDoctorSoft = $"{Schema}.sp_DeleteDoctorSoft";
        public const string SpUpdateDoctor = $"{Schema}.sp_UpdateDoctor";
        public const string SpListDoctorsBasic = $"{Schema}.sp_ListDoctorsBasic";


        // Patients
        public const string SpDeletePatientSoft = $"{Schema}.sp_DeletePatientSoft";
        public const string SpUpdatePatient = $"{Schema}.sp_UpdatePatient";
        public const string SpListPatientsPendingActivation = $"{Schema}.sp_ListPatientsPendingActivation";
        public const string SpGetPatientById = "[DABS].[sp_GetPatientById]";

        // Appointments
        public const string SpAdminCancelAppointment = $"{Schema}.sp_AdminCancelAppointment";
        public const string SpBookAppointment = $"{Schema}.sp_BookAppointment";
        public const string SpCancelAppointment = $"{Schema}.sp_CancelAppointment";
        public const string SpListAppointmentsByDoctor = $"{Schema}.sp_ListAppointmentsByDoctor";
        public const string SpListAppointmentsByPatient = $"{Schema}.sp_ListAppointmentsByPatient";
        public const string SpReportDoctorAppointments = $"{Schema}.sp_Report_DoctorAppointments";
        public const string SpReportDoctorAppointmentsCsv = $"{Schema}.sp_Report_DoctorAppointments_Csv";
        public const string SpListAppointments = $"{Schema}.sp_ListAppointments";

        // Schedules
        public const string SpAdminGenerateSlots = $"{Schema}.sp_AdminGenerateSlots";
        public const string SpAdminGenerateSlotsRange = $"{Schema}.sp_AdminGenerateSlotsRange";
        public const string SpClearSlotsRange = $"{Schema}.sp_ClearSlotsRange";
        public const string SpDoctorGenerateSlots = $"{Schema}.sp_DoctorGenerateSlots";
        public const string SpListAvailableSlots = $"{Schema}.sp_ListAvailableSlots";

        // Notifications
        public const string SpNotificationsCountUnread = $"{Schema}.sp_Notifications_CountUnread";
        public const string SpNotificationsDelete = $"{Schema}.sp_Notifications_Delete";
        public const string SpNotificationsListForUser = $"{Schema}.sp_Notifications_ListForUser";
        public const string SpNotificationsMarkRead = $"{Schema}.sp_Notifications_MarkRead";
        public const string SpNotifyCreate = $"{Schema}.sp_Notify_Create";
        public const string SpNotificationsMarkAll = $"{Schema}.sp_Notifications_MarkAll";


        // Reports
        public const string SpReportDoctorTotals = $"{Schema}.sp_Report_DoctorTotals";

        // Table-Valued Types (UDTT)
        public const string TypeIntList = $"{Schema}.IntList";

    }
}
