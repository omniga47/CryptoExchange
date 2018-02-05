using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLayer.Handler
{
    public class HandlerResponse
    {
        public HandlerResponse() { }

        public HandlerResponse(int id, string message)
        {
            this.MessageId = id;
            this.Message = message;
        }

        public HandlerResponse(int id, string message, object result)
        {
            this.MessageId = id;
            this.Message = message;
            this.Result = result;
        }

        public HandlerResponse(int id, string message, int pageId, object result)
        {
            this.MessageId = id;
            this.Message = message;
            this.Result = result;
            this.PageId = pageId;
        }

        public int MessageId { get; set; }

        public string Message { get; set; }
        public int PageId { get; set; }

        public object Result { get; set; }
    }
}
