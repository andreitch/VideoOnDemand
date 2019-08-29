using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace VOD.Common.Exceptions
{
    public class HttpResponseException: Exception
    {
        #region Variables
        public HttpStatusCode HttpStatusCode { get; }
        public object ValidationErrors { get; }
        #endregion

        #region Constructors
        public HttpResponseException(HttpStatusCode status, string message,
            object validationErrors) : base(message)
        {
            HttpStatusCode = status;
            ValidationErrors = validationErrors;
        }
        public HttpResponseException(HttpStatusCode status, string message)
            : this(status, message, null)
        {
            HttpStatusCode = status;
        }
        public HttpResponseException(HttpStatusCode status)
            : this(status, string.Empty, null)
        {
            HttpStatusCode = status;
        }
        #endregion
    }
}
