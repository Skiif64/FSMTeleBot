using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMTeleBot.Tests.Integration;
internal abstract class HostBuilder
{
    public abstract IServiceProvider BuildServiceProvider();
}
