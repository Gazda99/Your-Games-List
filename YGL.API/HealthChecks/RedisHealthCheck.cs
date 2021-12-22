using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using YGL.API.Settings;

namespace YGL.API.HealthChecks;

public class RedisHealthCheck : IHealthCheck {
    private const string HealthCheckTestKey = "health-check-test";
    private const string RedisCacheDisabledMessage = "Redis cache is disabled.";
    private const string RedisCacheNotWorking = "Reds cache not working";
    private const string NoneDescription = "none";
    private readonly RedisCacheSettings _redisCacheSettings;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisHealthCheck(RedisCacheSettings redisCacheSettings,
        IConnectionMultiplexer connectionMultiplexer = null) {
        _redisCacheSettings = redisCacheSettings;

        if (redisCacheSettings.Enabled)
            _connectionMultiplexer = connectionMultiplexer;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken()) {
        if (!_redisCacheSettings.Enabled)
            return Task.FromResult(HealthCheckResult.Unhealthy(RedisCacheDisabledMessage));


        try {
            IDatabase redisDb = _connectionMultiplexer.GetDatabase();
            redisDb.StringGet(HealthCheckTestKey);
            return Task.FromResult(HealthCheckResult.Healthy());
        }

        catch (Exception ex) {
            return Task.FromResult(HealthCheckResult.Unhealthy(RedisCacheNotWorking));
        }
    }
}