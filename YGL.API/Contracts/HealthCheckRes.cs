using System;
using System.Collections.Generic;
using YGL.API.HealthChecks;

namespace YGL.API.Contracts {
public class HealthCheckRes {
    public string Status { get; set; }
    public IEnumerable<HealthCheck> Checks { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime Date { get; set; }
}
}