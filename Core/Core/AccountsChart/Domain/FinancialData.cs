/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                            Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Abstract class                        *
*  Type     : FinancialData                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Abstract class with financial information for financial accounts.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Abstract class with financial information for financial accounts.</summary>
  abstract public class FinancialData {

    abstract internal string ToJsonString();

  }  // class FinancialData

}  // namespace Empiria.Financial
