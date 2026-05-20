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

namespace Empiria.CashFlow.Projections {

  /// <summary>Provides financial variables for cash flow projections.</summary>
  static internal class FinancialVariables {

    static internal decimal GetExchangeRate(int year, int month, Currency currency) {

      if (Currency.Default.Equals(currency)) {
        return decimal.One;
      }

      return 18.64m;
    }

  }  // class FinancialVariables

}  // namespace Empiria.CashFlow.Projections
