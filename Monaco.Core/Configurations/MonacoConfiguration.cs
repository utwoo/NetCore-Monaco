using Monaco.Core.Autofac;
using Monaco.Core.Caching;
using Monaco.Core.Data;
using Monaco.Core.MessageQueue;
using Monaco.Core.SEQ;

namespace Monaco.Core.Configurations
{
    public class MonacoConfiguration
    {
        public DataConfiguration DataConfig { get; set; }
        public AutofacConfiguration AutofacConfig { get; set; }
        public RedLockConfiguration RedLockConfig { get; set; }
        public CachingConfiguration CachingConfig { get; set; }
        public RabbitMQConfiguration RabbitMQConfig { get; set; }
        public SEQConfiguration SEQConfig { get; set; }
    }
}
