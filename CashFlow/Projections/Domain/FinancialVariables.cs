/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                       Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Projections.dll           Pattern   : Service provider                        *
*  Type     : FinancialVariables                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides financial variables for cash flow projections.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

using Empiria.DynamicData.ExternalData;
using Empiria.DynamicData.ExternalData.UseCases;

namespace Empiria.CashFlow.Projections {

  /// <summary>Provides financial variables for cash flow projections.</summary>
  static internal class FinancialVariables {

    static internal decimal GetExchangeRate(int year, int month, Currency currency) {

      if (Currency.Default.Equals(currency)) {
        return decimal.One;
      }

      var variable = ExternalVariable.TryParseWithCode(currency.ISOCode);

      ExternalValue value = ExternalValuesUseCases.UseCaseInteractor()
                                                  .GetExternalValue(variable, year);

      if (value == null) {
        return decimal.One;
      }

      string monthStr = EmpiriaString.MonthName(month, true)
                                     .ToLower();

      return value.GetTotalField(monthStr);
    }


    static internal decimal[] GetExchangeRates(int year, Currency currency) {

      if (Currency.Default.Equals(currency)) {
        return new decimal[12] { 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m };
      }

      var variable = ExternalVariable.TryParseWithCode(currency.ISOCode);

      ExternalValue value = ExternalValuesUseCases.UseCaseInteractor()
                                                  .GetExternalValue(variable, year);

      return new decimal[12] {
        value.GetTotalField("ene"),
        value.GetTotalField("feb"),
        value.GetTotalField("mar"),
        value.GetTotalField("abr"),
        value.GetTotalField("may"),
        value.GetTotalField("jun"),
        value.GetTotalField("jul"),
        value.GetTotalField("ago"),
        value.GetTotalField("sep"),
        value.GetTotalField("oct"),
        value.GetTotalField("nov"),
        value.GetTotalField("dic")
      };
    }

  }  // class FinancialVariables

}  // namespace Empiria.CashFlow.Projections
