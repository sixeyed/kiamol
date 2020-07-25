using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebPingOperator.Installer.Installers
{
    public interface IInstaller
    {
        Task InstallAsync();
    }
}
