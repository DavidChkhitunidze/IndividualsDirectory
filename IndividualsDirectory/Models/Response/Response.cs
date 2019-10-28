using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Response
{
    public class Response
    {
        public bool Success { get; private set; }

        public string SuccessMessage { get; private set; }

        public string[] ErrorMessages { get; private set; }

        public void SetSuccess(string successMessage = null)
        {
            SuccessMessage = successMessage;
            Success = true;
        }
        public void SetErrorMessages(params string[] errorMessage) => ErrorMessages = errorMessage;
    }

    public class Response<TViewModel> : Response
    {
        public TViewModel Model { get; private set; }

        public void SetModel(TViewModel model) => Model = model;
    }
}
