namespace WebApi
{
    public class RespuestaAPI
    {
        public bool isSuccess { get; set; }
        public int id { get; set; }
        public string message { get; set; }
        public int CodigoResultado { get; set; }

        public object objectResp { get; set; }

        public RespuestaAPI()
        {
            isSuccess = false;
            id = 0;
            message = String.Empty;
            CodigoResultado = 0;
            objectResp = null;
        }
    }
}
