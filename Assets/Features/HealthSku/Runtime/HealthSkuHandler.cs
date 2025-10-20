using System;
using System.Globalization;
using Core;

namespace Features.HealthSku
{
    internal class HealthSkuHandler : ConsumableSkuHandler
    {
        protected override string GetBalanceString(IConvertible amount)
        {
            var value = base.GetBalanceString(amount);
            return value + "%";
        }

        protected override IConvertible CalculateBalanceFromTransactions()
        {
            var value = base.CalculateBalanceFromTransactions().ToInt32(CultureInfo.InvariantCulture);
            return Math.Min(value, 100);
        }

        public override IConvertible Add(IConvertible a, IConvertible b)
        {
            var value = base.Add(a, b).ToInt32(CultureInfo.InstalledUICulture);
            return Math.Min(value, 100);
        }
    }
}