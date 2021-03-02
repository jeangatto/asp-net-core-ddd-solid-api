﻿using SGP.Domain.Enums;
using SGP.Shared.Entities;
using System;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Estado : BaseEntity
    {
        public Estado(Guid paisId, string nome, string uf, short ibge, Regiao regiao)
        {
            PaisId = paisId;
            Nome = nome;
            Sigla = uf;
            Ibge = ibge;
            Regiao = regiao;
        }

        private Estado()
        {
        }

        public Guid PaisId { get; private set; }
        public string Nome { get; private set; }
        public string Sigla { get; private set; }
        public short Ibge { get; private set; }
        public Regiao Regiao { get; private set; }

        public Pais Pais { get; private set; }
        public IReadOnlyList<Cidade> Cidades { get; private set; }
    }
}