namespace UserApp.Api.Models
{
    public class FormattedResponseModel
    {
        public FormattedResponseModel(
            bool succeed,
            object? value,
            ApplicationException? exception = null

        )
        {
            Succeed = succeed;
            if ( Succeed )
            {
                Message = "Success";
                Errors = null;
                Data = value;
            }
            else
            {
                Message = "Failure";
                Errors = exception;
                Data = null;
            }
        }
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public object? Errors { get; set; }
        public object? Data { get; set; }
    }
}
