using System;
using System.Linq;

namespace eve_capsim
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = System.IO.File.ReadAllText("caracal.json");
            var ship = Ship.FromJson(json);

            Console.WriteLine($"Loaded Ship Data {ship.Name}:\n\tCapacitor Capacity: {ship.Capacitor.MaxCapacity}\n\tCapacitor Recharge: {ship.Capacitor.RechargeTime}\n\tModules: {ship.Modules.Length}");

            var stepSize = 0.01; // in seconds
            var time = new TimeSpan(); // in seconds
            var maxTime = 20 * 60.0; // 20 min max
            ship.Capacitor.Reset();

            while (ship.Capacitor.CurrentCapacity > 0.0 || time.TotalSeconds >= maxTime) {

                var activeModules = ship.OnlineModules.Where(m => m.isActive).ToList();
                var inactiveModules = ship.OnlineModules.Where(m => !m.isActive).ToList();
                var capCost = inactiveModules.Select(m => m.ActivationCost).Aggregate(0.0, (acc, m) => acc + m);
                double recharge = ship.Capacitor.CurrentRechargeRate * stepSize;
                inactiveModules.ForEach(m => m.ActivateModule());
                activeModules.ForEach(m => m.Step(stepSize));

                ship.Capacitor.CurrentCapacity -= capCost;
                ship.Capacitor.CurrentCapacity += recharge;

                time = time.Add(TimeSpan.FromSeconds(stepSize));
            }

            Console.WriteLine("Capacitor drained in {0} ({1} seconds)", time, time.TotalSeconds);
        }
    }
}
