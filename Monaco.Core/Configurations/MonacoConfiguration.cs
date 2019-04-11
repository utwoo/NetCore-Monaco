using Monaco.Core.Autofac;
using Monaco.Core.Caching;
using Monaco.Core.Data;
using Monaco.Core.MessageQueue;
using Monaco.Core.SEQ;

namespace Monaco.Core.Configurations
{
    public class MonacoConfiguration
    {
        private MonacoConfiguration() { }

        public readonly static MonacoConfiguration Instance = new MonacoConfiguration();

        public DataConfiguration DataConfig { get; internal set; }
        public AutofacConfiguration AutofacConfig { get; internal set; }
        public RedLockConfiguration RedLockConfig { get; internal set; }
        public CachingConfiguration CachingConfig { get; internal set; }
        public RabbitMQConfiguration RabbitMQConfig { get; internal set; }
        public SEQConfiguration SEQConfig { get; internal set; }
    }
}
