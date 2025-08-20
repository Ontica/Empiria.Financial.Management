/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditAttributes                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds credit financial account attributes.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Financial {

  /// <summary>Holds credit financial account attributes.</summary>
  public class CreditAttributes : AccountAttributes {

    #region Constructors and Parsers

    private readonly JsonObject _attributes = new JsonObject();
    private readonly FinancialAccount _account;

    public CreditAttributes() {
      // no-op
    }

    internal CreditAttributes(FinancialAccount account, JsonObject attributes) {
      Assertion.Require(account, nameof(account));
      Assertion.Require(attributes, nameof(attributes));

      _account = account;
      _attributes = attributes;
    }

    #endregion Properties

    #region Properties

    public string Borrower {
      get {
        return _attributes.Get("borrower", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("borrower", value);
      }
    }


    public string AccountingAccount {
      get {
        return _attributes.Get("creditAccountingAccount", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("creditAccountingAccount", value);
      }
    }


    public CreditStage CreditStage {
      get {
        return _attributes.Get("creditStageId", CreditStage.Empty);
      }
      internal set {
        _attributes.SetIf("creditStageId", value.Id, !value.IsEmptyInstance);
      }
    }


    public CreditType CreditType {
      get {
        return _attributes.Get("creditTypeId", CreditType.Empty);
      }
      internal set {
        _attributes.SetIf("creditTypeId", value.Id, !value.IsEmptyInstance);
      }
    }


    public string ExternalCreditNo {
      get {
        return _account.SubledgerAccountNo;
        //_attributes.Get("externalCreditNo", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("externalCreditNo", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal override string ToJsonString() {
      return _attributes.ToString();
    }

    #endregion Helpers

  } // class CreditAttributes

} // namespace Empiria.Financial
