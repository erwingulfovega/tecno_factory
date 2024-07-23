using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDBinASP.NET.Clases
{
    public class RespuestaAPI
    {
        
        private bool _isSuccess;
        private int _id;
        private string _message;
        private int _CodigoResultado;
        private object _objectResp;
        public bool isSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string message { get { return _message; } set { _message = value; } }
        public int CodigoResultado
        {
            get { return _CodigoResultado; }
            set { _CodigoResultado = value; }
        }

        public object objectResp
        {
            get { return _objectResp; }
            set { _objectResp = value; }
        }

        public RespuestaAPI()
        {
            isSuccess = false;
            id = 0;
            message = String.Empty;
            this.CodigoResultado = 0;
        }
        public RespuestaAPI(bool _isSuccess, string _message, int _id = 0)
        {
            this.isSuccess = _isSuccess;
            this.id = _id;
            this.message = _message;
            this.CodigoResultado = 0;

        }
        public RespuestaAPI(bool _isSuccess, int _CodigoResultado, string _message, int _id = 0, object objectResp = null)
        {
            this.isSuccess = _isSuccess;
            this.id = _id;
            this.message = _message;
            this.CodigoResultado = _CodigoResultado;
            this.objectResp = objectResp;
        }
    }
}