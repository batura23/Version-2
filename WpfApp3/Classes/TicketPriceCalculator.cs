using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Classes
{
    public class TicketPriceCalculator
    {
        public enum Show { LittleRedRidingHood, FlyingShip, SwanLake, Quixote, ScarletSails, Nutcracker }
        public enum Zone { VIP, Orchestra, Balcony }

        public double CalculateTotalPrice(Show selectedShow, Zone selectedZone, int ticketCount, int maxTicketCount = 1000)
        {
            if (ticketCount < 0)
                throw new ArgumentOutOfRangeException(nameof(ticketCount), "Количество билетов не может быть отрицательным");
            if (ticketCount > maxTicketCount)
                throw new ArgumentOutOfRangeException(nameof(ticketCount), $"Количество билетов не может превышать {maxTicketCount}");

            double[] showPrices = { 1000, 1200, 1500, 1300, 1100, 1400 };
            double showPrice = showPrices[(int)selectedShow];

            double zoneMultiplier = 1;
            switch (selectedZone)
            {
                case Zone.VIP:
                    zoneMultiplier = 1.5;
                    break;
                case Zone.Orchestra:
                    zoneMultiplier = 1.07;
                    break;
                case Zone.Balcony:
                    zoneMultiplier = 1.2;
                    break;
            }

            double discount = 0;
            if (ticketCount > 30)
                discount = 0.25;
            else if (ticketCount > 20)
                discount = 0.10;
            else if (ticketCount > 15)
                discount = 0.07;
            else if (ticketCount > 10)
                discount = 0.05;

            return showPrice * zoneMultiplier * ticketCount * (1 - discount);
        }
    }
}
