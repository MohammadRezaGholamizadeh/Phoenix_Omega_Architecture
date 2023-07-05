﻿using InfrastructureLayer.BackgroundJobsConfiguration.QuartzConfigurations.QuartzJobs;
using Quartz;

namespace InfrastructureLayer.BackgroundJobsConfiguration.QuartzConfigurations
{
    public static class QuartzJobSetup
    {
        public static IServiceCollectionQuartzConfigurator ConfigureAllQuartzBackgroundJobs(
            this IServiceCollectionQuartzConfigurator configurator)
        {
            configurator.AddJob<SampleJob>("sampleJobIdentity")
                        .AddCroneTrigger("sampleTriggerIdentity", "0/59 * * * * ?");

            return configurator;
        }
    }
}
