/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                            Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields                          *
*  Type     : CreditExtDataFields                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields with financial credit information.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Accounts {

  /// <summary>Input fields with financial credit information.</summary>
  public class CreditExtDataFields {

    #region Properties

    public string Plazo {
      get; set;
    } = string.Empty;


    public decimal Monto {
      get; set;
    }

    #endregion Properties

    #region Methods

    #endregion Methods

  }  // class CreditExtDataFields

}  // namespace Empiria.Financial.Accounts
