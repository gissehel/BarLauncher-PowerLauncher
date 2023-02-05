using System.Collections.Generic;
using BarLauncher.PowerLauncher.Lib.DomainModel;

namespace BarLauncher.PowerLauncher.Lib.Core.Service
{
    public interface IPowerLauncherItemRepository
    {
        void Init();

        IEnumerable<PowerLauncherItem> SearchItems(IEnumerable<string> terms);

        void AddItem(PowerLauncherItem item);

        void RemoveItem(string url);

        PowerLauncherItem GetItem(string url);

        void EditPowerLauncherItem(string url, string newUrl, string newKeywords, string newProfile);
    }
}