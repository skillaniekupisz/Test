using Newtonsoft.Json;

namespace Games.Core.Entties
{
    public abstract class EntityBase
    {
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}
