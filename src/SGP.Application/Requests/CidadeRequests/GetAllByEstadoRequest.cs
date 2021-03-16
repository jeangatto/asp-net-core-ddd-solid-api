﻿using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequest : BaseRequestWithValidator<GetAllByEstadoRequest>
    {
        public GetAllByEstadoRequest(string estadoSigla) : base(true)
        {
            EstadoSigla = estadoSigla;
        }

        public string EstadoSigla { get; }
    }
}