# Monaco
Monaco是基于.NET Core开发的微服务项目。作为个人实验项目还处于开发阶段，还有很多不足之处需要进一步完善。

整体架构计划会所用到一些框架或插件：
* Autofac: IOC容器
* AutoMapper: 对象数据转换
* MassTransit: 消息传递及对象缓存，目前使用RabbitMQ作为消息队列。
* Serilog: 结构化日志存储，目前使用SEQ服务器和ElasticSearch作为日志服务器。
* EF Core: ORM
* RedLock: 基于Redis的分布式锁

未来会加入
* IdentityServer: 用户验证和授权服务器，基于OAuth2协议
* Polly: 弹性和瞬态故障处理库
* Refit：Web API 访问接口
