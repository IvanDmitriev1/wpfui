using System;
using Microsoft.Extensions.Options;
using WPFUI.DIControls;

namespace WPFUI.Demo;

public class StoreNavigationItemConfiguration : DefaultNavigationConfiguration
{
        
}

public sealed class StoreNavigation : DefaultNavigation
{
    public StoreNavigation(IServiceProvider serviceProvider, IOptions<StoreNavigationItemConfiguration> configureOptions) : base(serviceProvider, configureOptions)
    {

    }
}