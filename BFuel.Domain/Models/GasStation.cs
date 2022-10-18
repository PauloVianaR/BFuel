using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BFuel.Domain.Models
{
    public class GasStation
    {
        [BsonId()]
        public ObjectId _id { get; set; }

        [BsonElement("regiao")]
        [BsonRequired()]
        public string Regiao { get; set; }

        [BsonElement("estado")]
        [BsonRequired()]
        public string Estado { get; set; }

        [BsonElement("municipio")]
        [BsonRequired()]
        public string Municipio { get; set; }

        [BsonElement("revenda")]
        [BsonRequired()]
        public string Revenda { get; set; }

        [BsonElement("cnpj")]
        [BsonRequired()]
        public string CNPJ { get; set; }

        [BsonElement("rua")]
        [BsonRequired()]
        public string Rua { get; set; }

        [BsonElement("numero")]
        [BsonRequired()]
        public string Numero { get; set; }

        [BsonElement("complemento")]
        [BsonRequired()]
        public string Complemento { get; set; }

        [BsonElement("bairro")]
        [BsonRequired()]
        public string Bairro { get; set; }

        [BsonElement("cep")]
        [BsonRequired()]
        public string CEP { get; set; }

        [BsonElement("produto")]
        [BsonRequired()]
        public string Produto { get; set; }

        [BsonElement("data_coleta")]
        [BsonRequired()]
        public string Data_coleta { get; set; }

        [BsonElement("valor")]
        [BsonRequired()]
        public string Valor { get; set; }

        [BsonElement("bandeira")]
        [BsonRequired()]
        public string Bandeira { get; set; }
    }
}
