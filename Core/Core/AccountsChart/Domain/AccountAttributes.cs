/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                            Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Abstract class                        *
*  Type     : AccountAttributes                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Abstract class for financial account attibutes.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Abstract class for financial account attibutes.</summary>
  abstract public class AccountAttributes {

    abstract internal string ToJsonString();

  }  // class AccountAttributes

}  // namespace Empiria.Financial
