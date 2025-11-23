namespace AppointmentSystemAPI.Dtos.RolesDtos
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
