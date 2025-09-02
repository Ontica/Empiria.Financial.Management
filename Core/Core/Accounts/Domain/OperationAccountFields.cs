/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Account                            Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields DTO                      *
*  Type     : OperationAccountFields                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields used to create or update operation accounts.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Input fields used to create or update operation accounts.</summary>
  public class OperationAccountFields {

    #region Properties

    public string BaseAccountUID {
      get; set;
    } = string.Empty;


    [Newtonsoft.Json.JsonProperty(PropertyName = "OperationAccountTypeUID")]
    public string StandardAccountUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string AccountNo {
      get; set;
    } = string.Empty;

    #endregion Properties

    #region Methods

    internal void EnsureValid() {

      BaseAccountUID = Patcher.CleanUID(BaseAccountUID);
      StandardAccountUID = Patcher.CleanUID(StandardAccountUID);
      CurrencyUID = Patcher.CleanUID(CurrencyUID);

      AccountNo = EmpiriaString.Clean(AccountNo);

      if (BaseAccountUID.Length != 0) {
        _ = FinancialAccount.Parse(BaseAccountUID);
      }

      if (StandardAccountUID.Length != 0) {
        _ = StandardAccount.Parse(StandardAccountUID);
      }

      if (CurrencyUID.Length != 0) {
        _ = Currency.Parse(CurrencyUID);
      }
    }

    #endregion Methods

  }  // class OperationAccountFields

}  // namespace Empiria.Financial
